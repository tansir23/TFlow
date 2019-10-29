//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #实现网络请求# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
	public class UnityHttpRequest : IHttpRequest, IUpdateProgress
	{
		internal UnityWebRequest UnityWebRequest => unityWebRequest;
       

		private readonly UnityWebRequest unityWebRequest;
        private readonly WebRquestMonoHelper monoHelper;
        private readonly Dictionary<string, string> headers;

		private event Action<float> onUploadProgress;
		private event Action<float> onDownloadProgress;
		private event Action<HttpResponse> onSuccess;
		private event Action<HttpResponse> onError;
		private event Action<HttpResponse> onNetworkError;
        private event Action<string, bool, string> onResult;

		private float downloadProgress;
		private float uploadProgress;

		public UnityHttpRequest(WebRquestMonoHelper monoHelper,UnityWebRequest unityWebRequest)
		{
			this.unityWebRequest = unityWebRequest;
            this.monoHelper = monoHelper;

            headers = new Dictionary<string, string>(monoHelper.GetSuperHeaders());
		}

        public IHttpRequest SetEventCallback(Action<string, bool, string> onResult)
        {
            this.onResult += onResult;
            return this;
        }

        public IHttpRequest RemoveSuperHeaders()
		{
			foreach (var kvp in monoHelper.GetSuperHeaders())
			{
				headers.Remove(kvp.Key);
			}

			return this;
		}

		public IHttpRequest SetHeader(string key, string value)
		{
			headers[key] = value;
			return this;
		}

		public IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers)
		{
			foreach (var kvp in headers)
			{
				SetHeader(kvp.Key, kvp.Value);
			}

			return this;
		}

		public IHttpRequest OnUploadProgress(Action<float> onProgress)
		{
			onUploadProgress += onProgress;
			return this;
		}

		public IHttpRequest OnDownloadProgress(Action<float> onProgress)
		{
			onDownloadProgress += onProgress;
			return this;
		}

		public IHttpRequest OnSuccess(Action<HttpResponse> onSuccess)
		{
			this.onSuccess += onSuccess;
			return this;
		}

		public IHttpRequest OnError(Action<HttpResponse> onError)
		{
			this.onError += onError;
			return this;
		}

		public IHttpRequest OnNetworkError(Action<HttpResponse> onNetworkError)
		{
			this.onNetworkError += onNetworkError;
			return this;
		}

		public bool RemoveHeader(string key)
		{
			return headers.Remove(key);
		}

		public IHttpRequest SetTimeout(int duration)
		{
			unityWebRequest.timeout = duration;
			return this;
		}

        public IHttpRequest Send()
		{
			foreach (var header in headers)
			{
				unityWebRequest.SetRequestHeader(header.Key, header.Value);
			}

			monoHelper.SendRequest(this, onSuccess, onError, onNetworkError,onResult);
            return this;
		}


        public IHttpRequest SetRedirectLimit(int redirectLimit)
		{
			UnityWebRequest.redirectLimit = redirectLimit;
			return this;
		}

		public void UpdateProgress()
		{
			UpdateProgress(ref downloadProgress, unityWebRequest.downloadProgress, onDownloadProgress);
			UpdateProgress(ref uploadProgress, unityWebRequest.uploadProgress, onUploadProgress);
		}

		public void Abort()
		{
            monoHelper.Abort(this);
		}

		private void UpdateProgress(ref float currentProgress, float progress, Action<float> onProgress)
		{
			if (currentProgress < progress)
			{
				currentProgress = progress;
                onProgress?.Invoke(downloadProgress);
            }
		}

        
    }
}
