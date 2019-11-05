//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #Lua热更新管理器# </describe>
//-----------------------------------------------------------------------

using System;
using UnityEngine;

#if HOTFIX_XLUA
using XLua;
#endif

namespace GameFramework.Sunny
{
#if HOTFIX_XLUA
    [LuaCallCSharp]
#endif
    public sealed class HotFixLuaManager : GameFrameworkModule, IUpdate
    {
        //lua的环境变量
#if HOTFIX_XLUA
        public LuaEnv LuaEnv = new LuaEnv();
        private LuaTable _scriptEnv;
#endif

        //资源管理器
        private ResourceManager _resource;
        //lua assetbundle 名称
        private string _luaAssetBundle;
        //lua脚本前缀
        private string _luaPathPrefix;
        //lua脚本扩展名
        private string _luaPathExtension;

        Action _start;
        Action _update;
        Action _close;

        //lua 计时
        private static float _lastGCTime = 0.0f;
        //lua tick的间隔时间
        private const float _luaTickInterval = 1.0f;

        public HotFixLuaManager()
        {
            _resource = GameFrameworkMode.GetModule<ResourceManager>();

        }

        public void SetConfiguration(string assetBundle = "hotfix",
            string luaPathPrefix = "Assets/Game/HotFix", string luaPathExtension = ".lua.txt") {
            _luaAssetBundle = assetBundle;
            _luaPathPrefix = luaPathPrefix;
            _luaPathExtension = luaPathExtension;

        }
        /// <summary>
        /// 加载热更新脚本
        /// </summary>
        /// <param name="assetBundle"></param>
        /// <param name="luaScript"></param>
        /// <param name="luaPathPrefix"></param>
        /// <param name="luaPathExtension"></param>
        public async void LoadHotFix(string assetBundle="hotfix",string luaScript="main",
			string luaPathPrefix="Assets/Game/HotFix",string luaPathExtension=".lua.txt")
		{
#if HOTFIX_XLUA

			_luaAssetBundle = assetBundle;
			_luaPathPrefix = luaPathPrefix;
			_luaPathExtension = luaPathExtension;

            if(_resource.ResUpdateType != ResourceUpdateType.Editor)
                await _resource.LoadAssetBundle(_luaAssetBundle);

			_scriptEnv = LuaEnv.NewTable();
			
			// 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
			LuaTable meta = LuaEnv.NewTable();
			meta.Set("__index", LuaEnv.Global);
			_scriptEnv.SetMetaTable(meta);
			meta.Dispose();

			_scriptEnv.Set("self", this);
			LuaEnv.AddLoader(CustomLoader);
			LuaEnv.DoString($"require '{luaScript}'", luaScript, _scriptEnv);
			_scriptEnv.Get("Start", out _start);
			_scriptEnv.Get("Update", out _update);
			_scriptEnv.Get("Close", out _close);

			_start?.Invoke();

#endif
		}

		//自定义加载
		private byte[] CustomLoader(ref string filePath)
		{
			string path = System.IO.Path.Combine(_luaPathPrefix, $"{filePath}{_luaPathExtension}");
			TextAsset textAsset = _resource.LoadAssetSync<TextAsset>(_luaAssetBundle, path);
			return textAsset.bytes;
		}

		/// <summary>
		/// 加载lua的文本
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string LuaScriptLoader(string name)
		{
            string path = System.IO.Path.Combine(_luaPathPrefix, $"{name}{_luaPathExtension}");
            return _resource.LoadAssetSync<TextAsset>(_luaAssetBundle, path).text;
		}

		public void OnUpdate()
		{
			_update?.Invoke();

			//每隔一段时间对lua进行一次GC回收
			if (Time.time - _lastGCTime > _luaTickInterval)
			{
#if HOTFIX_XLUA
                LuaEnv.Tick();
#endif
				_lastGCTime = Time.time;
			}
		}
		
		public override void OnClose()
		{
			_close?.Invoke();
			//_luaEnv?.Dispose();
		}
	}
}
