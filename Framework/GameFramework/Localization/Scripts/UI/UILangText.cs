//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #本地化Text(UGUI)# </describe>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Sunny.Lang
{
    [RequireComponent(typeof(Text))]
    public class UILangText : MonoBehaviour
    {
        [SerializeField]
        private string key;

        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
            UpdateText();
        }

        private void Start()
        {
            LocalizationManager.OnChangedLanguage += Localizer_OnChangedLanguage;
        }

        private void OnDestroy()
        {
            LocalizationManager.OnChangedLanguage -= Localizer_OnChangedLanguage;
        }

        private void Localizer_OnChangedLanguage(object sender, LanguageEventArgs e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (!string.IsNullOrEmpty(key))
            {
                text.text = LocalizationManager.GetString(key);
            }
            else
            {
                Debug.LogWarning("Key in the UILangText component on " + gameObject.name + " is null or empty");
            }
        }
    }
}


