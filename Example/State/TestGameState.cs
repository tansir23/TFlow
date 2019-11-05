//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #测试状态# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using GameFramework.Example.UI;
using GameFramework.Sunny;
using UnityEngine;

namespace GameFramework.Example.State
{
	[GameState]
	public class TestGameState : GameState
	{
		#region 重写函数
		public override void OnEnter(params object[] parameters)
		{
			base.OnEnter(parameters);
			Debug.Log ("Enter Test State");
            LoadResourceAsync();
        }

        private async void LoadResourceAsync()
        {
            //await GameMode.Resource.LoadAssetBundle("ui");
            //await GameMode.Resource.LoadAssetBundle("cube");

            GameObject cube = await GameMode.Resource.LoadAsset<GameObject>("cube", GlobalManager.ModelRootPath + "Cube");
            if (cube != null)
            {
                GameObject obj = GameObject.Instantiate(cube);
                obj.transform.position = Vector3.zero;
            }
            // ChangeState<PreloadState>();
            GameMode.Localization.SetConfiguration("language", GlobalManager.LanguageRootPath);
            GameMode.HotFixLua.SetConfiguration("hotfix", GlobalManager.HotfixRootPath, ".lua");
            GameMode.UI.OpenUI<UITestMenuView>();
            
  
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
	}
}
