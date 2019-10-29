using GameFramework.Taurus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Example.UI
{
    [UIView("ui", GlobalManager.UIPrefabRootPath + "UILoadingView.prefab")]
    public class UILoadingView : UIView
    {
        public Slider slider;
        public Text text;

        public override void OnInitUI()
        {
            Debug.Log(gameObject + " OnInitUI");
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="parameters">不确定参数</param>
        public override void OnShow(params object[] parameters)
        {

            Debug.Log(gameObject + " OnShow");
        }

        /// <summary>
        /// 退出界面
        /// </summary>
        public override void OnClose()
        {
            Debug.Log(gameObject + " OnClose");
        }

        public override void OnHide()
        {
            Debug.Log(gameObject + " OnHide");
        }

        /// <summary>
        /// 暂停界面
        /// </summary>
        public override void OnPause()
        {
            Debug.Log(gameObject + " OnPause");
        }

        /// <summary>
        /// 恢复界面
        /// </summary>
        public override void OnResume()
        {
            Debug.Log(gameObject + " OnResume");
        }

        public void SetLoading(float value) {
            slider.value = value;
            text.text = value * 100 + "%";
        }
    }
}
