//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #本地化管理器# </describe>
//-----------------------------------------------------------------------

using GameFramework.Taurus.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace GameFramework.Taurus
{
	public sealed class LocalizationManager : GameFrameworkModule
	{
        private ResourceManager _resource;

        public static string rootPath = "Lang/";

        /// <summary>
        /// 配置AB包名称和xml文件路径
        /// </summary>
        private string assetsBundleName = "";
        private string xmlRootPath = "";

        private const string keyPref = "currentLang";
        private const Language defaultLang = Language.English;

        private static Dictionary<string, string> stringsDict;
        public static Language Language { get; private set; }

        private bool isInit = false;

        public LocalizationManager()
        {
            _resource = GameFrameworkMode.GetModule<ResourceManager>();
            stringsDict = new Dictionary<string, string>();
            isInit = false;
        }

        /// <summary>
        /// 配置参数
        /// </summary>
        public void SetConfiguration(string assetBundleName = "language", string xmlRootPath = "Assets/Resource/Lang")
        {
            this.assetsBundleName = assetBundleName;
            this.xmlRootPath = xmlRootPath;
            isInit = true;


           string currLangString = PlayerPrefs.GetString(keyPref, "default");
           if (currLangString.Equals("default"))
           {
                SetLanguage(ConvertSystemLanguage(Application.systemLanguage));
           }
           else
           {
               Language code = (Language)Enum.Parse(typeof(Language), currLangString);
                SetLanguage(code);

           }

           
        }

        /// <summary>
        /// 设置语言
        /// </summary>
        /// <param name="code"></param>
        public void SetLanguage(Language lang = Language.Chinese)
        {
            if (!isInit)
                throw new Exception("Please SetConfiguration first.");

            Language = lang;
            stringsDict = ReadXML(lang);

            PlayerPrefs.SetString(keyPref, lang.ToString());
            PlayerPrefs.Save();

            if (OnChangedLanguage != null) OnChangedLanguage(null, new LanguageEventArgs(lang));
        }
        
        /// <summary>
        /// 读取XML文件
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public Dictionary<string, string> ReadXML(Language lang = Language.Chinese)
        {
            string path = "";

            if(_resource.ResUpdateType == ResourceUpdateType.Resource)
                path = xmlRootPath + lang.ToString() + "/" + lang.ToString();
            else
                path = xmlRootPath + lang.ToString() + "/" + lang.ToString() + ".xml";

            TextAsset xmlAsset = _resource.LoadAssetSync<TextAsset>(assetsBundleName, path);
            //TextAsset xmlAsset = Resources.Load(path) as TextAsset;

            if (!xmlAsset)
            {
                Debug.LogError("File not found at path " + path);
                return null;
            }

            var reader = XmlReader.Create(new StringReader(xmlAsset.text));

            Dictionary<string, string> data = new Dictionary<string, string>();

            var key = string.Empty;
            var value = string.Empty;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        key = reader["key"];
                        break;
                    case XmlNodeType.Text:
                        value = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        if (!string.IsNullOrEmpty(key))
                        {
                            if (data.ContainsKey(key))
                            {
                                throw new Exception("Key " + key + " already exists in string dictionary!");
                            }

                            data.Add(key, value);
                            key = string.Empty;
                            value = string.Empty;
                        }
                        break;
                }
            }

            reader.Close();

            return data;
        }

        public static string GetString(string key)
        {
            if (stringsDict.ContainsKey(key))
            {
                string text = stringsDict[key];

                //need to support Rich text
                text = text.Replace("&lt;", "<");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&amp;", "&");

                return text;
            }

            return key;
        }
        public static Sprite GetSprite(string key)
        {
            Sprite sprite = Resources.Load<Sprite>(rootPath + Language.ToString() + "/" + key);
            if (sprite == null)
            {
                Debug.LogWarning("Sprite " + key + " not found in " + Language.ToString() + " language");
            }
            return sprite;
        }

        public Sprite GetLocalSprite(string key)
        {
            string path = rootPath + Language.ToString() + "/" + key;
           //Sprite sprite = Resources.Load<Sprite>(rootPath + Language.ToString() + "/" + key);
            Sprite sprite = _resource.LoadAssetSync<Sprite>(assetsBundleName, path);

            if (sprite == null)
            {
                Debug.LogWarning("Sprite " + key + " not found in " + Language.ToString() + " language");
            }
            return sprite;
        }

        public string GetLocalText(string key)
        {
            if (stringsDict.ContainsKey(key))
            {
                string text = stringsDict[key];

                //need to support Rich text
                text = text.Replace("&lt;", "<");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&amp;", "&");

                return text;
            }

            return key;
        }

       

        public static Language ConvertSystemLanguage(SystemLanguage selected)
        {
            switch (selected)
            {
                case SystemLanguage.English:
                    return Language.English;
                case SystemLanguage.Spanish:
                    return Language.Spanish;
                case SystemLanguage.Portuguese:
                    return Language.PortuguesePortugal;
                case SystemLanguage.Russian:
                    return Language.Russian;
                case SystemLanguage.Turkish:
                    return Language.Turkish;
                case SystemLanguage.German:
                    return Language.German;
                case SystemLanguage.Italian:
                    return Language.Italian;
                default:
                    return defaultLang;
            }
        }

        public override void OnClose()
        {
            if (stringsDict != null)
            {
                stringsDict.Clear();
                stringsDict = null;
            }
        }

        public static event EventHandler<LanguageEventArgs> OnChangedLanguage = delegate { };
    }
}
