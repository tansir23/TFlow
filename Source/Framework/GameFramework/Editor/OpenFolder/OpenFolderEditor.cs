//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #打开文件夹编辑器# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEditor;

public class OpenFolderEditor  {

    //打开读写文件夹
    [MenuItem("GF/Tools/OpenFolder/PersistentDataPath")]
    private static void OpenPersistentDataPath()
    {
        EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
    }

    //打开只读文件夹
    [MenuItem("GF/Tools/OpenFolder/StreamingAssetsPath")]
    private static void OpenStreamingAssetsPath()
    {
        EditorUtility.OpenWithDefaultApp(Application.streamingAssetsPath);
    }

    //打开工程文件夹
    [MenuItem("GF/Tools/OpenFolder/DataPath")]
    private static void OpenDataPath()
    {
        EditorUtility.OpenWithDefaultApp(Application.dataPath);
    }

    //打开缓存文件夹
    [MenuItem("GF/Tools/OpenFolder/TemporaryCachePath")]
    private static void OpenTemporaryCachePath()
    {
        EditorUtility.OpenWithDefaultApp(Application.temporaryCachePath);
    }
}
