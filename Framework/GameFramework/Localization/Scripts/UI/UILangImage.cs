//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #本地化图片(Image)# </describe>
//-----------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Sunny.Lang
{
    [RequireComponent(typeof(Image))]
    public class UILangImage : MonoBehaviour
    {
        [SerializeField]
        private string key;
        
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
            UpdateImage();
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
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (!string.IsNullOrEmpty(key))
            {
                image.sprite = LocalizationManager.GetSprite(key);
            }
            else
            {
                Debug.LogWarning("Key in the UILangImage component on " + gameObject.name + " is null or empty");
            }
        }
    }
}


