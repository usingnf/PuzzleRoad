using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Memo : MonoBehaviour
{
    [SerializeField] private SO_Memo memo;
    [SerializeField] private TextMeshProUGUI text_title;
    [SerializeField] private TextMeshProUGUI text_content;

    public void SetData(SO_Memo memo)
    {
        this.memo = memo;
        text_title.text = memo.memoName;
        text_content.text = memo.content;
    }
}
