using System.Collections;
using UnityEngine;

public class Stage003 : Stage
{
    public enum GroundColor
    {
        None = -1,
        Red = 0,
        Green = 1,
        Blue = 2,
    }

    [SerializeField] private GroundColor selectColor = GroundColor.None;
    [SerializeField] private Obj_ColorSwtGround[] grounds;
    [SerializeField] private UI_Page page;
    public Obj_ColorSwtGround lastGround;

    private int failCount = 0;
    private bool isHalf = false;
    private bool isHalf2 = false;
    private bool isHalfFail = true;
    private int lastIndex = 0;

    protected void Start()
    {
        base.Start();
        SetQuestion();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.T)) 
    //    {
    //        Hint();
    //    }
    //}

    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
        lastGround = null;
        selectColor = GroundColor.None;
        PlayerUnit.player.SetPos(startPos.position);
        failCount += 1;
        if(isHalf)
        {
            if(isHalfFail)
            {
                isHalfFail = false;
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Stage003_HalfFail");
            }
        }
        else if(failCount == 2)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Stage003_Fail");
        }
        //PlayerUnit.player.transform.position = startPos.position;
    }
    public void SetQuestion()
    {
        for (int i = 0; i < grounds.Length; i++)
        {
            grounds[i].index = i+1;
        }
        SwtMat(1);
        page.Clear();
        page.SetTitle("Stage003_ColorTrain", true);
        page.AddContent("Stage003_Content_1", true);
        page.AddContent("Stage003_Content_2", true);
        //page.SetEnd("");
    }

    public void SelectColor(GroundColor color)
    {
        selectColor = color;
    }

    public void SwtMat(int index)
    {
        lastIndex = index;
        index = index % 2;
        for (int i = 0; i < grounds.Length; i++)
        {
            if (i % 2 == index)
            {
                //grounds[i].SetMat(true);
                grounds[i].StartVisible(true);
            }
            else
            {
                //grounds[i].SetMat(false);
                grounds[i].StartVisible(false);
            }
        }

 
    }

    public void SwtHint(int index)
    {
        index = index % 2;
        for (int i = 0; i < grounds.Length; i++)
        {
            if (i % 2 == index)
                grounds[i].StartHint();
        }
    }

    public GroundColor GetSelectColor()
    {
        return selectColor;
    }

    public int GetLastIndex()
    {
        return lastIndex;
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
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage003_FirstEnter");
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

    public void Event_Half()
    {
        if (isHalf)
            return;
        isHalf = true;
        StartCoroutine(CoEvent_Half());
    }

    private IEnumerator CoEvent_Half()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage003_Half");
    }

    public void Event_Half2()
    {
        if (isHalf2)
            return;
        isHalf2 = true;
        StartCoroutine(CoEvent_Half2());
    }

    private IEnumerator CoEvent_Half2()
    {
        yield return new WaitForSeconds(0.1f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage003_Half2");
    }

    public override void Hint()
    {
        if (hintIndex == 0)
        {
            SwtHint(lastIndex);
            UIManager.Toast(Language.Instance.Get("Hint_3_1"));
        }
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Sweet_Positive_Fun", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
