//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #网页请求管理器# </describe>
// <time> #2019# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Sunny
{
    public sealed class WebRequestManager : GameFrameworkModule
    {
        #region 属性

        // 事件管理类
        private EventManager _event;

        //请求帮助类
        private IWebRequestHelper _webRequestHelper;
        //request失败的事件成功的事件
        private HttpResponseSuccessEventArgs _httpRequestSuccess;
        //request失败的事件
        private HttpResponseFailEventArgs _httpRequestFail;

        //http下载帮助类(暂时没管)
        private IWebDownloadHelper _webDownloadHelper;
        private DownloadSuccessEventArgs _downloadSuccess;
        private DownloadFaileEventArgs _downloadFaile;
        private DownloadProgressEventArgs _downloadProgress;
        #endregion

        public WebRequestManager()
        {
            _event = GameFrameworkMode.GetModule<EventManager>();
            _httpRequestSuccess = new HttpResponseSuccessEventArgs();
            _httpRequestFail = new HttpResponseFailEventArgs();
            _downloadSuccess = new DownloadSuccessEventArgs();
            _downloadFaile = new DownloadFaileEventArgs();
            _downloadProgress = new DownloadProgressEventArgs();
        }

        #region 外部接口

        /// <summary>
        /// 设置网页请求帮助类
        /// </summary>
        /// <param name="helper"></param>
        public void SetWebRequestHelper(IWebRequestHelper helper)
        {
            _webRequestHelper = helper;
        }
        /// <summary>
        /// 设置下载的帮助类
        /// </summary>
        /// <param name="helper"></param>
        public void SetWebDownloadHelper(IWebDownloadHelper helper)
        {
            _webDownloadHelper = helper;
        }

        #region 网络请求方法
        public IHttpRequest Get(string uri)
        {
            return _webRequestHelper?.Get(uri).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest GetTexture(string uri)
        {
            return _webRequestHelper?.GetTexture(uri).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Post(string uri, string postData)
        {
            return _webRequestHelper?.Post(uri, postData).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Post(string uri, WWWForm formData)
        {
            return _webRequestHelper?.Post(uri, formData).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Post(string uri, Dictionary<string, string> formData)
        {
            return _webRequestHelper?.Post(uri, formData).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Post(string uri, List<IMultipartFormSection> multipartForm)
        {
            return _webRequestHelper?.Post(uri, multipartForm).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Post(string uri, byte[] bytes, string contentType)
        {
            return _webRequestHelper?.Post(uri, bytes, contentType).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest PostJson(string uri, string json)
        {
            return _webRequestHelper?.PostJson(uri, json).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest PostJson<T>(string uri, T payload) where T : class
        {
            return _webRequestHelper?.PostJson(uri, payload).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Put(string uri, byte[] bodyData)
        {
            return _webRequestHelper?.Put(uri, bodyData).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Put(string uri, string bodyData)
        {
            return _webRequestHelper?.Put(uri, bodyData).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Delete(string uri)
        {
            return _webRequestHelper?.Delete(uri).SetEventCallback(OnEventCallback);
        }

        public IHttpRequest Head(string uri)
        {
            return _webRequestHelper?.Head(uri).SetEventCallback(OnEventCallback);
        }
        #endregion

        /// <summary>
        /// 开始下载文件
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localPath"></param>
        public void StartDownload(string remoteUrl, string localPath)
        {
            _webDownloadHelper?.StartDownload(remoteUrl, localPath, StartDownloadCallback,StartDownloadProgress);
        }

        #endregion

        public override void OnClose()
        {

        }

        #region 内部函数
        /// <summary>
        /// Http请求回调
        /// </summary>
        /// <param name="url"></param>
        /// <param name="result"></param>
        /// <param name="content"></param>
        private void OnEventCallback(string url, bool result, string content)
        {
            if (result)
            {
                _httpRequestSuccess.Url = url;
                _httpRequestSuccess.Content = content;
                _event.Trigger(this, _httpRequestSuccess);
            }
            else
            {
                _httpRequestFail.Url = url;
                _httpRequestFail.Error = content;
                _event.Trigger(this, _httpRequestFail);
            }
        }

        /// <summary>
        /// 开始下载的回调
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localPath"></param>
        /// <param name="result"></param>
        /// <param name="content"></param>
        private void StartDownloadCallback(string remoteUrl, string localPath, bool result, string content)
        {
            if (result)
            {
                _downloadSuccess.RemoteUrl = remoteUrl;
                _downloadSuccess.LocalPath = localPath;
                _event.Trigger(this, _downloadSuccess);
            }
            else
            {
                _downloadFaile.RemoteUrl = remoteUrl;
                _downloadFaile.LocalPath = localPath;
                _downloadFaile.Error = content;
                _event.Trigger(this, _downloadFaile);
            }
        }

        /// <summary>
        /// 下载的进度
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localPath"></param>
        /// <param name="dataLength"></param>
        /// <param name="progess"></param>
        /// <param name="seconds"></param>
        private void StartDownloadProgress(string remoteUrl, string localPath, ulong dataLength, float progess,float seconds)
        {
            _downloadProgress.RemoteUrl = remoteUrl;
            _downloadProgress.LocalPath = localPath;
            _downloadProgress.DownloadBytes = dataLength;
            _downloadProgress.DownloadProgress = progess;
            _downloadProgress.DownloadSeconds = seconds;
            _downloadProgress.DownloadSpeed =
                dataLength == 0.0f ? dataLength : dataLength / 1024.0f  / seconds;
            _event.Trigger(this, _downloadProgress);
        }
        #endregion

    }
}
