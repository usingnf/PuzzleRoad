using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage001 : Stage
{
    public int leftWeight = 0;
    public int rightWeight = 0;
    public UI_Balance balance;
    [SerializeField] private Inter_Balance inter_balance;
    public UI_Keypad keypad;
    public UI_Page page;
    public Inter_Weight[] inter_Weights;
    public int incorrestCount = 0;
    public int incorrestCountReset = 0;

    [SerializeField] private Transform[] weightTrans;
    private Vector3[] weightPos;

    [SerializeField] private Detector_Weight leftDetector;
    [SerializeField] private Detector_Weight rightDetector;

    private bool isEnd = false;
    [SerializeField] private int holdCount = 0;

    protected void Start()
    {
        base.Start();
        weightPos = new Vector3[weightTrans.Length];
        for(int i = 0; i < weightTrans.Length; i++)
            weightPos[i] = weightTrans[i].position;
        SetQuestion();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.T))
    //    {
    //        SetQuestion();
    //    }
    //}

    public override void ReStage()
    {
        base.ReStage();
        inter_balance.SetCount(4);
        InteractableObject holdWeight = PlayerUnit.player.GetHoldObj();
        if(holdWeight != null )
            holdWeight.Interact(PlayerUnit.player);
        for(int i = 0; i < weightTrans.Length; i++)
        {
            weightTrans[i].gameObject.SetActive(false);
            weightTrans[i].position = weightPos[i];
            weightTrans[i].gameObject.SetActive(true);
        }
        leftWeight = 0;
        rightWeight = 0;
        leftDetector.WeightReset();
        rightDetector.WeightReset();
        SetQuestion();
    }
    public void SetQuestion()
    {
        holdCount = 0;
        incorrestCountReset = 0;
        int aCount = Random.Range(2, 9);
        int aWeight = Random.Range(1, 5);
        int bWeight = aWeight + Random.Range(1, 5);
        if(bWeight > 9)
            bWeight = 9;
        List<int> list = new List<int>();
        for (int i = 0; i < inter_Weights.Length; i++)
        {
            if(i < aCount)
                list.Add(aWeight);
            else
                list.Add(bWeight);
        }
        
        int temp = 0;
        for(int i = 0; i < list.Count; i++)
        {
            temp = list[i];
            int rand = Random.Range(0, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
        for(int i = 0; i < inter_Weights.Length; i++)
        {
            if (list[i] == bWeight)
                inter_Weights[i].SetWeight(list[i], true);
            else
                inter_Weights[i].SetWeight(list[i], false);
        }

        if (leftWeight == 0 || rightWeight == 0)
            inter_balance.isZero = true;
        else
            inter_balance.isZero = false;


        page.Clear();
        page.SetTitle("Stage001_Balance", true);
        page.AddContent("Stage001_Content_1", true);
        page.AddContent("Stage001_Content_2", true);
        page.AddContent("Stage001_Content_3", true);
        page.AddContent("Stage001_Content_4", true);
        //page.AddContent("Stage001_Content_5", true);
        //page.AddContent($"  {Language.Instance.Get("Stage001_Content_1")}");
        //page.AddContent($"  {Language.Instance.Get("Stage001_Content_2")}");
        //page.AddContent($"  {Language.Instance.Get("Stage001_Content_3")}");
        //page.AddContent($"  {Language.Instance.Get("Stage001_Content_4")}");
        page.SetEnd("Stage001_Answer", true);

        keypad.SetAnswer(aCount * aCount);
        //Debug.Log(a + "/" + b);
    }

    public void AddWeight(Inter_Weight weight, bool isRight)
    {
        if (isRight)
            rightWeight += weight.weight;
        else
            leftWeight += weight.weight;
        balance.SetWeight(leftWeight, rightWeight);
        if (leftWeight == 0 || rightWeight == 0)
            inter_balance.isZero = true;
        else
            inter_balance.isZero = false;
    }

    public void SubWeight(Inter_Weight weight, bool isRight) 
    {
        if (isRight)
            rightWeight -= weight.weight;
        else
            leftWeight -= weight.weight;
        balance.SetWeight(leftWeight, rightWeight);
        if (leftWeight == 0 || rightWeight == 0)
            inter_balance.isZero = true;
        else
            inter_balance.isZero = false;
    }

    public void Incorrect()
    {
        incorrestCount += 1;
        incorrestCountReset += 1;
        if (incorrestCount == 1)
        {
            StartCoroutine(CoIncorrect());
        }
    }

    private IEnumerator CoIncorrect()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage001_Fail", null, null, "", false);
    }

    public void AddHoldCount()
    {
        holdCount += 1;
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
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage001_FirstEnter");
            if(coMusic != null)
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
        if (coDialog != null)
            StopCoroutine(coDialog);
        coDialog = CoDialog();
        StartCoroutine(coDialog);
    }

    private IEnumerator coMusic = null;

    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_New_Beginning", 0.2f, true);
    }

    private IEnumerator coDialog = null;
    private IEnumerator CoDialog()
    {
        yield return new WaitForSeconds(140.0f);
        if (isEnd)
            yield break;
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage001_Loop");
        yield return new WaitForSeconds(140.0f);
        if (isEnd)
            yield break;
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage001_Loop2");
    }

    public void Event_End()
    {
        if (isEnd)
            return;
        isEnd = true;
        if (coDialog != null)
            StopCoroutine(coDialog);
        StartCoroutine(CoEvent_End());
        StopMusic();
    }

    private IEnumerator CoEvent_End()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
        {
            if(holdCount > 9)
            {
                if(incorrestCountReset < 3)
                    DialogManager.Instance.ShowDialog("Dialog_Stage001_End");
                else
                    DialogManager.Instance.ShowDialog("Dialog_Stage001_EndLuck");
            }
            else
                DialogManager.Instance.ShowDialog("Dialog_Stage001_EndLuck");
        }
    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_1_1"));
        else
            base.Hint();
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }

}
