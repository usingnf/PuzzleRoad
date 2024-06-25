using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage0021 : Stage
{
    public List<GameObject> redObj;
    public List<GameObject> greenObj;
    public List<GameObject> blueObj;
    public List<GameObject> yellowObj;
    public GameObject[] lockObj;
    public UI_Page page;

    private List<int> openCount = new List<int>();
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
        page.Clear();
        page.SetTitle("Stage002_Button", true);
        page.AddContent("Stage002_Content_1", true);
        page.AddContent("Stage002_Content_2", true);
        page.AddContent("Stage002_Content_3", true);
    }

    public void SwtReverse()
    {
        foreach (GameObject obj in redObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        foreach (GameObject obj in greenObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        foreach (GameObject obj in blueObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        foreach (GameObject obj in yellowObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void SwtColor(Obj_RGBSwt.RGB rgb)
    {
        if (rgb == Obj_RGBSwt.RGB.Red)
        {
            foreach (GameObject obj in redObj)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
        else if (rgb == Obj_RGBSwt.RGB.Green)
        {
            foreach (GameObject obj in greenObj)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
        else if (rgb == Obj_RGBSwt.RGB.Blue)
        {
            foreach (GameObject obj in blueObj)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
        else if (rgb == Obj_RGBSwt.RGB.Yellow)
        {
            foreach (GameObject obj in yellowObj)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }

    public void OpenLock(int index)
    {
        lockObj[index].SetActive(false);
        if (!openCount.Contains(index))
            openCount.Add(index);
        else
            return;
        StartCoroutine(CoOpenLock());
    }

    private IEnumerator CoOpenLock()
    {
        yield return new WaitForSeconds(1.0f);
        if (openCount.Count == 1)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Stage002_FirstOpen");
        }
        else if (openCount.Count == 2)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Stage002_SecondOpen");
        }
        else if (openCount.Count == 3)
        {
            openCount.Add(111);
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Stage002_ThirdOpen");
        }
    }

    public void AddRed(GameObject obj)
    {
        redObj.Add(obj);
    }

    public void AddGreen(GameObject obj)
    {
        greenObj.Add(obj);
    }

    public void AddBlue(GameObject obj)
    {
        blueObj.Add(obj);
    }

    public void AddYellow(GameObject obj)
    {
        yellowObj.Add(obj);
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
        {
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage002_FirstEnter");
            if (coMusic != null)
                StopCoroutine(coMusic);
            coMusic = CoMusic(t + 1.0f);
            StartCoroutine(coMusic);
        }
        else
        {
            if (coMusic != null)
                StopCoroutine(coMusic);
            coMusic = CoMusic(1.0f);
            StartCoroutine(coMusic);
        }
        
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Dreaming_Of_Success", 0.2f, true);
    }

    public void Event_End()
    {
        StopMusic();
        StartCoroutine(CoEvent_End());
    }

    private IEnumerator CoEvent_End()
    {
        yield return new WaitForSeconds(1.0f);
        DialogManager.Instance.ShowDialog("Dialog_Stage002_End");
    }
    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
