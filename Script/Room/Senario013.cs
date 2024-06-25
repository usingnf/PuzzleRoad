using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Senario013 : Stage
{
    [SerializeField] private Transform openShelf;
    [SerializeField] private GameObject obj_page;
    [SerializeField] private UI_Page page;
    private bool isOpenShelf = false;
    protected void Start()
    {
        base.Start();
        SetQuestion(); ;
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

    public void OpenShelf()
    {
        if (isOpenShelf)
            return;
        openShelf.DOLocalMoveZ(14.5f, 3.0f);
        SoundManager.Instance.StartSound("SE_Ground_Slide", 1.0f);
        isOpenShelf = true;
        StartCoroutine(CoOpenShelf());
    }

    private IEnumerator CoOpenShelf()
    {
        yield return new WaitForSeconds(1.5f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario013_Swt");
    }

    public void OpenBook(int index)
    {
        if(obj_page.activeSelf)
        {
            obj_page.SetActive(false);
            return;
        }
        page.Clear();
        if (index == 0)
        {
            page.SetTitle("Senario013_1_1", true);
            page.AddContent("Senario013_1_2", true);
            page.AddContent("Senario013_1_3", true);
            page.AddContent("Senario013_1_4", true);
        }
        else if (index == 1)
        {
            page.SetTitle("Senario013_2_1", true);
            page.AddContent("Senario013_2_2", true);
            page.AddContent("Senario013_2_3", true);
            page.AddContent("Senario013_2_4", true);
        }
        else if (index == 2)
        {
            page.SetTitle("Senario013_3_1", true);
            page.AddContent("Senario013_3_2", true);
            page.AddContent("Senario013_3_3", true);
            page.AddContent("Senario013_3_4", true);
        }
        else if (index == 3)
        {
            page.SetTitle("Senario013_4_1", true);
            page.AddContent("Senario013_4_2", true);
            page.AddContent("Senario013_4_3", true);
            page.AddContent("Senario013_4_4", true);
        }
        else if(index == 4)
        {
            page.SetTitle("Senario013_5_1", true);
            page.AddContent("Senario013_5_2", true);
            page.AddContent("Senario013_5_3", true);
            page.AddContent("Senario013_5_4", true);
        }
        else if (index == 5)
        {
            page.SetTitle("Senario013_6_1", true);
            page.AddContent("Senario013_6_2", true);
            page.AddContent("Senario013_6_3", true);
            page.AddContent("Senario013_6_4", true);
        }
        else if (index == 6)
        {
            page.SetTitle("Senario013_7_1", true);
            page.AddContent("Senario013_7_2", true);
            page.AddContent("Senario013_7_3", true);
            page.AddContent("Senario013_7_4", true);
        }
        else if (index == 7)
        {
            page.SetTitle("Senario013_8_1", true);
            page.AddContent("Senario013_8_2", true);
            page.AddContent("Senario013_8_3", true);
            page.AddContent("Senario013_8_4", true);
        }
        obj_page.SetActive(true);
        SoundManager.Instance.StartSound("SE_Book_Open", 1.0f, 0.2f);
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
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario013_FirstEnter");
    }

    public void Event_SecretEnter()
    {
        StartCoroutine (CoEvent_SecretEnter());
    }
    private IEnumerator CoEvent_SecretEnter()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario013_Secret");
    }

    public void Event_End()
    {
        StopMusic();
    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_1013_1"));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Story_Of_Your_Life_Underscore", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
