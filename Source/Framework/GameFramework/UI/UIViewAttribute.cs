//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #UI类标记# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------


using System;

namespace GameFramework.Taurus
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UIViewAttribute : Attribute
    {
        public string AssetBundleName { get; private set; }
        public string ViewPath { get; private set; }

        public UIViewAttribute(string assetBundleName,string viewPath)
        {
            AssetBundleName = assetBundleName;
            ViewPath = viewPath;
        }
    }
}
