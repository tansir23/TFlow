//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #数据表模块编辑器# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEditor;

namespace GameFramework.Sunny
{
    public class DataTableModuleEditor : ModuleEditorBase
    {
        //private Gam UIRoot;

        public DataTableModuleEditor(string name, Color mainColor, GameMode gameMode)
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
            EditorGUILayout.LabelField("Data Table Count", GameMode.DataTable.Count.ToString());
            DataTableBase[] dataTables = GameMode.DataTable.GetAllDataTables();
            foreach (DataTableBase dataTable in dataTables)
            {
                DrawDataTable(dataTable);
            }
            GUILayout.EndVertical();
        }

        public override void OnClose()
        {
        }

        private void DrawDataTable(DataTableBase dataTable)
        {
            EditorGUILayout.LabelField(Utility.Text.GetFullName(dataTable.Type, dataTable.Name), Utility.Text.Format("{0} Rows", dataTable.Count.ToString()));
        }
    }
}