//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 tanSir. All rights reserved.
// </copyright>
// <describe> #GameMode的编辑器类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Profiling;

namespace GameFramework.Sunny
{
    [CustomEditor(typeof(GameMode))]
    public class GameModeEditor : Editor
    {
        private GameMode _gameMode;
		
        ////Color.cyan;
        private Color _defaultColor;

        //资源加载模块的颜色
        private Color _resourceColor = new Color(0.141f, 0.408f, 0.635f, 1.0f);

        //可操作模块的颜色
        private Color _operationColor = new Color(0.953f, 0.424f, 0.129f, 1.0f);

        //状态模块的颜色
        private Color _stateColor = new Color(0.141f, 0.408f, 0.635f, 1.0f);

        //配置表模块的颜色
        private Color _dataTableColor = new Color(0.989f, 0.686f, 0.090f, 1.0f);

        //数据节点模块的颜色
        private Color _nodeDataColor = new Color(0.435f, 0.376f, 0.667f, 1.0f);

        //步骤模块的颜色
        private Color _stepColor = new Color(0.439f, 0.631f, 0.624f, 1.0f);

        //设置模块的颜色
        private Color _settingColor = new Color(0.989f, 0.686f, 0.090f, 1.0f);

        //UI模块颜色
        private Color _uiColor = new Color(0.141f, 0.408f, 0.635f, 1.0f);

        //场景模块颜色
        private Color _sceneColor = new Color(0.141f, 0.408f, 0.635f, 1.0f);


        //所有的模块
        private List<ModuleEditorBase> _listModuleEditors;

		private void OnEnable()
        {
			_listModuleEditors = new List<ModuleEditorBase>();

			_gameMode = target as GameMode;

            _defaultColor = GUI.color;

            _listModuleEditors.Add(new SettingModuleEditor("Setting Module", _settingColor, _gameMode));
            _listModuleEditors.Add(new ResourceModuleEditor("Resource Module", _resourceColor, _gameMode));
			_listModuleEditors.Add(new StateModuleEditor("State Module", _stateColor, _gameMode));
            _listModuleEditors.Add(new UIModuleEditor("UI Module", _uiColor, _gameMode));
            //_listModuleEditors.Add(new DataTableModuleEditor("DataTable Module", _dataTableColor, _gameMode));
            //_listModuleEditors.Add(new DataNodeModuleEditor("DataNode Module", _nodeDataColor, _gameMode));
            //_listModuleEditors.Add(new ScenarioModuleEditor("Scene Module", _sceneColor, _gameMode));

        }

		private void OnDisable()
		{
			if (_listModuleEditors == null)
				return;

			for (int i = 0; i < _listModuleEditors.Count; i++)
			{
				_listModuleEditors[i].OnClose();
			}
			_listModuleEditors.Clear();
		}

		public override void OnInspectorGUI()
        {
			if (_gameMode == null || _listModuleEditors == null)
				return;

			GUILayout.BeginVertical();

			for (int i = 0; i < _listModuleEditors.Count; i++)
			{
				_listModuleEditors[i].OnInspectorGUI();
                GUILayout.Space(15);
			}

			GUILayout.EndVertical();

			EditorUtility.SetDirty(_gameMode);
		}

    }
}
