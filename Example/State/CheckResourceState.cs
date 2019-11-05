﻿//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #检查资源状态# </describe>
//-----------------------------------------------------------------------


using GameFramework.Example.UI;
using GameFramework.Sunny;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameFramework.Example.State
{
    [GameState]
    public class CheckResourceState : GameState
    {
        #region 属性
        //平台的资源名称
        private string _assetPlatformVersionText = "AssetPlatformVersion.txt";

        //资源信息文本名称
        private string _assetVersionTxt = "AssetVersion.txt";

        //本地版本信息
        private AssetBundleVersionInfo _localVersion;

        //远程版本信息
        private AssetBundleVersionInfo _remoteVersion;

        //资源更新完成
        private bool _resourceUpdateDone;
        //需要更新的资源
        private Dictionary<string, string> _downloadResouces;
		//余下的资源
		private List<string> _remainingResources;
		#endregion

		#region 重写函数

		public override void OnInit()
        {
            base.OnInit();

            _downloadResouces = new Dictionary<string, string>();
			_remainingResources = new List<string>();

        }

        public override void OnEnter(params object[] parameters)
        {
            base.OnEnter(parameters);
            GameMode.Event.AddListener<HttpResponseSuccessEventArgs>(OnHttpReadTextSuccess);
            GameMode.Event.AddListener<HttpResponseFailEventArgs>(OnHttpReadTextFail);
            GameMode.Event.AddListener<DownloadSuccessEventArgs>(OnDownloadSuccess);
            GameMode.Event.AddListener<DownloadFaileEventArgs>(OnDownloadFaile);
            GameMode.Event.AddListener<DownloadProgressEventArgs>(OnDownloadProgress);

            _localVersion = LoadLocalVersion();
            LoadRemoteVersion();
        }


        public override void OnExit()
        {
            GameMode.Event.RemoveListener<HttpResponseSuccessEventArgs>(OnHttpReadTextSuccess);
            GameMode.Event.RemoveListener<HttpResponseFailEventArgs>(OnHttpReadTextFail);
            GameMode.Event.RemoveListener<DownloadSuccessEventArgs>(OnDownloadSuccess);
            GameMode.Event.RemoveListener<DownloadFaileEventArgs>(OnDownloadFaile);
            GameMode.Event.RemoveListener<DownloadProgressEventArgs>(OnDownloadProgress);

            base.OnExit();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            //更新资源
            if (_resourceUpdateDone && _remainingResources.Count == 0)
            {
                //更新本地资源信息文本
                UpdateAssetVersionTxt();
                //  切换到加载界面
                ChangeState<LoadResourceState>();

                Debug.Log("Resource download complete");
            }
        }

        #endregion


        #region 事件回调
        //http文本读取成功
        private void OnHttpReadTextSuccess(object sender, IEventArgs e)
        {

            HttpResponseSuccessEventArgs ne = (HttpResponseSuccessEventArgs)e;
            if (ne != null)
            {
                if (ne.Url == Path.Combine(GameMode.Resource.ResUpdatePath, _assetPlatformVersionText))
                {
                    PlatformVersionInfo assetPlatform= JsonUtility.FromJson<PlatformVersionInfo>(ne.Content);
                    string platformName = GetPlatformName();
                    if (assetPlatform.Platforms.Contains(platformName))
                    {
                        //更新远程资源的路径
                        GameMode.Resource.ResUpdatePath =
                            Path.Combine(GameMode.Resource.ResUpdatePath, platformName);

                        //读取远程的文本
                        string remotePath = Path.Combine(GameMode.Resource.ResUpdatePath, _assetVersionTxt);
                        GameMode.WebRequest.Get(remotePath).Send();
                    }
                }
                else
                {
                    Debug.Log("CheckResourceState:RemoteVersion:"+ ne.Content);
                    _remoteVersion = JsonUtility.FromJson<AssetBundleVersionInfo>(ne.Content);
                    if (_remoteVersion == null)
                    {
                        Debug.LogError("Remote Version is null");
                        return;
                    }

                    //如果资源版本不一样 则更新资源
                    if (!CompareVersion())
                    {
                        //更新资源
                        UpdateResource();
                        //下载资源
                        DownloadResource();
                    }
                    else {
                        Debug.Log("No New Update!");
                    }

                    //资源更新完成
                    _resourceUpdateDone = true;
                }
            }
        }
        //http文件读取错误
        private void OnHttpReadTextFail(object sender, IEventArgs e)
        {
            HttpResponseFailEventArgs ne = (HttpResponseFailEventArgs) e;
            if (ne != null)
                Debug.LogError(ne.Error);
        }
        //加载文件成功
        private void OnDownloadSuccess(object sender, IEventArgs e)
        {
            DownloadSuccessEventArgs ne = (DownloadSuccessEventArgs) e;
			if (_remainingResources.Contains(ne.RemoteUrl))
				_remainingResources.Remove(ne.RemoteUrl);
            //if (_downloadResouces.ContainsKey(ne.RemoteUrl))
            //    _downloadResouces.Remove(ne.RemoteUrl);
		}
        //下载文件失败
        private void OnDownloadFaile(object sender, IEventArgs e)
        {
            DownloadFaileEventArgs ne = (DownloadFaileEventArgs)e;
            if (ne != null)
                Debug.LogError(ne.Error);
        }
        //下载进度
        private void OnDownloadProgress(object sender, IEventArgs e)
        {
            DownloadProgressEventArgs ne = (DownloadProgressEventArgs) e;
            Debug.Log(
                $"path:{ne.LocalPath} progress:{ne.DownloadProgress} bytes:{ne.DownloadBytes} speed:{ne.DownloadSpeed}");
        }
        #endregion

        #region 内部函数
        //加载本地版本信息
        private AssetBundleVersionInfo LoadLocalVersion()
        {
            string localPath =Path.Combine(GameMode.Resource.LocalPath, _assetVersionTxt);
            if(!File.Exists(localPath))
                return null;
            
            string content= File.ReadAllText(localPath);
            return JsonUtility.FromJson<AssetBundleVersionInfo>(content);
        }

        //加载远程版本信息
        private void LoadRemoteVersion()
        {
            string remotePath = Path.Combine(GameMode.Resource.ResUpdatePath, _assetPlatformVersionText);
            GameMode.WebRequest.Get(remotePath).Send();
        }

        //比较版本
        private bool CompareVersion()
        {
            bool noNewVersion = _localVersion != null && _remoteVersion.Version == _localVersion.Version;
            //Debug.Log("CheckResourceState:UpdateResource:noNewVersion:"+ noNewVersion);
            return noNewVersion;
        }

        //更新资源
        private void UpdateResource()
        {
            Debug.Log("CheckResourceState:UpdateResource");

            if (_localVersion == null)
            {
                Debug.LogError("UpdateResource:_localVersion == null");
                return;
            }

            if (_localVersion.AssetHashInfos == null)
            {
                Debug.LogError("UpdateResource:_localVersion.AssetHashInfos == null");
                return;
            }

            foreach (var item in _remoteVersion.AssetHashInfos)
            {

                //本地是否包含此文件(对比名字和Hash码)
                if (_localVersion.AssetHashInfos.Contains(item))
                {
                    continue;
                }
                else
                {
                    Debug.Log("UpdateResource:Res_"+ item.Name +" has new version!");
                }
                    
                string remoteUrl = Path.Combine(GameMode.Resource.ResUpdatePath, item.Name);
                //获取本地文件的路径
                string localPath = Path.Combine(GameMode.Resource.LocalPath, item.Name);
	           
				//创建文件夹
                string localDir = Path.GetDirectoryName(localPath);
                if (!Directory.Exists(localDir))
                    Directory.CreateDirectory(localDir);

                //添加需要下载的资源
                _downloadResouces.Add(remoteUrl, localPath);
		
				_remainingResources.Add(remoteUrl);
			}
          
        }

        //下载资源
        private void DownloadResource()
        {
            Debug.Log("CheckResourceState:DownloadResource");
            foreach (var item in _downloadResouces)
            {
                GameMode.WebRequest.StartDownload(item.Key, item.Value);
            }
        }

        //更新资源版本信息文本
        private void UpdateAssetVersionTxt()
        {
            if (!CompareVersion())
            {
                string localPath = Path.Combine(GameMode.Resource.LocalPath, _assetVersionTxt);
                File.WriteAllText(localPath, JsonUtility.ToJson(_remoteVersion));
                Debug.Log("UpdateAssetVersionTxt");
            }
        }

        //获取平台名称
        private string GetPlatformName()
        {
            string platformName = "StandaloneWindows";
            #if UNITY_IOS
                platformName = "IOS";
            #elif UNITY_ANDROID
                platformName = "Android";
            #elif UNITY_STANDALONE_OSX 
                platformName = "StandaloneOSX";
            #endif
            return platformName;
        }
#endregion

    }
}
