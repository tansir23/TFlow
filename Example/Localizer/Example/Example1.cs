using System.Collections;
using System.Collections.Generic;
using GameFramework.Example;
using GameFramework.Taurus;
using IndigoBunting.Lang;
using UnityEngine;

public class Example1 : MonoBehaviour
{
    public void ChangeLanguage()
    {
        LocalizationManager  _localModule = GameFrameworkMode.GetModule<LocalizationManager>();

        if (LocalizationManager.Language == Language.Russian)
        {
            _localModule.SetLanguage(Language.English);
        }
        else
        {
            _localModule.SetLanguage(Language.Russian);
        }
    }
}