using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LogContent : MonoBehaviour
{
    [SerializeField] private MyText text_name;
    [SerializeField] private MyText text_content;
    public void SetTextName(string str, bool isId = false, string addText = "")
    {
        text_name.SetText(str, isId, addText);
    }

    public void SetTextContent(string str, bool isId = false, string addText = "")
    {
        text_content.SetText(str, isId, addText);
    }
}
