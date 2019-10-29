﻿//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #网络请求接口# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
	public interface IHttpRequest
	{
		IHttpRequest RemoveSuperHeaders();
		IHttpRequest SetHeader(string key, string value);
		IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers);
		IHttpRequest OnUploadProgress(Action<float> onProgress);
		IHttpRequest OnDownloadProgress(Action<float> onProgress);
		IHttpRequest OnSuccess(Action<HttpResponse> onSuccess);
		IHttpRequest OnError(Action<HttpResponse> onError);
		IHttpRequest OnNetworkError(Action<HttpResponse> onNetworkError);
		bool RemoveHeader(string key);
		IHttpRequest SetTimeout(int duration);
		IHttpRequest Send();
        IHttpRequest SetEventCallback(Action<string,bool,string> onResult);
        IHttpRequest SetRedirectLimit(int redirectLimit);
        void Abort();
	}
}
