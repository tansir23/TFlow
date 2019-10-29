//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #本地化的事件# </describe>
//-----------------------------------------------------------------------

using System;

namespace GameFramework.Taurus.Lang
{
    public class LanguageEventArgs : EventArgs
    {
        public Language Lang { get; private set; }

        public LanguageEventArgs(Language lang)
        {
            this.Lang = lang;
        }
    }
}