//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #游戏的管理类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Sunny
{
    public class GameMode : MonoBehaviour
    {
		#region 属性
		public static EventManager Event;
        public static GameStateManager State;
        public static NodeManager Node;
        public static DataTableManager DataTable;
        public static ResourceManager Resource;
        public static ScenarioManager Scene;
        public static UIManager UI;
        public static WebRequestManager WebRequest;
		public static AudioManager Audio;
	    public static LocalizationManager Localization;
		public static SettingManager Setting;
        public static SystemManager System;
        public static NetworkManager Network;
		public static PoolManager Pool;
        public static CoroutineManager Coroutines;

        public static HotFixLuaManager HotFixLua;
        public static HotFixILRuntimeManager HotFixILRuntime;


        /// <summary>
        /// 当前程序集
        /// </summary>
        public static System.Reflection.Assembly Assembly { get; private set; }

#region 资源
        /// <summary>
        /// 资源加载方式 默认为编辑器加载
        /// </summary>
        // public bool IsEditorMode = true;

        /// <summary>
        /// 资源更新类型
        /// </summary>
        public ResourceUpdateType ResUpdateType = ResourceUpdateType.Editor;

	    /// <summary>
	    /// 资源本地路径
	    /// </summary>
	    public PathType LocalPathType = PathType.ReadOnly;

		/// <summary>
		/// 资源更新的路径
		/// </summary>
		public string ResUpdatePath = "";

        /// <summary>
        /// 是否开启调试器
        /// </summary>
        public bool DebugEnable = true;

        private int m_FrameRate = 30;
        private float m_GameSpeed = 1.0f;

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public int FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                Application.targetFrameRate = m_FrameRate = value;
            }
        }

        /// <summary>
        /// 获取或设置游戏速度。
        /// </summary>
        public float GameSpeed
        {
            get
            {
                return m_GameSpeed;
            }
            set
            {
                Time.timeScale = m_GameSpeed = (value >= 0f ? value : 0f);
            }
        }

        /// <summary>
        /// UI根节点
        /// </summary>
        public GameObject UIRoot;
#endregion

#endregion

        private void Awake()
        {
            PrintFrameworkInfo();
        }

        IEnumerator Start()
		{
			//默认不销毁
			DontDestroyOnLoad(gameObject);

#region Module
            Event = GameFrameworkMode.GetModule<EventManager>();
            State = GameFrameworkMode.GetModule<GameStateManager>();
            Node = GameFrameworkMode.GetModule<NodeManager>();
            DataTable = GameFrameworkMode.GetModule<DataTableManager>();
            Resource = GameFrameworkMode.GetModule<ResourceManager>();
            Scene = GameFrameworkMode.GetModule<ScenarioManager>();
            UI = GameFrameworkMode.GetModule<UIManager>();
            WebRequest = GameFrameworkMode.GetModule<WebRequestManager>();
			Audio = GameFrameworkMode.GetModule<AudioManager>();
			Localization = GameFrameworkMode.GetModule<LocalizationManager>();
			Setting = GameFrameworkMode.GetModule<SettingManager>();
		    System= GameFrameworkMode.GetModule<SystemManager>();
		    Network= GameFrameworkMode.GetModule<NetworkManager>();
			Pool = GameFrameworkMode.GetModule<PoolManager>();
            Coroutines = GameFrameworkMode.GetModule<CoroutineManager>();
            HotFixLua = GameFrameworkMode.GetModule<HotFixLuaManager>();
            HotFixILRuntime = GameFrameworkMode.GetModule<HotFixILRuntimeManager>();
#endregion

#region Resource
            Resource.ResUpdateType = ResUpdateType;
	        Resource.ResUpdatePath = ResUpdatePath;
	        Resource.LocalPathType = LocalPathType;

			//添加对象池管理器
	        GameObject gameObjectPoolHelper = new GameObject("IGameObjectPoolHelper");
	        gameObjectPoolHelper.transform.SetParent(transform);
	        Resource.SetGameObjectPoolHelper(gameObjectPoolHelper.AddComponent<GameObjectPoolHelper>());
#endregion

#region UI
            // 如果没有主动绑定UI节点,那么就自动寻找当前场景Canvas组件
            if (UIRoot == null)
            {
                UIRoot = FindObjectOfType<Canvas>().gameObject;
            }
            if(UIRoot!=null)
                UI.InitUIRoot(UIRoot);
#endregion

#region Auido
            //设置音频播放
            GameObject audioPlayer = new GameObject("AudioSourcePlayer");
			audioPlayer.transform.SetParent(transform);
			//添加AduioSource
			Audio.SetDefaultAudioSource(audioPlayer.AddComponent<AudioSource>(), audioPlayer.AddComponent<AudioSource>(),
				audioPlayer.AddComponent<AudioSource>());
#endregion

#region WebRequest
			//设置帮助类
			GameObject webRequestHelper = new GameObject("IWebRequestHelper");
	        webRequestHelper.transform.SetParent(transform);
	        GameObject webDownloadHelper = new GameObject("IWebDownloadMonoHelper");
	        webDownloadHelper.transform.SetParent(transform);
			WebRequest.SetWebRequestHelper(webRequestHelper.AddComponent<WebRquestMonoHelper>());
            WebRequest.SetWebDownloadHelper(webDownloadHelper.AddComponent<WebDownloadMonoHelper>());
#endregion

#region Coroutine
            GameObject coroutineRoot = new GameObject("CoroutineRoot");
            coroutineRoot.transform.SetParent(transform);
            Coroutines.Initialize(coroutineRoot);
#endregion

#region Setting
            /*Transform debugT = transform.Find("[Graphy]");
            if (debugT != null)
            {
                GameObject debugHelper = debugT.gameObject;
                Setting.SetDebuger(debugHelper);
                Setting.DebugEnable = DebugEnable;
            }*/
            GameObject debugHelper = new GameObject("DebugHelper");
            debugHelper.transform.SetParent(transform);
            Setting.SetDebuger(debugHelper.AddComponent<DebugHelper>().gameObject);
            Setting.DebugEnable = DebugEnable;
            // 帧率
            FrameRate = m_FrameRate;
#endregion

#region State
            //开启整个项目的流程
            Assembly = typeof(GameMode).Assembly;
            State.CreateContext(Assembly);
            yield return new WaitForEndOfFrame();
            State.SetStateStart();
#endregion

        }
        
        private void PrintFrameworkInfo()
        {
            Debug.Log(string.Format("Game Framework Version: {0},Unity Version: {1}", GameModeVersion.GameFrameworkVersion, Application.unityVersion));
        }

		private void Update()
		{
			GameFrameworkMode.Update();
		}

		private void FixedUpdate()
		{
			GameFrameworkMode.FixedUpdate();
		}

		private void OnDestroy()
		{
			GameFrameworkMode.ShutDown();
		}
	}
}
