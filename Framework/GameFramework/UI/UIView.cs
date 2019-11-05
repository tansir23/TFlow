//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #view的实现虚类 继承Monobehaviour# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Sunny
{

    /// <summary>
    /// UI状态
    /// </summary>
    public enum UIState {
        None,
        Show,
        Hide
    }

    public abstract class UIView : MonoBehaviour,IUIView
    {
        #region 属性
        /// <summary>
        /// 状态
        /// </summary>
		[HideInInspector]
		public UIState state = UIState.None;
        #endregion

        #region 接口
        /// <summary>
        /// 初始化界面
        /// </summary>
        public abstract void OnInitUI();
        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="parameters">不确定参数</param>
        public abstract void OnShow(params object[] parameters);
        /// <summary>
        /// 退出界面
        /// </summary>
        public abstract void OnClose();
        /// <summary>
        /// 暂停界面
        /// </summary>
        public abstract void OnPause();
        /// <summary>
        /// 恢复界面
        /// </summary>
        public abstract void OnResume();
        /// <summary>
        /// 隐藏界面
        /// </summary>
        public abstract void OnHide();
        #endregion

        #region 基本方法
        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show() {
            state = UIState.Show;
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide(){
            state = UIState.Hide;
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void DestroySelf()
        {
            Destroy(gameObject);
        }
        #endregion


    }
}
