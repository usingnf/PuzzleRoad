using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senario015 : Stage
{
    [SerializeField] private UI_Page page001;
    [SerializeField] private Door door;
    [SerializeField] private UI_Keypad keypad;

    private bool isBadEnd = false;
    protected void Start()
    {
        base.Start();
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
        page001.SetTitle("Senario015_Title", true);
        page001.AddContent("Senario015_Content_1", true);
        page001.AddContent("Senario015_Content_2", true);
        page001.AddContent("Senario015_Content_3", true);
        page001.AddContent("Senario015_Content_4", true);
        page001.SetEnd("Senario015_Answer", true, answer.ToString());

        //page001.SetTitle("???");
        //page001.AddContent("비밀을 알아내는 것이 항상 좋은 것만은 아니라는 것을 알게 되었다.");
        //page001.AddContent("만약 내가 비밀을 몰랐다면 아름다운 끝을 가질 수 있었을까.");
        //page001.AddContent("모든 것은 가정일 뿐. 이제 나는 내가 할 수 있는 최선의 수단을 사용할 수밖에 없어.");
        //page001.AddContent("난 죽고 싶지 않아.");
        //page001.SetEnd("Password:   " + answer);
        keypad.SetAnswer(answer);
    }

    public void Event_BadEnd()
    {
        if (isBadEnd)
            return;
        isBadEnd = true;
        StartCoroutine(BadEnd());
    }

    private IEnumerator BadEnd()
    {
        if (endElevator.GetIsTrueRoute())
            yield break;
        endElevator.ExcuteSolo(PlayerUnit.player, true);
        if(DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario015_End");
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.StartBadEnd();
    }
}
