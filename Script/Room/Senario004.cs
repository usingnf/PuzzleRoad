using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Senario004 : Stage
{
    public GameObject[] floors;
    [SerializeField] private GameObject[] hintObj;

    private bool isEnd = false;
    protected void Start()
    {
        base.Start();
        SetQuestion(); ;
    }
    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }
    public void SetQuestion()
    {
        EnterFloor(0);
    }

    //Inspector
    public void EnterFloor(int index)
    {
        foreach (GameObject obj in floors)
        {
            obj.SetActive(false);
        }
        floors[index].SetActive(true);
    }

    public void Event_First()
    {
        StartCoroutine(CoEvent_First());
    }

    private IEnumerator CoEvent_First()
    {
        string str = Language.Instance.Get("UI_Toast_CameraRotate");
        str = MyText.ConvertText(str, MyKey.CameraZ.ToString(), MyKey.CameraC.ToString());
        UIManager.Toast(str);
        yield return new WaitForSeconds(2.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario004_FirstEnter", null, MusicStart);
    }

    private void MusicStart()
    {
        if (isEnd)
            return;
        //SoundManager.Instance.StartMusicInterupt("Music_Exciting_Adventure", 1.0f, 1.0f);
        SoundManager.Instance.StartMusic("Music_Exciting_Adventure", 0.2f, false);
        if(coEvent != null)
            StopCoroutine(coEvent);
        coEvent = CoEvent(SoundManager.Instance.GetAudioMusic().clip.length);
        StartCoroutine(coEvent);
    }

    private IEnumerator coEvent = null;
    private IEnumerator CoEvent(float t)
    {
        yield return new WaitForSeconds(t);
        if (isEnd)
            yield break;
        DialogManager.Instance.ShowDialog("Dialog_Senario004_Delay", null, MusicStart2, ((int)(SoundManager.Instance.GetAudioMusic().clip.length)).ToString());
    }
    private void MusicStart2()
    {
        if (isEnd)
            return;
        SoundManager.Instance.StartMusic("Music_Enjoying_Freedom", 0.2f, true, 1.0f);
    }

    public void Event_End()
    {
        isEnd = true;
        if (coEvent != null)
            StopCoroutine(coEvent);
        DialogManager.Instance.ShowDialog("Dialog_Senario004_End", null, MusicStart2);
        SoundManager.Instance.StopMusic(0.5f);
    }

    public override void Hint()
    {
        if (hintIndex == 0)
        {
            UIManager.Toast(Language.Instance.Get("Hint_1004_1"));
            hintIndex = 1;
            foreach(GameObject obj in hintObj)
            {
                obj.SetActive(true); 
            }
        }
        else
            base.Hint();
    }
}
