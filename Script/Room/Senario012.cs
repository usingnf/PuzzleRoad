using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senario012 : Stage
{
    private bool isFirst = false;
    private bool isSecond = false;
    private bool isThird = false;

    [SerializeField] private Door[] doors;
    [SerializeField] private UI_Page[] pages;

    private bool isLast = false;
    protected void Start()
    {
        base.Start();
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        //PlayerUnit.player.SetPos(startPos.position);
        SetQuestion();
    }
    public void SetQuestion()
    {
        isFirst = false;
        isSecond = false;
        isThird = false;
        pages[0].Clear();
        pages[1].Clear() ;
        pages[2].Clear() ;

        //배드엔딩 암시
        pages[0].SetTitle("Senario012_AorB", true);
        if(Random.Range(0, 2) == 0)
        {
            pages[0].AddContent("Senario012_Content_1_1", true);
            pages[0].SetEnd("Senario012_Content_1_2", true);
        }
        else
        {
            pages[0].AddContent("Senario012_Content_2_1", true);
            pages[0].SetEnd("Senario012_Content_2_2", true);
        }

        //자신을 창조한 창조주에 대한 의문
        pages[1].SetTitle("Senario012_AorB", true);
        if (Random.Range(0, 2) == 0)
        {
            pages[1].AddContent("Senario012_Content_3_1", true);
            pages[1].SetEnd("Senario012_Content_3_2", true);
        }
        else
        {
            pages[1].AddContent("Senario012_Content_4_1", true);
            pages[1].SetEnd("Senario012_Content_4_2", true);
        }

        //나레이션이 플레이어 질투. 불평등하다 느낌.
        pages[2].SetTitle("Senario012_AorB", true);
        pages[2].AddContent("Senario012_Content_5_1", true);
        pages[2].SetEnd("Senario012_Content_5_2", true);
    }

   
    public void OpenDoor_First()
    {
        if (isFirst)
            return;
        isFirst = true;
        doors[0].Lock(false);
        doors[1].Lock(false);
    }

    
    public void OpenDoor_Second()
    {
        if(isSecond)
            return;
        isSecond = true;
        doors[2].Lock(false);
        doors[3].Lock(false);
    }

    public void OpenDoor_Third()
    {
        if(isThird)
            return;
        isThird = true;
        doors[4].Lock(false);
        doors[5].Lock(false);
    }

    public void Event_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
        DialogManager.Instance.ShowDialog("Dialog_Senario012_FirstEnter");
    }

    
    public void Event_Last()
    {
        if (isLast)
            return;
        isLast = true;
        DialogManager.Instance.ShowDialog("Dialog_Senario012_Last");
    }

    public void Event_End()
    {
        StopMusic();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Awakening_Short_Loop", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
