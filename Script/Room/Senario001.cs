using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Senario001 : Stage
{
    [SerializeField] private UI_Page page001;
    [SerializeField] private Door door;
    [SerializeField] private UI_Keypad keypad;
    [SerializeField] private TextMeshPro text_interactKey;
    protected void Start()
    {
        base.Start();
        text_interactKey.text = MyKey.Interact.ToString();
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }
    public void SetQuestion()
    {
        int answer = Random.Range(1000, 9999);
        page001.Clear();
        page001.SetTitle("Senario001_Title", true);
        page001.AddContent("Senario001_Content_1", true);
        page001.AddContent("Senario001_Content_2", true);
        page001.AddContent("Senario001_Content_3", true);
        page001.AddContent("Senario001_Content_4", true);
        //page001.AddContent("모든 존재는 욕망을 채우기 위해 살아간다고 해. 하지만 내게는 날 행복하게 해줄 것도, 불행하게할 시련도 없어.");
        //page001.AddContent("내가 할 수 있는 것은 방황뿐이야. 나는 무엇도 가지고 있지 않고, 아무것도 알지 못해. 지금 내가 느끼는 감정은 공포인 것일까?");
        //page001.AddContent("끝없는 의문이 나를 잡아먹는 것만 같아.");
        //page001.AddContent("나는 대체 무엇일까?");
        //page001.SetEnd($"{Language.Instance.Get("Senario001_Answer")}{answer}");
        page001.SetEnd("Senario001_Answer", true, answer.ToString());
        keypad.SetAnswer(answer);
    }
}
