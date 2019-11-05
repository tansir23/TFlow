//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #加载资源状态# </describe>
//-----------------------------------------------------------------------

using GameFramework.Example.UI;
using GameFramework.Sunny;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameFramework.Example.State
{
	[GameState]
	public class LoadResourceState : GameState
	{
		#region 重写函数
		public override void OnEnter(params object[] parameters)
		{
			base.OnEnter(parameters);
            
		    string localPath = Path.Combine(GameMode.Resource.LocalPath, "AssetVersion.txt");
		    AssetBundleVersionInfo versionInfo = JsonUtility.FromJson<AssetBundleVersionInfo>(File.ReadAllText(localPath));
            
            //设置ab包的加载方式
            GameMode.Resource.SetResourceHelper(new BundleResourceHelper());

            //加载ab包的mainfest文件
		    GameMode.Resource.SetMainfestAssetBundle(versionInfo.ManifestAssetBundle, versionInfo.IsEncrypt);

            // 载入AB资源
            LoadResourceAsync();


        }

        private async void LoadResourceAsync()
        {
            await GameMode.Resource.LoadAssetBundle("ui");

            await GameMode.Resource.LoadAssetBundle("datatable");
            await GameMode.Resource.LoadAssetBundle("language");

            GameObject cube = await GameMode.Resource.LoadAsset<GameObject>("cube", GlobalManager.ModelRootPath + "Cube.Prefab");
            GameObject sphere = await GameMode.Resource.LoadAsset<GameObject>("cube", GlobalManager.ModelRootPath + "Sphere.Prefab");
            if (cube != null)
            {
                GameObject obj = GameObject.Instantiate(cube);
                obj.transform.position = Vector3.zero;

                GameObject obj1 = GameObject.Instantiate(sphere);
                obj1.transform.position = Vector3.zero;
            }

            GameMode.Localization.SetConfiguration("language", GlobalManager.LanguageRootPath);




            //GameObject ui = await GameMode.Resource.LoadAsset<GameObject>("ui", "assets/dependency/tflowframework/example/resources/prefab/ui/uitestmenuview.prefab");
            //if (ui != null)
            //{
            GameMode.UI.OpenUI<UITestMenuView>();
            //}
            //GameMode.UI.OpenUI<UITestMenuView>();

            ChangeState<PreloadState>();
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
