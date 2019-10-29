//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #数据节点编辑器# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Taurus
{
    public class DataNodeModuleEditor : ModuleEditorBase
    {
        public DataNodeModuleEditor(string name, Color mainColor, GameMode gameMode)
    : base(name, mainColor, gameMode)
        {

        }

        public override void OnDrawGUI()
        {

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            GUILayout.BeginVertical("HelpBox");
            DrawDataNode(GameMode.Node.Root);
            GUILayout.EndVertical();
           
        }

        public override void OnClose()
        {
        }

        private void DrawDataNode(DataNode dataNode)
        {

            EditorGUILayout.LabelField(dataNode.FullName, dataNode.ToDataString());
            DataNode[] child = dataNode.GetAllChild();
            foreach (DataNode c in child)
            {
                DrawDataNode(c);
            }
        }
    }
}
