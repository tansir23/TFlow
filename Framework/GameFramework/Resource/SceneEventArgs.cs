///-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #场景加载事件# </describe>
//-----------------------------------------------------------------------

namespace GameFramework.Sunny
{
    /// <summary>
    /// 场景加载中事件
    /// </summary>
    public class SceneLoadingEventArgs : GameEventArgs<SceneLoadingEventArgs>
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName;
        /// <summary>
        /// 场景加载进度
        /// </summary>
        public float Progress;
    }

    /// <summary>
    /// 场景加载完成事件
    /// </summary>
    public class SceneLoadedEventArgs : GameEventArgs<SceneLoadedEventArgs>
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName;
    }

}
