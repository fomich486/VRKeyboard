using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TevaVR.UI;

[CreateAssetMenu(fileName = "Keyboard Data", menuName = "KeyBoard Data", order = 51)]
public class KeyboardInfo : ScriptableObject{
    [Header("Languages Keyboards")]
    public List<Language> LanguageList = new List<Language>();
    [Header("Symbols Keyboards")]
    public List<Language> SymbolsList = new List<Language>();

    //TODO: Add methodes to return required list
    public Language GetReqiredLanguage(string _languageName)
    {
        foreach (var language in LanguageList)
        {
            if (_languageName == language.LanguageName)
                return language;
        }
        Debug.LogError("No required language found!");
        return null;
    }

    public Language GetReqiredSymbols(string _languageName)
    {
        foreach (var language in SymbolsList)
        {
            if (_languageName == language.LanguageName)
                return language;
        }
        Debug.LogError("No required language found!");
        return null;
    }

    public Language GetNextLanguage(string _languageName)
    {
        int currentLanguage = GetLanguageListPosition(_languageName, LanguageList);
        return GetNextListElement(currentLanguage, LanguageList);
    }

    public Language GetNextSymbols(string _symbolsPackName)
    {
        int currentSymbols = GetLanguageListPosition(_symbolsPackName, SymbolsList);
        return GetNextListElement(currentSymbols, SymbolsList);
    }

    private int GetLanguageListPosition(string _languageName, List<Language> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_languageName == _list[i].LanguageName)
                return i;
        }
        Debug.LogError("No required language found!");
        return 0;
    }


    private Language GetNextListElement(int currentLanguage, List<Language> _list)
    {
        if ((currentLanguage + 1) <= (_list.Count - 1))
        {
            int next = currentLanguage + 1;
            return _list[next];
        }
        else
        {
            return _list[0];
        }
    }
}

namespace TevaVR.UI { 
    [System.Serializable]
    public class KeyboardLineSymbols
    {
        public List<string> chars = new List<string>();
    }

    [System.Serializable]
    public class Language
    {
        public string LanguageName;
        public List<KeyboardLineSymbols> LineSymbols = new List<KeyboardLineSymbols>();
    }
}

//1.Предупреждение о воздействии прямого попадания света на линзы маски.
//2. О том, что можно вертеть головой.
//3. Как отцентрировать экран.
//4. Как управлять маской без пульта.
//5. Как вкл/выкл пульт.
//6. Как управлять пультом.
//7. Как свернуть приложение и включить WiFi. 