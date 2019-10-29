//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #网页请求 继承MonoBehaviour的实现类# </describe>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
    public sealed class WebRquestMonoHelper : MonoBehaviour,IWebRequestHelper
    {
        private Dictionary<string, string> superHeaders;
        private Dictionary<IHttpRequest, Coroutine> httpRequests;

        public void Start()
        {
            superHeaders = new Dictionary<string, string>();
            httpRequests = new Dictionary<IHttpRequest, Coroutine>();
        }

        #region 
        /// <summary>
        /// Super headers are key value pairs that will be added to every subsequent HttpRequest.
        /// </summary>
        /// <returns>A dictionary of super-headers.</returns>
        public Dictionary<string, string> GetSuperHeaders()
        {
            return new Dictionary<string, string>(superHeaders);
        }

        /// <summary>
        /// Sets a header to the SuperHeaders key value pair, if the header key already exists, the value will be replaced.
        /// </summary>
        /// <param name="key">The header key to be set.</param>
        /// <param name="value">The header value to be assigned.</param>
        public void SetSuperHeader(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value cannot be null or empty, if you are intending to remove the value, use the RemoveSuperHeader() method.");
            }

            superHeaders[key] = value;
        }

        /// <returns>If the removal of the element was successful</returns>
        public bool RemoveSuperHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty.");
            }

            return superHeaders.Remove(key);
        }
        #endregion

        internal void SendRequest(IHttpRequest request, Action<HttpResponse> onSuccess = null,
           Action<HttpResponse> onError = null, Action<HttpResponse> onNetworkError = null, Action<string, bool, string> result = null)
        {
            var enumerator = SendCoroutine(request, onSuccess, onError, onNetworkError, result);
            var coroutine = StartCoroutine(enumerator);
            httpRequests.Add(request, coroutine);
        }

        private IEnumerator SendCoroutine(IHttpRequest request, Action<HttpResponse> onSuccess = null,
            Action<HttpResponse> onError = null, Action<HttpResponse> onNetworkError = null, Action<string, bool, string> result = null)
        {
            yield return Send(request, onSuccess, onError, onNetworkError, result);
            httpRequests.Remove(request);
        }


        internal void AbortRequest(IHttpRequest request)
        {
            Abort(request);

            if (httpRequests.ContainsKey(request))
            {
                StopCoroutine(httpRequests[request]);
            }

            httpRequests.Remove(request);
        }

        private void Update()
        {
            foreach (var httpRequest in httpRequests.Keys)
            {
                (httpRequest as IUpdateProgress)?.UpdateProgress();
            }
        }

        public IHttpRequest Get(string uri)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Get(uri));
        }

        public IHttpRequest GetTexture(string uri)
        {
            return new UnityHttpRequest(this,UnityWebRequestTexture.GetTexture(uri));
        }

        public IHttpRequest Post(string uri, string postData)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Post(uri, postData));
        }

        public IHttpRequest Post(string uri, WWWForm formData)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Post(uri, formData));
        }

        public IHttpRequest Post(string uri, Dictionary<string, string> formData)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Post(uri, formData));
        }

        public IHttpRequest Post(string uri, List<IMultipartFormSection> multipartForm)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Post(uri, multipartForm));
        }

        public IHttpRequest Post(string uri, byte[] bytes, string contentType)
        {
            var unityWebRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(bytes)
                {
                    contentType = contentType
                },
                downloadHandler = new DownloadHandlerBuffer()
            };
            return new UnityHttpRequest(this,unityWebRequest);
        }

        public IHttpRequest PostJson(string uri, string json)
        {
            return Post(uri, System.Text.Encoding.UTF8.GetBytes(json), "application/json");
        }

        public IHttpRequest PostJson<T>(string uri, T payload) where T : class
        {
            return PostJson(uri, JsonUtility.ToJson(payload));
        }

        public IHttpRequest Put(string uri, byte[] bodyData)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Put(uri, bodyData));
        }

        public IHttpRequest Put(string uri, string bodyData)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Put(uri, bodyData));
        }

        public IHttpRequest Delete(string uri)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Delete(uri));
        }

        public IHttpRequest Head(string uri)
        {
            return new UnityHttpRequest(this,UnityWebRequest.Head(uri));
        }

        /*
        public void RequestHttpGet(string url, Action<string, bool,string> result)
        {
            StartCoroutine(ReadHttpTextGet(url, result));
        }

        public void RequestHttpPost(string url, WWWForm form, Action<string, bool, string> result)
        {
            StartCoroutine(ReadHttpTextPost(url, form, result));
        }*/

        public IEnumerator Send(IHttpRequest request, Action<HttpResponse> onSuccess = null,
            Action<HttpResponse> onError = null, Action<HttpResponse> onNetworkError = null, Action<string, bool, string> result = null)
        {
            var unityHttpRequest = (UnityHttpRequest)request;
            var unityWebRequest = unityHttpRequest.UnityWebRequest;

            yield return unityWebRequest.SendWebRequest();

            var response = CreateResponse(unityWebRequest);

            if (unityWebRequest.isNetworkError)
            {
                onNetworkError?.Invoke(response);
            }
            else if (unityWebRequest.isHttpError)
            {
                onError?.Invoke(response);
            }
            else
            {
                onSuccess?.Invoke(response);
            }

            if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
                result?.Invoke(response.Url, false, response.Error);
            else
            {
                result?.Invoke(response.Url, true, response.Text);
            }
        }


        public void Abort(IHttpRequest request)
        {
            var unityHttpRequest = request as UnityHttpRequest;
            if (unityHttpRequest?.UnityWebRequest != null && !unityHttpRequest.UnityWebRequest.isDone)
            {
                unityHttpRequest.UnityWebRequest.Abort();
            }
        }

        private static HttpResponse CreateResponse(UnityWebRequest unityWebRequest)
        {
            return new HttpResponse
            {
                Url = unityWebRequest.url,
                Bytes = unityWebRequest.downloadHandler.data,
                Text = unityWebRequest.downloadHandler.text,
                IsSuccessful = !unityWebRequest.isHttpError && !unityWebRequest.isNetworkError,
                IsHttpError = unityWebRequest.isHttpError,
                IsNetworkError = unityWebRequest.isNetworkError,
                Error = unityWebRequest.error,
                StatusCode = unityWebRequest.responseCode,
                ResponseHeaders = unityWebRequest.GetResponseHeaders(),
                Texture = (unityWebRequest.downloadHandler as DownloadHandlerTexture)?.texture
            };
        }

        /*
        IEnumerator ReadHttpTextGet(string url, Action<string, bool,string> result)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);

            yield return webRequest.SendWebRequest();
            if (webRequest.isHttpError || webRequest.isNetworkError)
                result.Invoke(url, false, webRequest.error.ToString());
            else
            {
                result.Invoke(url, true, webRequest.downloadHandler.text);
            }
        }

        IEnumerator ReadHttpTextPost(string url, WWWForm form, Action<string, bool, string> result)
        {

            UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

            yield return webRequest.SendWebRequest();
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                result.Invoke(url, false, webRequest.error.ToString());
                Debug.Log(webRequest.error);
            }
            else
            {
                result.Invoke(url, true, webRequest.downloadHandler.text);
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
        */

    }
}
