//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #场景管理# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using System.Threading.Tasks;

namespace GameFramework.Sunny
{
    public sealed class ScenarioManager : GameFrameworkModule, IUpdate
    {
        //事件触发类
        private EventManager _event;

        //资源管理器 帮助类
        private IResourceHelper _resourceHelper;

        //场景加载中事件
        private SceneLoadingEventArgs _sceneLoadingEventArgs;
        //场景加载完毕事件
        private SceneLoadedEventArgs _sceneLoadedEventArgs;
        //场景异步加载
        private Dictionary<string, AsyncOperation> _sceneAsyncOperations;

        // 已经加载的场景名
        private readonly List<string> m_LoadedSceneAssetNames;
        // 载入中的场景名
        private readonly List<string> m_LoadingSceneAssetNames;

        public ScenarioManager()
        {
            //获取事件管理器
            _event = GameFrameworkMode.GetModule<EventManager>();


            //场景事件
            m_LoadedSceneAssetNames = new List<string>();
            m_LoadingSceneAssetNames = new List<string>();
            _sceneLoadingEventArgs = new SceneLoadingEventArgs();
            _sceneLoadedEventArgs = new SceneLoadedEventArgs();
            _sceneAsyncOperations = new Dictionary<string, AsyncOperation>();
        }
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="sceneName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        
        public async Task<AsyncOperation> LoadSceneAsync(string assetBundleName, string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            _resourceHelper = GameMode.Resource.GetResourceHelper();

            if (_resourceHelper == null)
                throw new GamekException("You must set resource manager first.");

            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GamekException("Scene asset name is invalid.");
            }
   
            if (SceneIsLoading(sceneName))
            {
                throw new GamekException(string.Format("Scene asset '{0}' is being loaded.", sceneName));
            }

            if (SceneIsLoaded(sceneName))
            {
                throw new GamekException(string.Format("Scene asset '{0}' is already loaded.", sceneName));
            }

            m_LoadingSceneAssetNames.Add(sceneName);

            AsyncOperation asyncOperation = await _resourceHelper.LoadSceneAsync(assetBundleName, sceneName, mode);
            _sceneAsyncOperations.Add(sceneName, asyncOperation);
            return asyncOperation;
        }
        
        #region 重写函数

        public void OnUpdate()
        {
            if (_sceneAsyncOperations.Count > 0)
            {
                foreach (var item in _sceneAsyncOperations)
                {
                    //触发加载完毕事件
                    if (item.Value.isDone)
                    {
                        string sceneAssetName = item.Key;
                        m_LoadingSceneAssetNames.Remove(sceneAssetName);
                        m_LoadedSceneAssetNames.Clear();
                        m_LoadedSceneAssetNames.Add(sceneAssetName);

                        _sceneLoadedEventArgs.SceneName = item.Key;
                        _event.Trigger(this, _sceneLoadedEventArgs);
                        _sceneAsyncOperations.Remove(item.Key);
                        break;
                    }
                    //触发正在加载事件
                    else
                    {
                        _sceneLoadingEventArgs.SceneName = item.Key;
                        _sceneLoadingEventArgs.Progress = item.Value.progress;
                        _event.Trigger(this, _sceneLoadingEventArgs);
                    }
                }
            }
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void UnloadSceneAsync(string sceneName)
        {
            if (_resourceHelper == null)
                return;

            _resourceHelper.UnloadSceneAsync(sceneName);
            return;
        }


        /// <summary>
        /// 获取场景是否已加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否已加载。</returns>
        public bool SceneIsLoaded(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GamekException("Scene asset name is invalid.");
            }

            return m_LoadedSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            return m_LoadedSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <param name="results">已加载场景的资源名称。</param>
        public void GetLoadedSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GamekException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadedSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在加载。</returns>
        public bool SceneIsLoading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GamekException("Scene asset name is invalid.");
            }

            return m_LoadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return m_LoadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <param name="results">正在加载场景的资源名称。</param>
        public void GetLoadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new GamekException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadingSceneAssetNames);
        }

        /// <summary>
        /// 获取场景名称。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景名称。</returns>
        public static string GetSceneName(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new GamekException("Scene asset name is invalid.");
                //return null;
            }

            int sceneNamePosition = sceneAssetName.LastIndexOf('/');
            if (sceneNamePosition + 1 >= sceneAssetName.Length)
            {
                throw new GamekException(string.Format("Scene asset name '{0}' is invalid.", sceneAssetName));
                //return null;
            }

            string sceneName = sceneAssetName.Substring(sceneNamePosition + 1);
            sceneNamePosition = sceneName.LastIndexOf(".unity");
            if (sceneNamePosition > 0)
            {
                sceneName = sceneName.Substring(0, sceneNamePosition);
            }

            return sceneName;
        }

        public override void OnClose()
        {
            m_LoadedSceneAssetNames.Clear();
            m_LoadingSceneAssetNames.Clear();
        }
        #endregion
    }
}