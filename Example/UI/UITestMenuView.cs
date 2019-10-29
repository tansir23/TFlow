using GameFramework.Taurus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Example.UI
{
	[UIView ("ui", GlobalManager.UIPrefabRootPath+"UITestMenuView")]
	public class UITestMenuView : UIView
	{
		public Button btn_UI;
        public Button btn_Scene;
        public Button btn_dataTable;
        public Button btn_dataNode;
        public Button btn_Event;
        public Button btn_WebRequst;
        public Button btn_Language;

        public override void OnInitUI ()
		{
            //监听自定义事件
            GameMode.Event.AddListener<TestEventArgs>(OnTestEventCallback);
            //监听网络请求成功
            GameMode.Event.AddListener<HttpResponseSuccessEventArgs>(OnHttpResponseSuccess);
            //网络请求失败
            GameMode.Event.AddListener<HttpResponseFailEventArgs>(OnHttpResponseFail);

            btn_UI.onClick.AddListener (() => {
				GameMode.UI.OpenUI<UITestView>();
			});

            btn_Scene.onClick.AddListener(async () => {
                AsyncOperation asyncOperation = await GameMode.Scene.LoadSceneAsync("scene", "Test1",UnityEngine.SceneManagement.LoadSceneMode.Single);

                GameMode.Event.AddListener<SceneLoadingEventArgs>(OnSceneLoadingCallbak);
                GameMode.Event.AddListener<SceneLoadedEventArgs>(OnSceneLoadedCallbak);
            });

            

            btn_dataTable.onClick.AddListener(() => {
                GameMode.DataTable.LoadDataTable<StarForce.DRScene>("Scene", GlobalManager.DataTableRootPath + "Scene");
               // 获得数据表
               IDataTable<StarForce.DRScene> dtScene = GameMode.DataTable.GetDataTable<StarForce.DRScene>();
               // 获得所有行
               StarForce.DRScene[] drScenes = dtScene.GetAllDataRows();
               // 根据行号获得某一行
               StarForce.DRScene drScene = dtScene.GetDataRow(2); // 或直接使用 dtScene[1]
               if (drScene != null)
               {
                   // 此行存在，可以获取内容了
                   //string name = drScene.Name;
                   string assetName = drScene.AssetName;
                   int backgroundMusicId = drScene.BackgroundMusicId;
                   Debug.Log("AssetName:"+ assetName);
               }
            });

            btn_dataNode.onClick.AddListener(() => {
                GameMode.Node.Set("Test",1);
                GameMode.Node.Set("Test", false);
                GameMode.Node.Set("Test.1", 1);
                GameMode.Node.Set("Test0", 10000000);
                GameMode.Node.Set("Test1", 1.9999f);
                GameMode.Node.Set("Test1", 1.99900000009f);
                GameMode.Node.Set("Test2", "str");
                GameMode.Node.Set("Test3", true);
                GameMode.Node.Set("Test4", false);
                TestDataNode a = new TestDataNode();
                GameMode.Node.Set("Test4", a);
            });

            btn_Event.onClick.AddListener(() =>
            {
                GameMode.Event.Trigger(this, new TestEventArgs() { Parameters = "TriggerEvent"});
            });

            btn_WebRequst.onClick.AddListener(() =>
            {
                //Get请求
                //GameMode.WebRequest.RequestHttpGet("http://unionsug.baidu.com/su?wd=js&cb=baiduSU%27");
                GameMode.WebRequest.Get("http://unionsug.baidu.com/su?wd=js&cb=baiduSU%271").OnSuccess(response=> { Debug.Log(response.Text); }).Send();
                GameMode.WebRequest.Get("http://unionsug.baidu.com/su?wd=js&cb=baiduSU%271").Send();
            });

            btn_Language.onClick.AddListener(()=>{
                if (LocalizationManager.Language == Language.English)
                    GameMode.Localization.SetLanguage(Language.Chinese);
                else
                    GameMode.Localization.SetLanguage(Language.English);
            });
        }

        private IEnumerator test()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("IEnumerator Test");
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="parameters">不确定参数</param>
        public override void OnShow (params object[] parameters)
		{
            GameMode.Coroutines.Start(test());
        }

		/// <summary>
		/// 退出界面
		/// </summary>
		public override void OnClose ()
		{
		}

		public override void OnHide ()
		{
		}

		/// <summary>
		/// 暂停界面
		/// </summary>
		public override void OnPause ()
		{
		}

		/// <summary>
		/// 恢复界面
		/// </summary>
		public override void OnResume ()
		{
		}


        #region Scene Load Callback
        private void OnSceneLoadingCallbak(object sender, IEventArgs e)
        {
            SceneLoadingEventArgs ne = (SceneLoadingEventArgs)e;
            Debug.Log("OnSceneLoadingCallbak:" + ne.SceneName + ",Progress:"+ne.Progress);
        }
        private void OnSceneLoadedCallbak(object sender, IEventArgs e)
        {
            SceneLoadedEventArgs ne = (SceneLoadedEventArgs)e;
            Debug.Log("OnSceneLoadedCallbak:" + ne.SceneName);
        }
        #endregion

        private void OnTestEventCallback(object sender, IEventArgs e)
        {
            TestEventArgs ne = (TestEventArgs)e;
            Debug.Log("OnTestEventCallback:" + ne.Parameters);
        }

        private void OnHttpResponseSuccess(object sender, IEventArgs e)
        {
            HttpResponseSuccessEventArgs ne = (HttpResponseSuccessEventArgs)e;
            Debug.Log("OnHttpReadTextSuccess:url:" + ne.Url+",content:"+ne.Content);
        }
        private void OnHttpResponseFail(object sender, IEventArgs e)
        {
            HttpResponseFailEventArgs ne = (HttpResponseFailEventArgs)e;
            Debug.Log("OnHttpReadTextFailEventArgs:url:" + ne.Url+",error:"+ ne.Error);
        }
    }


    class TestDataNode
    {
        int a = 0;
    }

    public class TestEventArgs : GameEventArgs<TestEventArgs>
    {
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters;
    }

}
