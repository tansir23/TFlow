//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #网络响应事件# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace GameFramework.Sunny
{
    public class WebRequestEventArgs
    {

    }

    public class HttpResponseSuccessEventArgs : GameEventArgs<HttpResponseSuccessEventArgs>
    {
        /// <summary>
        /// 链接
        /// </summary>
        public string Url;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content;
    }

    public class HttpResponseFailEventArgs : GameEventArgs<HttpResponseFailEventArgs>
    {
        /// <summary>
        /// 链接
        /// </summary>
        public string Url;
        /// <summary>
        /// 错误内容
        /// </summary>
        public string Error;
    }

    public class DownloadSuccessEventArgs : GameEventArgs<DownloadSuccessEventArgs>
    {
        /// <summary>
        /// 远程的链接
        /// </summary>
        public string RemoteUrl;
        /// <summary>
        /// 本地文件的路径
        /// </summary>
        public string LocalPath;
    }

    public class DownloadFaileEventArgs : GameEventArgs<DownloadFaileEventArgs>
    {
        /// <summary>
        /// 远程的链接
        /// </summary>
        public string RemoteUrl;
        /// <summary>
        /// 本地文件的路径
        /// </summary>
        public string LocalPath;
        /// <summary>
        /// 错误
        /// </summary>
        public string Error;
    }

    public class DownloadProgressEventArgs : GameEventArgs<DownloadProgressEventArgs>
    {
        /// <summary>
        /// 远程的路径
        /// </summary>
        public string RemoteUrl;
        /// <summary>
        /// 本地的路径
        /// </summary>
        public string LocalPath;
        /// <summary>
        /// 下载的字节长度
        /// </summary>
        public ulong DownloadBytes;
        /// <summary>
        /// 下载的进度 0 ~ 1
        /// </summary>
        public float DownloadProgress;
        /// <summary>
        /// 下载消耗的时间 单位s
        /// </summary>
        public float DownloadSeconds;
        /// <summary>
        /// 下载速度 KB/s
        /// </summary>
        public float DownloadSpeed;
    }

}
