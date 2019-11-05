//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #UI模块编辑器# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GameFramework.Sunny
{
    public class UIModuleEditor : ModuleEditorBase
    {
        //private Gam UIRoot;

        public UIModuleEditor(string name, Color mainColor, GameMode gameMode)
    : base(name, mainColor, gameMode)
        {

        }

        public override void OnDrawGUI()
        {

            GUILayout.BeginVertical("HelpBox");
            //GUILayout.BeginHorizontal("HelpBox");
            _gameMode.UIRoot = EditorGUILayout.ObjectField("UI Root", _gameMode.UIRoot, typeof(GameObject), true) as GameObject;
            if (Application.isPlaying)
            {
                GUILayout.BeginVertical("HelpBox");
                DrawUIItemName();
                GUILayout.EndVertical();
            }else
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
            }
               
           
            //GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public override void OnClose()
        {
        }

        private void DrawUIItemName()
        {
            Dictionary<UIManager.AssetConfig, UIView> allUIView = GameMode.UI.GetAllUIView();
            if (allUIView != null && allUIView.Count > 0)
            {
                foreach (var item in allUIView)

                {
                    EditorGUILayout.LabelField(item.Value.ToString(), ((UIState)item.Value.state).ToString());
                    //Console.WriteLine(item.Key + item.Value);

                }
                
                //EditorGUILayout.LabelField()
            }
        }
    }
}