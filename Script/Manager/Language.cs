using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 언어 데이터 관리.
/// Key값과 해당 언어의 데이터를 저장.
/// 데이터는 Excel 파일을 변환하여 저장.
/// </summary>


[System.Serializable]
public struct LanguageStruct
{
    public string Key;
    public string English;
    public string Korean;
}
public class Language : Singleton<Language>
{
    
    public enum LanguageEnum
    {
        English = 0,
        Korean = 1,
        End = 2,
    }
    [SerializeField] private LanguageEnum currentLang;
    [SerializeField] private LanguageEnum currentVoiceLang;
    private UnityAction<LanguageEnum> changeLang;
    [SerializeField] private LanguageData langData;
    private Dictionary<string, string> English = new Dictionary<string, string>();
    private Dictionary<string, string> Korean = new Dictionary<string, string>();

    private void Awake()
    {
        Instance = this;
        if (Instance != this)
            return;

        DontDestroyOnLoad(this.gameObject);
        foreach (LanguageStruct data in langData.Lang)
        {
            if (string.IsNullOrEmpty(data.Key))
                continue;
            English[data.Key] = data.English;
            Korean[data.Key] = data.Korean;
        }
        currentLang = (LanguageEnum)PlayerPrefs.GetInt("Lang", (int)LanguageEnum.English);
        currentVoiceLang = (LanguageEnum)PlayerPrefs.GetInt("VoiceLang", (int)LanguageEnum.English);
    }

    public LanguageEnum GetCurrentLang()
    {
        return currentLang;
    }
    public LanguageEnum GetVoiceLang()
    {
        return currentVoiceLang;
    }

    public void SetCurrentLang(LanguageEnum lang)
    {
        currentLang = lang;
        PlayerPrefs.SetInt("Lang", (int)lang);
        if(changeLang != null)
            changeLang.Invoke(currentLang);
    }
    public void SetCurrentVoiceLang(LanguageEnum lang)
    {
        currentVoiceLang = lang;
        PlayerPrefs.SetInt("VoiceLang", (int)lang);
    }
    

    //Key값에 따른 언어 데이터 반환.
    public string Get(LanguageEnum lang, string key)
    {
        if(lang == LanguageEnum.English)
        {
            if (!English.ContainsKey(key))
                return "";
            return English[key];
        }
        else if(lang == LanguageEnum.Korean)
        {
            if (!Korean.ContainsKey(key))
                return "";
            return Korean[key];
        }

        return "";
    }

    public string Get(string key)
    {
        if(currentLang == LanguageEnum.English)
        {
            if (!English.ContainsKey(key))
                return "";
            return English[key];
        }
        else if(currentLang == LanguageEnum.Korean)
        {
            if (!Korean.ContainsKey(key))
                return "";
            return Korean[key];
        }
        return "";
    }

    //addText는 가변 데이터.
    public string Get(string key, string addText)
    {
        string str = "";
        if (currentLang == LanguageEnum.English)
        {
            if (English.ContainsKey(key))
                str =  English[key];
        }
        else if (currentLang == LanguageEnum.Korean)
        {
            if (Korean.ContainsKey(key))
                str = Korean[key];
        }

        if (str.Contains("/@"))
        {
            str = str.Replace("/@", addText);
        }
        return str;
    }

    public void AddChange(UnityAction<LanguageEnum> func)
    {
        changeLang += func;
    }

    public void RemoveChange(UnityAction<LanguageEnum> func)
    {
        if (changeLang == null)
            return;
        changeLang -= func;
    }
}
