using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Example
{
    public class GlobalManager : MonoBehaviour
    {


#if MODE_RESOURCE
        // 绝对路径引用,针对非Resrouce目录引用模式
        public const string ResourceRootPath = "Assets/Dependency/TFlowFramework/Example/Resources/";
        public const string UIPrefabRootPath = ResourceRootPath + "Prefab/UI/";
        public const string ModelRootPath = ResourceRootPath + "Prefab/Model/";
        public const string HotfixRootPath = ResourceRootPath + "Hotfix/";
        public const string SceneRootPath = ResourceRootPath + "Scene/";
        public const string DataTableRootPath = ResourceRootPath + "DataTable/";
        public const string LanguageRootPath = ResourceRootPath + "Language/";
#else
        // Resource目录引用
        public const string UIPrefabRootPath = "Prefab/UI/";
        public const string ModelRootPath = "Prefab/Model/";
        public const string HotfixRootPath = "Hotfix/";
        public const string SceneRootPath = "Scene/";
        public const string DataTableRootPath = "DataTable/";
        public const string LanguageRootPath = "Language/";
#endif

    }
}
