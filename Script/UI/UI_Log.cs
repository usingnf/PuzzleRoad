using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Log : MonoBehaviour
{
    [SerializeField] private UI_LogContent prefab_content;
    [SerializeField] private Transform trans_content;
    public void Clear()
    {
        foreach(Transform trans in trans_content)
        {
            Destroy(trans.gameObject);
        }
    }

    public void AddLog(string name, string content, bool isId, string addText = "")
    {
        UI_LogContent log = Instantiate(prefab_content, trans_content);
        log.SetTextName(name, isId);
        log.SetTextContent(content, isId, addText);

        if(trans_content.childCount > 10)
        {
            Destroy(trans_content.GetChild(0).gameObject);
        }
    }
}
