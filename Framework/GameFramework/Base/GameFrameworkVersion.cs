//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #版本号管理# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

namespace GameFramework.Sunny
{
    public class GameModeVersion
    {

        private const string GameFrameworkVersionString = "1.0.0";

        /// <summary>
        /// 获取游戏框架版本号。
        /// </summary>
        public static string GameFrameworkVersion
        {
            get
            {
                return GameFrameworkVersionString;
            }
        }

    }
}
