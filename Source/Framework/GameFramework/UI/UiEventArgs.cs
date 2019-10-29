//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #ui事件参数# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

namespace GameFramework.Taurus
{

    /// <summary>
	/// ui初始化事件
	/// </summary>
	public class UIInitEventArgs : GameEventArgs<UIInitEventArgs>
    {
        public IUIView UIView;

    }
    /// <summary>
    /// ui打开事件
    /// </summary>
    public class UIShowEventArgs : GameEventArgs<UIShowEventArgs>
	{
		public IUIView UIView;
	}

	/// <summary>
	/// ui关闭事件
	/// </summary>
	public class UICloseEventArgs : GameEventArgs<UICloseEventArgs>
	{
		public IUIView UIView;
	}

	/// <summary>
	/// ui暂停事件
	/// </summary>
	public class UIPauseEventArgs : GameEventArgs<UIPauseEventArgs>
	{
		public IUIView UIView;
	}


	/// <summary>
	/// ui恢复事件
	/// </summary>
	public class UIResumeEventArgs : GameEventArgs<UIResumeEventArgs>
	{
		public IUIView UIView;
	}

    /// <summary>
    /// ui隐藏事件
    /// </summary>
    public class UIHideEventArgs : GameEventArgs<UIHideEventArgs>
    {
        public IUIView UIView;
    }

}