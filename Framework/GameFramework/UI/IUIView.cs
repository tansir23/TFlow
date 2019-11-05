//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #ui显示的接口类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------


namespace GameFramework.Sunny
{
    public interface IUIView
    {
        /// <summary>
        /// 初始化界面
        /// </summary>
        void OnInitUI();
        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="parameters">不确定参数</param>
        void OnShow(params object[] parameters);
        /// <summary>
        /// 退出界面
        /// </summary>
        void OnClose();
        /// <summary>
        /// 暂停界面
        /// </summary>
        void OnPause();
        /// <summary>
        /// 恢复界面
        /// </summary>
        void OnResume();
        /// <summary>
        /// 隐藏界面
        /// </summary>
        void OnHide();

    }
}
