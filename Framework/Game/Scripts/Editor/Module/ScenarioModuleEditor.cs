//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #场景模块编辑器# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEditor;

namespace GameFramework.Sunny
{
    public class ScenarioModuleEditor : ModuleEditorBase
    {

        public ScenarioModuleEditor(string name, Color mainColor, GameMode gameMode)
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
            
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Loaded Scene Names", GetSceneNameString(GameMode.Scene.GetLoadedSceneAssetNames()));
                EditorGUILayout.LabelField("Loading Scene Names", GetSceneNameString(GameMode.Scene.GetLoadingSceneAssetNames()));
                //EditorGUILayout.LabelField("Unloading Scene Asset Names", GetSceneNameString(t.GetUnloadingSceneAssetNames()));
                //EditorGUILayout.ObjectField("Main Camera", t.MainCamera, typeof(Camera), true);

                
            }
            GUILayout.EndVertical();
        }

        public override void OnClose()
        {
        }

        private string GetSceneNameString(string[] sceneAssetNames)
        {
            if (sceneAssetNames == null || sceneAssetNames.Length <= 0)
            {
                return "<Empty>";
            }

            string sceneNameString = string.Empty;
            foreach (string sceneAssetName in sceneAssetNames)
            {
                if (!string.IsNullOrEmpty(sceneNameString))
                {
                    sceneNameString += ", ";
                }

                sceneNameString += ScenarioManager.GetSceneName(sceneAssetName);
            }

            return sceneNameString;
        }
    }
}