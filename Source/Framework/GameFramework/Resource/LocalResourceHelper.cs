//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #本地资源加载(Resource目录)# </describe>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using System.Threading.Tasks;

namespace GameFramework.Taurus
{
    public class LocalResourceHelper : IResourceHelper
    {
        public void SetResourcePath(PathType pathType, string rootAssetBundle = "AssetBundles/AssetBundles", bool isEncrypt=false)
        {
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public Task<T> LoadAsset<T>(string assetBundleName,string assetName) where T : Object
        {
			TaskCompletionSource<T> task = new TaskCompletionSource<T>();
			task.SetResult(Resources.Load<T>(assetName));
			return task.Task;
        }

		
		/// <summary>
		/// 卸载资源 主要为卸载AssetBundle
		/// </summary>
		/// <param name="assetName">资源名称</param>
		/// <param name="allAssets">是否卸载调所有资源</param>
		public void UnloadAsset(string assetBundleName, bool unload)
	    {
	    }

	    /// <summary>
		/// 异步加载场景
		/// </summary>
		/// <param name="sceneName"></param>
		public Task<AsyncOperation> LoadSceneAsync(string assetBundleName,string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
			TaskCompletionSource<AsyncOperation> task = new TaskCompletionSource<AsyncOperation>();
			task.SetResult(UnitySceneManager.LoadSceneAsync(sceneName, mode));
            //Debug.Log("szfsdfsd");
			return task.Task;
			//  return UnitySceneManager.LoadSceneAsync(sceneName, mode);
		}

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public AsyncOperation UnloadSceneAsync(string sceneName)
        {
            return UnitySceneManager.UnloadSceneAsync(sceneName);
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Clear()
        {
        }

		public Task<AssetBundle> LoadAssetBundle(string assetBundleName)
		{
			return null;
		}

		public T LoadAssetSync<T>(string assetBundleName, string assetName) where T : Object
		{
            /*Type t = typeof(T);
            if (t.Name.Equals("TextAsset"))
            {
                return Resources.Load(assetName) as T;
            }*/
            return Resources.Load<T>(assetName);

        }
	}
}
