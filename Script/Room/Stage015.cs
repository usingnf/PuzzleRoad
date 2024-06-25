using System;
using System.Collections;
using System.Data;
using TMPro;
using UnityEngine;

public class Stage015 : Stage
{
    public enum Operator
    {
        plus = 0,
        minus = 1,
        multi = 2,
        divide = 3,
        equal = 4,
    }

    [SerializeField] private UI_Page page;
    [SerializeField] private Door door;

    [SerializeField] private TextMeshPro[] text1;
    [SerializeField] private TextMeshPro[] text2;
    [SerializeField] private TextMeshPro[] text3;
    [SerializeField] private TextMeshPro[] text4;
    [SerializeField] private TextMeshPro[] text5;
    [SerializeField] private TextMeshPro[] text6;

    [SerializeField] private TextMeshPro[] textOper1;
    [SerializeField] private TextMeshPro[] textOper2;
    [SerializeField] private TextMeshPro[] textOper3;
    [SerializeField] private TextMeshPro[] textOper4;
    [SerializeField] private TextMeshPro[] textOper5;

    [SerializeField] private TextMeshPro textResult;

    private int[] numbers1;
    private int[] numbers2;
    private int[] numbers3;
    private int[] numbers4;
    private int[] numbers5;
    private int[] numbers6;

    private Operator[] operators1;
    private Operator[] operators2;
    private Operator[] operators3;
    private Operator[] operators4;
    private Operator[] operators5;

    private int index1;
    private int index2;
    private int index3;
    private int index4;
    private int index5;
    private int index6;
    private int indexOper1;
    private int indexOper2;
    private int indexOper3;
    private int indexOper4;
    private int indexOper5;

    private bool isFirstAnswer = true;
    private bool isFirstIncorrect = true;
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
        numbers1 = new int[] { 3, 4, 7, 8, 9 };
        numbers2 = new int[] { 4, 6, 7, 4, 1 };
        numbers3 = new int[] { 9, 5, 2, 2, 6 };
        numbers4 = new int[] { 7, 9, 8, 2, 6 };
        numbers5 = new int[] { 1, 5, 2, 5, 7 };
        numbers6 = new int[] { 3, 5, 3, 4, 6 };
        operators1 = new Operator[] { Operator.multi, Operator.plus };
        operators2 = new Operator[] { Operator.plus, Operator.multi };
        operators3 = new Operator[] { Operator.equal };
        operators4 = new Operator[] { Operator.multi, Operator.plus };
        operators5 = new Operator[] { Operator.minus, Operator.multi };
        for(int i = 0; i < numbers1.Length; i++)
            text1[i].text = numbers1[i].ToString();
        for (int i = 0; i < numbers2.Length; i++)
            text2[i].text = numbers2[i].ToString();
        for (int i = 0; i < numbers3.Length; i++)
            text3[i].text = numbers3[i].ToString();
        for (int i = 0; i < numbers4.Length; i++)
            text4[i].text = numbers4[i].ToString();
        for (int i = 0; i < numbers5.Length; i++)
            text5[i].text = numbers5[i].ToString();
        for (int i = 0; i < numbers6.Length; i++)
            text6[i].text = numbers6[i].ToString();
        ResetQuestion();


        page.Clear();
        page.SetTitle("Stage015_Train", true);
        page.AddContent("Stage015_Content_1", true);
        page.AddContent("Stage015_Content_2", true);
        page.AddContent("Stage015_Content_3", true);
        page.AddContent("Stage015_Content_4", true);
    }

    public void ResetQuestion()
    {
        index1 = -1;
        index2 = -1;
        index3 = -1;
        index4 = -1;
        index5 = -1;
        index6 = -1;
        indexOper1 = -1;
        indexOper2 = -1;
        indexOper3 = -1;
        indexOper4 = -1;
        indexOper5 = -1;
        for (int i = 0; i < numbers1.Length; i++)
            text1[i].color = Color.black;
        for (int i = 0; i < numbers2.Length; i++)
            text2[i].color = Color.black;
        for (int i = 0; i < numbers3.Length; i++)
            text3[i].color = Color.black;
        for (int i = 0; i < numbers4.Length; i++)
            text4[i].color = Color.black;
        for (int i = 0; i < numbers5.Length; i++)
            text5[i].color = Color.black;
        for (int i = 0; i < numbers6.Length; i++)
            text6[i].color = Color.black;
        for (int i = 0; i < operators1.Length; i++)
            textOper1[i].color = Color.black;
        for (int i = 0; i < operators2.Length; i++)
            textOper2[i].color = Color.black;
        for (int i = 0; i < operators3.Length; i++)
            textOper3[i].color = Color.black;
        for (int i = 0; i < operators4.Length; i++)
            textOper4[i].color = Color.black;
        for (int i = 0; i < operators5.Length; i++)
            textOper5[i].color = Color.black;
        textResult.text = "";

    }

    public void ResetQuestionEnd()
    {
        ResetQuestion();
        PlayerUnit.player.SetPos(startPos.position);
    }

    public void CheckAnswer()
    {
        int answer1 = 0;
        int answer2 = 0;

        string s = textResult.text.Replace("x", "*");
        string[] str = s.Split("=");
        DataTable data = new DataTable();
        answer1 = Convert.ToInt32(data.Compute(str[0], ""));
        answer2 = Convert.ToInt32(data.Compute(str[1], ""));
        if(answer1 == answer2)
        {
            door.Lock(false);
            door.Open();
            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
            if(isFirstAnswer)
            {
                isFirstAnswer = false;
                StopMusic();
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Stage015_End");
            }
        }
        else
        {
            door.Close();
            door.Lock(true);
            SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
            if(isFirstIncorrect && isFirstAnswer)
            {
                isFirstIncorrect = false;
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Stage015_Incorrect");
            }
        }
    }

    public void Enter_Number1(int index)
    {
        if (index1 >= 0)
            return;

        for (int i = 0; i < numbers1.Length; i++)
            text1[i].color = Color.black;
        if (index >= 0)
            text1[index].color = Color.red;
        index1 = index;

        textResult.text = numbers1[index].ToString();
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Number2(int index)
    {
        if (index2 >= 0)
            return;
        if (indexOper1 < 0)
            return;
        if (index <= 1 && indexOper1 == 1)
            return;
        if (index >= 2 && indexOper1 == 0)
            return;

        for (int i = 0; i < numbers2.Length; i++)
            text2[i].color = Color.black;
        if (index >= 0)
            text2[index].color = Color.red;
        index2 = index;

        textResult.text += numbers2[index].ToString();
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Number3(int index)
    {
        if (index3 >= 0)
            return;
        if (indexOper2 < 0)
            return;
        if (index <= 2 && indexOper2 == 1)
            return;
        if (index >= 3 && indexOper2 == 0)
            return;

        for (int i = 0; i < numbers3.Length; i++)
            text3[i].color = Color.black;
        if (index >= 0)
            text3[index].color = Color.red;
        index3 = index;

        textResult.text += numbers3[index].ToString();
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Number4(int index)
    {
        if (index4 >= 0)
            return;
        if (indexOper3 != 0)
            return;
        if (indexOper3 < 0)
            return;

        for (int i = 0; i < numbers4.Length; i++)
            text4[i].color = Color.black;
        if (index >= 0)
            text4[index].color = Color.red;
        index4 = index;

        textResult.text += numbers4[index].ToString();
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Number5(int index)
    {
        if (index5 >= 0)
            return;
        if (indexOper4 < 0)
            return;
        if (index <= 2 && indexOper4 == 1)
            return;
        if (index >= 3 && indexOper4 == 0)
            return;

        for (int i = 0; i < numbers5.Length; i++)
            text5[i].color = Color.black;
        if (index >= 0)
            text5[index].color = Color.red;
        index5 = index;

        textResult.text += numbers5[index].ToString();
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Number6(int index)
    {
        if (index6 >= 0)
            return;
        if (indexOper5 < 0)
            return;
        if (index <= 1 && indexOper5 == 1)
            return;
        if (index >= 2 && indexOper5 == 0)
            return;

        for (int i = 0; i < numbers6.Length; i++)
            text6[i].color = Color.black;
        if (index >= 0)
            text6[index].color = Color.red;
        index6 = index;
        textResult.text += numbers6[index].ToString();
        CheckAnswer();
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Operator1(int index)
    {
        if (indexOper1 >= 0)
            return;
        if (index1 < 0)
            return;
        if (index == 0 && index1 >= 2)
            return;
        if (index == 1 && index1 <= 1)
            return;

        for (int i = 0; i < operators1.Length; i++)
            textOper1[i].color = Color.black;
        if (index >= 0)
            textOper1[index].color = Color.red;
        indexOper1 = index;

        if(operators1[index] == Operator.plus)
            textResult.text += "+";
        else if (operators1[index] == Operator.minus)
            textResult.text += "-";
        else if(operators1[index] == Operator.multi)
            textResult.text += "x";
        else if(operators1[index] == Operator.divide)
            textResult.text += "/";
        else if (operators1[index] == Operator.equal)
            textResult.text += "=";

        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Operator2(int index)
    {
        if (indexOper2 >= 0)
            return;
        if (index2 < 0)
            return;
        if (index == 0 && index2 >= 3)
            return;
        if (index == 1 && index2 <= 2)
            return;
        for (int i = 0; i < operators2.Length; i++)
            textOper2[i].color = Color.black;
        if (index >= 0)
            textOper2[index].color = Color.red;
        indexOper2 = index;

        if (operators2[index] == Operator.plus)
            textResult.text += "+";
        else if (operators2[index] == Operator.minus)
            textResult.text += "-";
        else if (operators2[index] == Operator.multi)
            textResult.text += "x";
        else if (operators2[index] == Operator.divide)
            textResult.text += "/";
        else if (operators2[index] == Operator.equal)
            textResult.text += "=";
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Operator3(int index)
    {
        if (indexOper3 >= 0)
            return;
        if (index3 < 0)
            return;

        for (int i = 0; i < operators3.Length; i++)
            textOper3[i].color = Color.black;
        if (index >= 0)
            textOper3[index].color = Color.red;
        indexOper3 = index;
        if (operators3[index] == Operator.plus)
            textResult.text += "+";
        else if (operators3[index] == Operator.minus)
            textResult.text += "-";
        else if (operators3[index] == Operator.multi)
            textResult.text += "x";
        else if (operators3[index] == Operator.divide)
            textResult.text += "/";
        else if (operators3[index] == Operator.equal)
            textResult.text += "=";

        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Operator4(int index)
    {
        if (indexOper4 >= 0)
            return;
        if (index4 < 0)
            return;
        if (index == 0 && index4 >= 3)
            return;
        if (index == 1 && index4 <= 2)
            return;

        for (int i = 0; i < operators4.Length; i++)
            textOper4[i].color = Color.black;
        if (index >= 0)
            textOper4[index].color = Color.red;
        indexOper4 = index;
        if (operators4[index] == Operator.plus)
            textResult.text += "+";
        else if (operators4[index] == Operator.minus)
            textResult.text += "-";
        else if (operators4[index] == Operator.multi)
            textResult.text += "x";
        else if (operators4[index] == Operator.divide)
            textResult.text += "/";
        else if (operators4[index] == Operator.equal)
            textResult.text += "=";
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
    }

    public void Enter_Operator5(int index)
    {
        if (indexOper5 >= 0)
            return;
        if (index5 < 0)
            return;
        if (index == 0 && index5 >= 2)
            return;
        if (index == 1 && index5 <= 1)
            return;
        for (int i = 0; i < operators5.Length; i++)
            textOper5[i].color = Color.black;
        if(index >= 0)
            textOper5[index].color = Color.red;
        indexOper5 = index;
        if (operators5[index] == Operator.plus)
            textResult.text += "+";
        else if (operators5[index] == Operator.minus)
            textResult.text += "-";
        else if (operators5[index] == Operator.multi)
            textResult.text += "x";
        else if (operators5[index] == Operator.divide)
            textResult.text += "/";
        else if (operators5[index] == Operator.equal)
            textResult.text += "=";
        SoundManager.Instance.StartSound("SE_Tile_Signal", 1.0f);
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
            DialogManager.Instance.ShowDialog("Dialog_Stage015_FirstEnter");
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Happy_Upbeat_Pop_Loop", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
