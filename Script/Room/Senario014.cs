using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Senario014 : Stage
{
    [SerializeField] private GameObject obj_page;
    [SerializeField] private UI_Page page;
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

    }

    public void OpenPage(int index)
    {
        if (obj_page.activeSelf)
        {
            obj_page.SetActive(false);
            return;
        }
        page.Clear();
        page.SetEndColor(Color.white);
        page.SetEnd("", false);
        if (index == 0)
        {
            page.SetTitle("Senario014_Content_1_1", true);
            page.AddContent("Senario014_Content_1_2", true);
            page.SetEnd("Senario014_Content_1_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        else if(index == 1)
        {
            page.SetTitle("Senario014_Content_2_1", true);
            page.AddContent("Senario014_Content_2_2", true);
            page.SetEnd("Senario014_Content_2_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        else if (index == 2)
        {
            page.SetTitle("Senario014_Content_3_1", true);
            page.AddContent("Senario014_Content_3_2", true);
            page.SetEnd("Senario014_Content_3_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        else if (index == 3)
        {
            page.SetTitle("Senario014_Content_4_1", true);
            page.AddContent("Senario014_Content_4_2", true);
            page.SetEnd("Senario014_Content_4_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        else if (index == 4)
        {
            page.SetTitle("Senario014_Content_5_1", true);
            page.AddContent("Senario014_Content_5_2", true);
            page.SetEnd("Senario014_Content_5_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        else if (index == 5)
        {
            page.SetTitle("Senario014_Content_6_1", true);
            page.AddContent("Senario014_Content_6_2", true);
            page.SetEnd("Senario014_Content_6_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        else if (index == 6)
        {
            page.SetTitle("Senario014_Content_7_1", true);
            page.AddContent("Senario014_Content_7_2", true);
            page.SetEnd("Senario014_Content_7_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        else if (index == 7)
        {
            page.SetTitle("Senario014_Content_8_1", true);
            page.AddContent("Senario014_Content_8_2", true);
            page.SetEnd("Senario014_Content_8_3", true);
            page.SetEndColor(new Color(0, 0, 0, 0.2f));
        }
        
        
        obj_page.SetActive(true);
        //Event_Page(index);
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
        yield return new WaitForSeconds(0.5f);
        if(DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario014_FirstEnter");
    }

    public void Event_End()
    {
        StopMusic();
    }

    public void Event_Page(int index)
    {
        StartCoroutine (CoEvent_Page(index));
    }

    private IEnumerator CoEvent_Page(int index)
    {
        yield return new WaitForSeconds(0.5f);
        if (index == 0)
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario014_First");
        //else if (index == 1)
        //    DialogManager.Instance.ShowDialog("Dialog_Senario014_Second");
        //else if (index == 2)
        //    DialogManager.Instance.ShowDialog("Dialog_Senario014_Third");
        //else if (index == 3)
        //    DialogManager.Instance.ShowDialog("Dialog_Senario014_Fourth");
        //else if (index == 4)
        //    DialogManager.Instance.ShowDialog("Dialog_Senario014_Fifth");
        //else if (index == 5)
        //    DialogManager.Instance.ShowDialog("Dialog_Senario014_Sixth");
        //else if (index == 6)
        //    DialogManager.Instance.ShowDialog("Dialog_Senario014_Seventh");
        //else if (index == 7)
        //    DialogManager.Instance.ShowDialog("Dialog_Senario014_Eighth");
    }

    public void ClosePage()
    {
        obj_page.SetActive(false);
    }

    public void Event_Dialog(int index)
    {
        if (index == 1)
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario014_Second");
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        if(SoundManager.Exist())
            SoundManager.Instance.StartMusic("Music_Emotional_Piano_Romance", 0.4f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        if (SoundManager.Exist())
            SoundManager.Instance.StopMusic(0.5f);
    }
}
