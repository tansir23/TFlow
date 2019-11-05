//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #ui管理类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameFramework.Sunny
{
    public enum UILevel
    {
        AlwayBottom = -3, //如果不想区分太复杂那最底层的UI请使用这个
        Bg = -2, //背景层UI
        AnimationUnderPage = -1, //动画层
        Common = 0, //普通层UI
        AnimationOnPage = 1, // 动画层
        PopUI = 2, //弹出层UI
        Guide = 3, //新手引导层
        Const = 4, //持续存在层UI
        Toast = 5, //对话框层UI
        Forward = 6, //最高UI层用来放置UI特效和模型
        AlwayTop = 7, //如果不想区分太复杂那最上层的UI请使用这个
    }

    public sealed class UIManager : GameFrameworkModule
    {
        private GameObject UIRoot;

		//事件管理器
		private EventManager _event;
		private UIShowEventArgs _uiShowArgs;
		private UICloseEventArgs _uiCloseArgs;
		private UIPauseEventArgs _uiPauseArgs;
		private UIResumeEventArgs _uiResumeArgs;
        private UIInitEventArgs _uiInitArgs;
        private UIHideEventArgs _uiHideArgs;

        //资源管理器
        private ResourceManager _resource;
		//ui堆栈
		private Stack<AssetConfig> _stackUiAsset = new Stack<AssetConfig>();
		//所有的uiview
		private readonly Dictionary<AssetConfig, UIView> _allUiViews = new Dictionary<AssetConfig, UIView>();
		//默认的uipath路径
        private readonly Dictionary<int, AssetConfig> _uiAssetPath =
            new Dictionary<int, AssetConfig>();
		//所有的uiAsset
		private readonly Dictionary<int, AssetConfig> _allUiAssets = new Dictionary<int, AssetConfig>();
		#region 构造函数
		public UIManager()
		{
			//获取资源模块
			_resource = GameFrameworkMode.GetModule<ResourceManager>();
			//获取事件模块
			_event = GameFrameworkMode.GetModule<EventManager>();

            _uiInitArgs = new UIInitEventArgs();
            _uiShowArgs = new UIShowEventArgs();
			_uiCloseArgs = new UICloseEventArgs();
			_uiPauseArgs = new UIPauseEventArgs();
			_uiResumeArgs = new UIResumeEventArgs();
            _uiHideArgs = new UIHideEventArgs();

        }
		#endregion

        /// <summary>
        /// 初始化UIRoot
        /// </summary>
        /// <param name="_UIRoot"></param>
        public void InitUIRoot(GameObject _UIRoot)
        {
            UIRoot = _UIRoot;
        }

		#region 外部接口
		public async void Push<T>(bool allowMulti = false, params object[] parameters) where T : UIView
		{
		    AssetConfig assetConfig = CheckAssetPath(typeof(T));
			if (assetConfig==null)
				return;
            
			if (_stackUiAsset.Count > 0)
			{
			    AssetConfig lastAssetConfig = _stackUiAsset.Peek();
				//如果界面已经打开 则不在执行
				if (Equals(lastAssetConfig, assetConfig) && !allowMulti)
					return;

				IUIView uiView = await GetUiView(lastAssetConfig);

				//触发暂停事件
				_uiPauseArgs.UIView = uiView;
				_event.Trigger(this, _uiPauseArgs);

				uiView.OnPause();
			}

		    AssetConfig newAssetConfig = null;
		    if (allowMulti)
		        newAssetConfig = new AssetConfig(assetConfig.AssetBundleName, assetConfig.AssetPath);
		    else
		        newAssetConfig = assetConfig;

            _stackUiAsset.Push(newAssetConfig);
			UIView newUiView = await GetUiView(newAssetConfig);
			newUiView.OnShow(parameters);

			//触发打开事件
			_uiShowArgs.UIView = newUiView;
			_event.Trigger(this, _uiShowArgs);
		}


		public void Pop(bool isDestory = false)
		{
			//移除当前UI
			if (_stackUiAsset.Count > 0)
			{
			    AssetConfig lastAssetConfig = _stackUiAsset.Pop();
                UIView lastUiView;
				if (_allUiViews.TryGetValue(lastAssetConfig, out lastUiView))
				{
					//触发关闭事件
					_uiCloseArgs.UIView = lastUiView;
					_event.Trigger(this, _uiCloseArgs);

					lastUiView.OnClose();
					if (isDestory)
					{
						_allUiViews.Remove(lastAssetConfig);
						MonoBehaviour.Destroy(lastUiView);
					}
					else
						lastUiView.gameObject.SetActive(false);
				}
			}

			if (_stackUiAsset.Count > 0)
			{
			    AssetConfig lastAssetConfig = _stackUiAsset.Peek();
                UIView lastUiView;
			    if (_allUiViews.TryGetValue(lastAssetConfig, out lastUiView))
			    {
			        lastUiView.OnResume();
			        //触发恢复事件
			        _uiResumeArgs.UIView = lastUiView;
			        _event.Trigger(this, _uiResumeArgs);
                }
			}
		}

        //获取ui界面
        private async Task<UIView> GetUiView(AssetConfig assetConfig)
        {
            UIView uiView;
            if (!_allUiViews.TryGetValue(assetConfig, out uiView))
            {
                GameObject uiViewSource = await _resource.LoadAsset<GameObject>(assetConfig.AssetBundleName, assetConfig.AssetPath);
                if (uiViewSource == null)
                    throw new Exception("uiview path not found:" + assetConfig.AssetBundleName + ":" + assetConfig.AssetPath);

                GameObject uiViewClone = GameObject.Instantiate(uiViewSource);
                BindToUIRoot(uiViewClone);
                uiView = uiViewClone.GetComponent<UIView>();
                if (uiView == null)
                    return null;
                _allUiViews[assetConfig] = uiView;
                return uiView;
            }
            uiView.gameObject.SetActive(true);
            return uiView;
        }

        #endregion



        #region 内部函数
        //检查路径
        private AssetConfig CheckAssetPath(Type t)
		{
		    int hashCode = t.GetHashCode();

		    AssetConfig assetCofig = null;
            if (!_uiAssetPath.TryGetValue(hashCode, out assetCofig))
		    {
		        object[] attrs = t.GetCustomAttributes(typeof(UIViewAttribute), false);
		        if (attrs.Length == 0)
		            return null;
		        UIViewAttribute uIViewAttribute = attrs[0] as UIViewAttribute;

                if (string.IsNullOrEmpty(uIViewAttribute?.ViewPath) || string.IsNullOrEmpty(uIViewAttribute.AssetBundleName))
		            return null;
		        assetCofig = new AssetConfig(uIViewAttribute.AssetBundleName, uIViewAttribute.ViewPath);

				_uiAssetPath[hashCode] = assetCofig;
			}
		    return assetCofig;
        }


		

        #endregion

        /// <summary>
		/// 显示UI
		/// </summary>
		public void ShowUI<T>() where T : UIView
        {
            AssetConfig assetConfig = CheckAssetPath(typeof(T));
            if (assetConfig == null)
                return;

            UIView uiView = null;
            if (_allUiViews.TryGetValue(assetConfig, out uiView))
            {
                _uiShowArgs.UIView = uiView;
                _event.Trigger(this, _uiShowArgs);

                uiView.Show();
          
            }
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="uiBehaviourName"></param>
        public void HideUI<T>() where T : UIView
        {
            AssetConfig assetConfig = CheckAssetPath(typeof(T));
            if (assetConfig == null)
                return;

            UIView uiView = null;
            if (_allUiViews.TryGetValue(assetConfig, out uiView))
            {
                _uiHideArgs.UIView = uiView;
                _event.Trigger(this, _uiHideArgs);
                uiView.OnHide();

                uiView.Hide();
               
            }
        }

        /// <summary>
		/// 关闭并卸载UI
		/// </summary>
		public void CloseUI<T>() where T : UIView
        {
            AssetConfig assetConfig = CheckAssetPath(typeof(T));
            if (assetConfig == null)
                return;

            UIView uiView = null;
            if (_allUiViews.TryGetValue(assetConfig, out uiView))
            {
                //触发关闭事件
                _uiCloseArgs.UIView = uiView;
                _event.Trigger(this, _uiCloseArgs);

                uiView.OnClose();
                _allUiViews.Remove(assetConfig);
                uiView.DestroySelf();
            }
        }

        /// <summary>
		/// 获取UI
		/// </summary>
		/// <returns></returns>
		public UIView GetUI<T>() where T : UIView
        {
            AssetConfig assetConfig = CheckAssetPath(typeof(T));
            if (assetConfig == null)
                return null;

            UIView uiView = null;
            if (_allUiViews.TryGetValue(assetConfig, out uiView))
            {
                return uiView as UIView;
            }

            return null;
        }



            /// <summary>
            /// 打开UI
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public async void OpenUI<T>(params object[] parameters) where T : UIView
        {
            AssetConfig assetConfig = CheckAssetPath(typeof(T));
            if (assetConfig == null)
                return;

            UIView newUiView = null;
            if (!_allUiViews.ContainsKey(assetConfig))
            {
                newUiView = await CreateUI(assetConfig);
            }else
            {
                newUiView = GetUI<T>();
            }

            if(newUiView!=null)
            {
                newUiView.Show();

                newUiView.OnShow(parameters);

                //触发打开事件
                _uiShowArgs.UIView = newUiView;
                _event.Trigger(this, _uiShowArgs);
            }

        }

        public async Task<UIView> CreateUI(AssetConfig assetConfig)
        {
            UIView uiView = null;

            if (_allUiViews.TryGetValue(assetConfig, out uiView))
            {
                return uiView;
            }

            GameObject uiViewSource = await _resource.LoadAsset<GameObject>(assetConfig.AssetBundleName, assetConfig.AssetPath);
            if (uiViewSource == null)
                throw new Exception("UIView path not found:" + assetConfig.AssetBundleName + ":" + assetConfig.AssetPath);

            GameObject uiViewClone = GameObject.Instantiate(uiViewSource);
            BindToUIRoot(uiViewClone);
            uiView = uiViewClone.GetComponent<UIView>();
            if (uiView == null) {
                throw new Exception("Can't get UIView Component!");
            }
                
            //_allUiViews[assetConfig] = uiView;
            _allUiViews.Add(assetConfig, uiView);
            uiView.OnInitUI();
            _uiInitArgs.UIView = uiView;
            _event.Trigger(this, _uiInitArgs);

            return uiView;
        }


        /// <summary>
        /// 将生成的UI挂载到指定节点
        /// </summary>
        /// <param name="UI层级"></param>
        private void BindToUIRoot(GameObject uiObject,UILevel uiLevel = UILevel.Common)
        {
            if (UIRoot == null)
            {
                throw new Exception("Please set UIRoot first");
            }
            else
            {
                uiObject.transform.SetParent(UIRoot.transform);
                uiObject.transform.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// 获取所有显示的UI
        /// </summary>
        /// <returns></returns>
        public Dictionary<AssetConfig, UIView> GetAllUIView()
        {
            return _allUiViews;
        }

        #region 重写函数
        public override void OnClose()
		{
			_stackUiAsset.Clear();
			_allUiAssets.Clear();

			foreach (var item in _allUiViews.Values)
			{
				MonoBehaviour.Destroy(item);
			}
			_allUiViews.Clear();
		}
        #endregion


        #region 数据结构
        //资源配置
        public class AssetConfig
        {
            public string AssetBundleName;
            public string AssetPath;

            public AssetConfig()
            {
            }

            public AssetConfig(string assetBundleName, string assetPath)
            {
                AssetBundleName = assetBundleName;
                AssetPath = assetPath;
            }
        }
        #endregion

    }
}
