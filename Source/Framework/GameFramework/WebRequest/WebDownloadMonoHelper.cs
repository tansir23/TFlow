//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #下载帮助实现# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{

	public sealed class WebDownloadMonoHelper : MonoBehaviour,IWebDownloadHelper
    {
        public void StartDownload(string remoteUrl, string localPath, Action<string, string, bool, string> result, Action<string, string, ulong, float,float> progress)
        {
            StartCoroutine(UnityWebStartDownload(remoteUrl,localPath,result,progress));
        }
		
        IEnumerator UnityWebStartDownload(string remoteUrl, string localPath, Action<string, string, bool, string> result, Action<string, string, ulong, float,float> progress)
        {
            //断点续传写不写呢...
            //纠结------------------

            UnityWebRequest request = UnityWebRequest.Get(remoteUrl);
            request.downloadHandler = new DownloadHandlerFile(localPath);
            //yield return request.SendWebRequest(); 
            request.SendWebRequest();

            long lastTicks = DateTime.Now.Ticks;

            while (!request.isDone)
            {
                float seconds = (DateTime.Now.Ticks - lastTicks)/ 10000000.0f;
                progress.Invoke(remoteUrl, localPath, request.downloadedBytes, request.downloadProgress, seconds);
                yield return null;
            }

            if (request.isNetworkError || request.isHttpError)
                result.Invoke(remoteUrl, localPath, false,
                    "NetworkError:" + request.isNetworkError + "  HttpError:" + request.isHttpError);
            else
                result.Invoke(remoteUrl, localPath, true,
                    "File successfully downloaded and saved to " + localPath);
        }

    }
}
