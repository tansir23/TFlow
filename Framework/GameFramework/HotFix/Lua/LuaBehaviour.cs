//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #lua的运行脚本# </describe>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if HOTFIX_XLUA
using XLua;
#endif

namespace GameFramework.Sunny
{
    

    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

#if HOTFIX_XLUA
	[LuaCallCSharp]
#endif
    public class LuaBehaviour : MonoBehaviour
	{
        // Lua热更新管理器
        private HotFixLuaManager _hotfix;

        /// <summary>
        /// 需要赋值的物体
        /// </summary>
        public Injection[] injections;

        public string luaScriptName;

        //lua脚本
        private string _luaScript;

#if HOTFIX_XLUA
        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        private LuaTable scriptEnv;
#endif
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 
        

        private Action _luaStart;
		private Action _luaClose;
		private Action _luaUpdate;
		private Action _luaEnable;
		private Action _luaDisable;

        private void Awake()
        {
            Run();
        }


        /// <summary>
        /// 运行lua的脚本
        /// </summary>
        public void Run()
		{
            _hotfix = GameFrameworkMode.GetModule<HotFixLuaManager>();

#if HOTFIX_XLUA
            if (string.IsNullOrEmpty(_luaScript))
			{
				scriptEnv = luaEnv.NewTable();

                // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
                LuaTable meta = luaEnv.NewTable();
                meta.Set("__index", luaEnv.Global);
                scriptEnv.SetMetaTable(meta);
                meta.Dispose();

                scriptEnv.Set("self", this);
				foreach (var injection in injections)
				{
					scriptEnv.Set(injection.name, injection.value);
				}

				string luaScript = _hotfix.LuaScriptLoader(luaScriptName);

                luaEnv.DoString(luaScript, luaScriptName, scriptEnv);

                Action luaAwake = scriptEnv.Get<Action>("Awake");
				scriptEnv.Get("Start", out _luaStart);
				scriptEnv.Get("Update", out _luaUpdate);
				scriptEnv.Get("Close", out _luaClose);
				scriptEnv.Get("Enable", out _luaEnable);
				scriptEnv.Get("Disable", out _luaDisable);

				_luaScript = luaScriptName;

                luaAwake?.Invoke();
            }
#endif

        }

        private void Start()
        {
            _luaStart?.Invoke();
        }

        private void OnEnable()
		{
            _luaEnable?.Invoke();
        }

		private void OnDisable()
		{
            _luaDisable?.Invoke();
        }
		private void Update()
		{
            _luaUpdate?.Invoke();

            if (Time.time - lastGCTime > GCInterval)
            {
#if HOTFIX_XLUA
                luaEnv.Tick();
#endif
                lastGCTime = Time.time;
            }
        }
		private void OnDestroy()
		{
            _luaClose?.Invoke();

            _luaClose = null;
            _luaUpdate = null;
            _luaStart = null;
#if HOTFIX_XLUA
            scriptEnv.Dispose();
#endif
            injections = null;
        }
	}
}
