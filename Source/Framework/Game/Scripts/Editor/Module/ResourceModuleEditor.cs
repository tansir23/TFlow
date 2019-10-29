//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #资源模块编辑器# </describe>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Taurus
{
	public class ResourceModuleEditor : ModuleEditorBase
	{
		private BuildTargetGroup _lastBuildTargetGroup;
		private string _lastScriptingDefineSymbols;

		public ResourceModuleEditor(string name, Color mainColor, GameMode gameMode)
			: base(name, mainColor, gameMode)
		{
			//获取当前的BuildTargetGroup
			_lastBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
			_lastScriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);
		}

		public override void OnDrawGUI()
		{
			GUILayout.BeginVertical("HelpBox");

			GUILayout.BeginHorizontal("HelpBox");
			GUILayout.Label("Define",GUILayout.Width(50));
			string scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);
			_lastScriptingDefineSymbols = GUILayout.TextArea(_lastScriptingDefineSymbols);
			if (GUILayout.Button("OK",GUILayout.Width(40))&&!_lastScriptingDefineSymbols.Equals(scriptingDefineSymbols))
			{
				_lastBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup, _lastScriptingDefineSymbols);
			}
			GUILayout.EndHorizontal();


			_gameMode.ResUpdateType =
					(ResourceUpdateType)EditorGUILayout.EnumPopup("Resource Update Type", _gameMode.ResUpdateType);

			if (_gameMode.ResUpdateType == ResourceUpdateType.Local || _gameMode.ResUpdateType == ResourceUpdateType.Update)
			{
				//        _gameMode.ResUpdateType =
				//(ResourceUpdateType)EditorGUILayout.EnumPopup("Resource Update Type", _gameMode.ResUpdateType);
				if (_gameMode.ResUpdateType == ResourceUpdateType.Update)
				{
					_gameMode.ResUpdatePath =
						EditorGUILayout.TextField("Resource Update Remote Path", _gameMode.ResUpdatePath);
					_gameMode.LocalPathType =
						(PathType)EditorGUILayout.EnumPopup("Local Path Type", PathType.ReadWrite);
				}
				else
				{
					_gameMode.LocalPathType =
						(PathType)EditorGUILayout.EnumPopup("Local Path Type", _gameMode.LocalPathType);
				}
				string path = "";
				switch (_gameMode.LocalPathType)
				{
					case PathType.DataPath:
						path = Application.dataPath;
						break;
					case PathType.ReadOnly:
						path = Application.streamingAssetsPath;
						break;
					case PathType.ReadWrite:
						path = Application.persistentDataPath;
						break;
					case PathType.TemporaryCache:
						path = Application.temporaryCachePath;
						break;
				}

				EditorGUILayout.LabelField("Path", path);
			}

			GUILayout.EndVertical();
		}


		public override void OnClose()
		{
		}

	}
}