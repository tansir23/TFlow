﻿using GameFramework.Taurus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Example.UI
{
	[UIView ("ui", GlobalManager.UIPrefabRootPath + "UITestView")]
	public class UITestView : UIView
	{
		public Button btnTest;
		public Button btnTest2;

		public override void OnInitUI ()
		{
			Debug.Log (gameObject + " OnInitUI");
		}

		/// <summary>
		/// 打开界面
		/// </summary>
		/// <param name="parameters">不确定参数</param>
		public override void OnShow (params object[] parameters)
		{

			Debug.Log (gameObject + " OnShow");

			btnTest.onClick.AddListener (() => {
				GameMode.UI.CloseUI<UITestView>();
				GameMode.UI.OpenUI<UITestMenuView>();
			});

			btnTest2.onClick.AddListener (() => {
				GameMode.UI.CloseUI<UITestView>();
				GameMode.UI.OpenUI<UITest2View> ();

			});
		}

		/// <summary>
		/// 退出界面
		/// </summary>
		public override void OnClose ()
		{
			Debug.Log (gameObject + " OnClose");
		}

		public override void OnHide ()
		{
			Debug.Log (gameObject + " OnHide");
		}

		/// <summary>
		/// 暂停界面
		/// </summary>
		public override void OnPause ()
		{
			Debug.Log (gameObject + " OnPause");
		}

		/// <summary>
		/// 恢复界面
		/// </summary>
		public override void OnResume ()
		{
			Debug.Log (gameObject + " OnResume");
		}
	}

}
