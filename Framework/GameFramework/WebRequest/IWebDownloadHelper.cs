//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #下载帮助接口# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.Sunny
{
    public interface IWebDownloadHelper
    {
        void StartDownload(string remoteUrl, string localPath,Action<string,string,bool,string> result,Action<string,string,ulong,float,float> progress);
    }
}
