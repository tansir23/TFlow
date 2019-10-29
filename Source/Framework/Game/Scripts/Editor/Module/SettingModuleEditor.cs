//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #设置模块编辑器# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace GameFramework.Taurus
{
	public class SettingModuleEditor : ModuleEditorBase
	{
		public SettingModuleEditor(string name, Color mainColor, GameMode gameMode)
	: base(name, mainColor, gameMode)
		{ }

		public override void OnDrawGUI()
		{
			GUILayout.BeginVertical("HelpBox");

            DrawDebugger();
            //DrawFrameRate();
            GUI.color = Color.white;

			GUILayout.EndVertical();
		}

		public override void OnClose()
		{
		}

        void DrawDebugger()
        {
            GUILayout.BeginVertical("HelpBox");
            GUI.color = _gameMode.DebugEnable ? Color.white : Color.gray;
            _gameMode.DebugEnable = GUILayout.Toggle(_gameMode.DebugEnable, "Show Debugger");
            GUILayout.EndVertical();
        }

        void DrawFrameRate()
        {
            GUILayout.BeginVertical("HelpBox");
            int frameRate = EditorGUILayout.IntSlider("Frame Rate", _gameMode.FrameRate, 1, 120);
            if (frameRate != _gameMode.FrameRate)
            {
                if (EditorApplication.isPlaying)
                {
                    _gameMode.FrameRate = frameRate;
                }
                else
                {
                    _gameMode.FrameRate = frameRate;
                }
            }
            GUILayout.EndVertical();
        }

        void DrawGameSpeed()
        {
        }
	}
}