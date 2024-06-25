using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string str;
    //[SerializeField] private string idStr;
    [SerializeField] private bool isId = false;
    [SerializeField] private string addText = "";

    public string text
    {
        get { return str; }
        set { SetText(value); }
    }
    public Color color
    {
        set { _text.color = value; }
    }
    private void OnEnable()
    {
        if(!string.IsNullOrEmpty(str) && !string.Equals(str, _text.text))
            SetText(str, isId, addText);
    }

    private void Start()
    {
        Language.Instance.AddChange(ChangeLanguage);
    }

    private void OnDestroy()
    {
        Language.Instance.RemoveChange(ChangeLanguage);
    }

    private void ChangeLanguage(Language.LanguageEnum lang)
    {
        SetText(str, isId, addText);
    }

    public string SetText(string str, bool isId = false, string addText = "")
    {
        if (text == null)
            return null;
        if(isId)
        {
            this.str = str;
            if(Language.Exist())
                str = Language.Instance.Get(Language.Instance.GetCurrentLang(), str);
        }

        if(str.Contains("/@"))
        {
            str = str.Replace("/@", addText);
        }
        if (str.Contains("\\n"))
        {
            str = str.Replace("\\n", "\n");
        }
        _text.text = str;
        if(!isId)
            this.str = str;
        this.addText = addText;
        this.isId = isId;
        return str;
    }

    public void SetColor(Color color)
    {
        if (text == null)
            return;
        _text.color = color;
    }

    public TextMeshProUGUI GetTMP()
    {
        return _text;
    }

    public static string ConvertText(string str, string addText)
    {
        if (str.Contains("/@"))
        {
            str = str.Replace("/@", addText);
        }
        return str;
    }

    public static string ConvertText(string str, string addText, string addText2)
    {
        if (str.Contains("/@"))
        {
            str = str.Replace("/@", addText);
        }
        if (str.Contains("/#"))
        {
            str = str.Replace("/#", addText2);
        }
        return str;
    }
}
