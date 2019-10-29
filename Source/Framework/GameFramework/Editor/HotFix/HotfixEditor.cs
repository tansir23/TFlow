//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> # HotFix Editor# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace GameFramework.Taurus{

    public class HotfixEditor : EditorWindow
    {
        public bool isHotfix_xLua = false;
        public bool isHotfix_ILRumtime = false;
        public bool isNoHotfix = false;

        private bool[] ischange = new bool[] { false, false, false};

        private string currentHotfix = "";
        private BuildTargetGroup _lastBuildTargetGroup;
        private string _lastScriptingDefineSymbols;

        [MenuItem("GF/Hotfix")]
        public static void AssetBundilesOptions()
        {
            GetWindowWithRect<HotfixEditor>(new Rect(200, 300, 400, 300), false, "Hotfix Setting");
           
        }

        private void OnEnable()
        {
            CheckHotfix();

            //获取当前的BuildTargetGroup
            _lastBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            _lastScriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);
        }
        private void CheckHotfix()
        {
            string scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);
            if (scriptingDefineSymbols.Contains("HOTFIX_ILRuntime"))
                SetCurrentILRuntime();
            else if (scriptingDefineSymbols.Contains("HOTFIX_XLUA"))
                SetCurrentXLua();
            else
                SetCurrentNoHotfix();
        }
        private void SetHotFix(string platform_hotfix)
        {
            string scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);
            if (platform_hotfix.Equals("NoHotfix"))
            {
                scriptingDefineSymbols = scriptingDefineSymbols.Replace(";HOTFIX_ILRuntime", "");
                scriptingDefineSymbols = scriptingDefineSymbols.Replace(";HOTFIX_XLUA", "");
            }

            if (scriptingDefineSymbols.Contains(platform_hotfix))
            {
                return;
            }
            else
            {
                scriptingDefineSymbols = scriptingDefineSymbols.Replace("NoHotfix", "");

                if (scriptingDefineSymbols.Contains("HOTFIX_ILRuntime"))
                    scriptingDefineSymbols = scriptingDefineSymbols.Replace("HOTFIX_ILRuntime", "HOTFIX_XLUA");
                else if (scriptingDefineSymbols.Contains("HOTFIX_XLUA"))
                    scriptingDefineSymbols = scriptingDefineSymbols.Replace("HOTFIX_XLUA", "HOTFIX_ILRuntime");
                else
                    scriptingDefineSymbols += (";"+platform_hotfix);
            }

            _lastBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup, scriptingDefineSymbols);
            Debug.Log(scriptingDefineSymbols);
        }

        void OnGUI()
        {

            //EditorGUILayout.BeginToggleGroup("Type", true);
            string scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);

            GUILayout.BeginHorizontal("HelpBox");
            GUILayout.Label("Current:");
            GUILayout.Label(currentHotfix);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("HelpBox");

            isHotfix_xLua = EditorGUILayout.Toggle("xLua", isHotfix_xLua);

            isHotfix_ILRumtime = EditorGUILayout.Toggle("ILRuntime", isHotfix_ILRumtime);

            isNoHotfix = EditorGUILayout.Toggle("Not Using Hotfix", isNoHotfix);

            //EditorGUILayout.EndToggleGroup();

            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                if (isHotfix_xLua && !ischange[0])
                {
                    SetCurrentXLua();
                    SetHotFix("HOTFIX_XLUA");
                }
                if (isHotfix_ILRumtime && !ischange[1])
                {
                    SetCurrentILRuntime();
                    SetHotFix("HOTFIX_ILRuntime");
                }
                if (isNoHotfix && !ischange[2])
                {
                    SetCurrentNoHotfix();
                    SetHotFix("NoHotfix");
                }

                
            }

        }

        void SetCurrentXLua()
        {
            currentHotfix = "xLua";
            isHotfix_ILRumtime = false;
            isNoHotfix = false;
            ischange = new bool[] { true, false, false };
        }

        void SetCurrentILRuntime()
        {
            currentHotfix = "ILRumtime";
            isHotfix_xLua = false;
            isNoHotfix = false;
            ischange = new bool[] { false, true, false };
        }

        void SetCurrentNoHotfix()
        {
            currentHotfix = "NoHotfix";
            isHotfix_xLua = false;
            isHotfix_ILRumtime = false;
            ischange = new bool[] { false, false, true };
        }
    }

}