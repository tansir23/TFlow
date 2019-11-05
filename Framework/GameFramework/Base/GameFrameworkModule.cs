//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #游戏模块的基类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

namespace GameFramework.Sunny
{
    public abstract class GameFrameworkModule
    {
        /// <summary>
        /// 关闭当前模块
        /// </summary>
        public abstract void OnClose();

    }
}
