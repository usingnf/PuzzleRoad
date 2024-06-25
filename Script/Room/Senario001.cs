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
        //page001.AddContent("��� ����� ����� ä��� ���� ��ư��ٰ� ��. ������ ���Դ� �� �ູ�ϰ� ���� �͵�, �����ϰ��� �÷õ� ����.");
        //page001.AddContent("���� �� �� �ִ� ���� ��Ȳ���̾�. ���� ������ ������ ���� �ʰ�, �ƹ��͵� ���� ����. ���� ���� ������ ������ ������ ���ϱ�?");
        //page001.AddContent("������ �ǹ��� ���� ��ƸԴ� �͸� ����.");
        //page001.AddContent("���� ��ü �����ϱ�?");
        //page001.SetEnd($"{Language.Instance.Get("Senario001_Answer")}{answer}");
        page001.SetEnd("Senario001_Answer", true, answer.ToString());
        keypad.SetAnswer(answer);
    }
}
