//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2018 Zhang Yang. All rights reserved.
// </copyright>
// <describe> #加载热更新状态# </describe>
// <email> yeozhang@qq.com </email>
// <time> #2018年7月15日 16点47分# </time>
//-----------------------------------------------------------------------

using GameFramework.Sunny;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Example.State
{
	[GameState]
	public class LoadHotfixState : GameState
	{

		#region 重写函数
		public override void OnEnter(params object[] parameters)
		{
			base.OnEnter(parameters);

            //加载热更新
            LoadHotFix_Lua();
            //LoadHotFix_ILruntime();


        }

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
		}

		public override void OnInit()
		{
			base.OnInit();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

         }
        #endregion

        #region 内部函数

        private void LoadHotFix_Lua()
		{

            //GameMode.HotFixLua.LoadHotFix("hotfix","main",GlobalManager.HotfixRootPath,".lua.txt");
            //GameMode.HotFixLua.LoadHotFix("hotfix", "uimenu", GlobalManager.HotfixRootPath, ".lua.txt");
            GameMode.HotFixLua.SetConfiguration("hotfix", GlobalManager.HotfixRootPath, ".lua.txt");
        }

        private async void LoadHotFix_ILruntime()
        {
            //热更新的资源
            string _dllPath = GlobalManager.HotfixRootPath + "HotFix_Project.dll.bytes";
            string _pdbPath = GlobalManager.HotfixRootPath + "HotFix_Project.pdb.bytes";

            byte[] dllDatas = (await GameMode.Resource.LoadAsset<TextAsset>("hotfix", _dllPath)).bytes;
            byte[] pdbDatas = null;

#if UNITY_EDITOR
            //pdbDatas = GameMode.Resource.LoadAsset<TextAsset>("hotfix", _pdbPath)?.bytes;
            //GameMode.HotFix.Appdomain.DebugService.StartDebugService(56000);
#endif
            GameMode.HotFixILRuntime.LoadHotfixAssembly(dllDatas, pdbDatas);
        }
		#endregion

	}
}
