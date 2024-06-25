using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage008 : Stage
{
    [SerializeField] private Transform[] tiles;
    [SerializeField] private Transform[] holdTiles;
    [SerializeField] private Inter_ColorBlock[] defaultHoldObj;
    [SerializeField] private Inter_ColorBlock[] holdObj;
    [SerializeField] private Material[] mats;
    [SerializeField] private UI_Page page;
    [SerializeField] private Door door;
    [SerializeField] private List<int> answerList = new List<int>();

    [SerializeField] ColorBlock[] answerColor;

    private bool isAnswer = false;
    private bool isFirstAnswer = true;

    protected void Start()
    {
        base.Start();
        mats = new Material[tiles.Length];
        for(int i = 0; i < tiles.Length; i++)
        {
            mats[i] = tiles[i].GetComponent<MeshRenderer>().material;
        }
        SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        door.Close();
        door.Lock(true);
        SetQuestion();
        //PlayerUnit.player.transform.position = startPos.position;
    }
    public void SetQuestion()
    {
        answerColor = new ColorBlock[5];
        holdObj = new Inter_ColorBlock[10];
        
        for(int i = 0; i < defaultHoldObj.Length; i++)
        {
            answerList.Add(i);
        }
        int temp = 0;
        for (int i = 0; i < defaultHoldObj.Length; i++)
        {
            int rand = Random.Range(0, defaultHoldObj.Length);
            temp = answerList[rand];
            answerList[rand] = answerList[i];
            answerList[i] = temp;
        }
        foreach(Inter_ColorBlock block in defaultHoldObj)
        {
            block.SetColor((ColorBlock)Random.Range(1, 7));
        }
        answerColor[0] = CalColor(defaultHoldObj[answerList[0]].myColor, defaultHoldObj[answerList[1]].myColor, true);
        answerColor[1] = CalColor(defaultHoldObj[answerList[2]].myColor, defaultHoldObj[answerList[3]].myColor, false);
        answerColor[2] = CalColor(defaultHoldObj[answerList[4]].myColor, defaultHoldObj[answerList[5]].myColor, true);
        answerColor[3] = CalColor(defaultHoldObj[answerList[6]].myColor, defaultHoldObj[answerList[7]].myColor, false);
        answerColor[4] = CalColor(defaultHoldObj[answerList[8]].myColor, defaultHoldObj[answerList[9]].myColor, true);
        mats[2].SetColor("_Color", ConvertColor(answerColor[0]));
        mats[5].SetColor("_Color", ConvertColor(answerColor[1]));
        mats[8].SetColor("_Color", ConvertColor(answerColor[2]));
        mats[11].SetColor("_Color", ConvertColor(answerColor[3]));
        mats[14].SetColor("_Color", ConvertColor(answerColor[4]));

        page.Clear();
        page.SetTitle("Stage008_Color", true);
        page.AddContent("Stage008_Content_1", true);
        page.AddContent("Stage008_Content_2", true);
    }

    private void CheckAnswer()
    {
        bool isAnswer = true;

        for(int i = 0; i < holdObj.Length; i++)
        {
            if (holdObj[i] == null)
            {
                door.Close();
                door.Lock(true);
                this.isAnswer = false;
                return;
            }
        }

        

        if ((answerColor[0] == CalColor(holdObj[0].myColor, holdObj[1].myColor,true)) &&
            (answerColor[1] == CalColor(holdObj[2].myColor, holdObj[3].myColor, false)) &&
            (answerColor[2] == CalColor(holdObj[4].myColor, holdObj[5].myColor, true)) &&
            (answerColor[3] == CalColor(holdObj[6].myColor, holdObj[7].myColor, false)) &&
            (answerColor[4] == CalColor(holdObj[8].myColor, holdObj[9].myColor, true)))
        {
            isAnswer = true;
            this.isAnswer = true;
            door.Lock(false);
            door.Open();                
            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
            if(isFirstAnswer)
            {
                isFirstAnswer = false;
                StartCoroutine(CoAnswer());
                StopMusic();
            }
            this.isAnswer = true;
        }
        else
        {
            door.Close();
            door.Lock(true);
            this.isAnswer = false;
        }
        
        
    }

    private IEnumerator CoAnswer()
    {
        yield return new WaitForSeconds(0.5f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage008_End");
    }

    public void SetTile(Inter_ColorBlock holdObj)
    {
        Transform target = null;
        float max = 9999;
        float dis = 0;
        int index = -1;
        for (int i = 0; i < holdTiles.Length; i++)
        {
            dis = Vector3.Distance(holdTiles[i].position, holdObj.transform.position);
            if(dis <= 1.25f && dis < max && this.holdObj[i] == null)
            {
                max = dis;
                target = holdTiles[i];
                index = i;
            }
        }
        if(target != null)
        {
            holdObj.transform.position = new Vector3(target.position.x, holdObj.transform.position.y, target.position.z);
            if(index >= 0)
                this.holdObj[index] = holdObj;
        }
        if (index < 0)
            return;
        CheckAnswer();
        //SetTileColor(index);
        SetAllTileColor(index);
    }

    public void HoldOutObj(Inter_ColorBlock obj)
    {
        for(int i = 0; i < holdObj.Length; i++)
        {
            if(obj == holdObj[i])
            {
                holdObj[i] = null;
                CheckAnswer();
                //SetTileColor(i);
                SetAllTileColor(i);
                return;
            }
        }
    }

    //private void SetTileColor(int index)
    //{
    //    ColorBlock left = ColorBlock.Black;
    //    ColorBlock right = ColorBlock.Black;
    //    if (holdObj[0] != null)
    //        left = holdObj[0].myColor;
    //    if (holdObj[1] != null)
    //        center = holdObj[1].myColor;
    //    if (holdObj[2] != null)
    //        right = holdObj[2].myColor;
    //    SetColor(3, CalColor(left, right , true));


    //    Color gray = new Color(0.4622641f, 0.4622641f, 0.4622641f);
    //    Color orange = new Color(1.0f, 0.62f, 0.0f);
    //    Color red = new Color(1.0f, 0.04f, 0.04f);
    //    Color green = new Color(0.04f, 1.0f, 0.04f);


    //    if (index >= 0 && index <= 2)
    //    {
    //        if (holdObj[1] != null)
    //        {
    //            if (holdObj[0] != null || holdObj[2] != null) 
    //            {
    //                mats[1].SetColor("_Color", green);
    //                if (holdObj[0] != null)
    //                    mats[0].SetColor("_Color", green);
    //                else
    //                    mats[0].SetColor("_Color", gray);
    //                if (holdObj[2] != null)
    //                    mats[2].SetColor("_Color", green);
    //                else
    //                    mats[2].SetColor("_Color", gray);
    //            }
    //            else
    //            {
    //                mats[0].SetColor("_Color", gray);
    //                mats[1].SetColor("_Color", orange);
    //                mats[2].SetColor("_Color", gray);
    //            }
    //        }
    //        else
    //        {
    //            mats[1].SetColor("_Color", gray);
    //            if (holdObj[0] == null)
    //                mats[0].SetColor("_Color", gray);
    //            else
    //                mats[0].SetColor("_Color", orange);
    //            if (holdObj[2] == null)
    //                mats[2].SetColor("_Color", gray);
    //            else
    //                mats[2].SetColor("_Color", orange);
    //        }
    //    }
    //}

    private ColorBlock CalColor(ColorBlock left, ColorBlock right, bool isAdd)
    {
        if(isAdd)
        {
            //+
            return left | right;
        }
        else
        {
            //-
            if ((left & right) == ColorBlock.Black)
                return left;
            else
                return left & ~right;
        }
        //return ColorBlock.Black;
    }

    public void SetColor(int blockIndex, ColorBlock color)
    {
        Color c = Color.black;
        if (color == ColorBlock.Black)
            c = Color.black;
        else if (color == ColorBlock.Red)
            c = Color.red;
        else if (color == ColorBlock.Green)
            c = Color.green;
        else if (color == ColorBlock.Yellow)
            c = Color.yellow;
        else if (color == ColorBlock.Blue)
            c = Color.blue;
        else if (color == ColorBlock.Purple)
            c = new Color(1, 0, 1);
        else if (color == ColorBlock.Emerald)
            c = Color.cyan;
        else if (color == ColorBlock.White)
            c = Color.white;
        else
            c = Color.white;
        mats[blockIndex].SetColor("_Color", c);
    }

    private Color ConvertColor(ColorBlock color)
    {
        Color c = Color.black;
        if (color == ColorBlock.Black)
            c = Color.black;
        else if (color == ColorBlock.Red)
            c = Color.red;
        else if (color == ColorBlock.Green)
            c = Color.green;
        else if (color == ColorBlock.Yellow)
            c = Color.yellow;
        else if (color == ColorBlock.Blue)
            c = Color.blue;
        else if (color == ColorBlock.Purple)
            c = new Color(1, 0, 1);
        else if (color == ColorBlock.Emerald)
            c = Color.cyan;
        else if (color == ColorBlock.White)
            c = Color.white;
        else
            c = Color.white;
        return c;
    }

    private void SetAllTileColor(int index)
    {
        Color gray = new Color(0.4622641f, 0.4622641f, 0.4622641f);
        Color orange = new Color(1.0f, 0.62f, 0.0f);
        Color red = new Color(1.0f, 0.04f, 0.04f);
        Color green = new Color(0.04f, 1.0f, 0.04f);

        if(index == 0 || index == 1)
        {
            if (holdObj[0] != null && holdObj[1] != null)
            {
                if (answerColor[0] == CalColor(holdObj[0].myColor, holdObj[1].myColor, true))
                {
                    //answer
                    mats[0].SetColor("_Color", green);
                    mats[1].SetColor("_Color", green);
                    if (!isAnswer)
                        SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                }
                else
                {
                    //no answer
                    mats[0].SetColor("_Color", red);
                    mats[1].SetColor("_Color", red);
                    SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                if (holdObj[0] == null)
                    mats[0].SetColor("_Color", gray);
                else
                    mats[0].SetColor("_Color", orange);
                if (holdObj[1] == null)
                    mats[1].SetColor("_Color", gray);
                else
                    mats[1].SetColor("_Color", orange);
            }
        }
        else if (index == 2 || index == 3)
        {
            if (holdObj[2] != null && holdObj[3] != null)
            {
                if (answerColor[1] == CalColor(holdObj[2].myColor, holdObj[3].myColor, false))
                {
                    //answer
                    mats[3].SetColor("_Color", green);
                    mats[4].SetColor("_Color", green);
                    if (!isAnswer)
                        SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                }
                else
                {
                    //no answer
                    mats[3].SetColor("_Color", red);
                    mats[4].SetColor("_Color", red);
                    SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                if (holdObj[2] == null)
                    mats[3].SetColor("_Color", gray);
                else
                    mats[3].SetColor("_Color", orange);
                if (holdObj[3] == null)
                    mats[4].SetColor("_Color", gray);
                else
                    mats[4].SetColor("_Color", orange);
            }
        }
        else if (index == 4 || index == 5)
        {
            if (holdObj[4] != null && holdObj[5] != null)
            {
                if (answerColor[2] == CalColor(holdObj[4].myColor, holdObj[5].myColor, true))
                {
                    //answer
                    mats[6].SetColor("_Color", green);
                    mats[7].SetColor("_Color", green);
                    if (!isAnswer)
                        SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                }
                else
                {
                    //no answer
                    mats[6].SetColor("_Color", red);
                    mats[7].SetColor("_Color", red);
                    SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                if (holdObj[4] == null)
                    mats[6].SetColor("_Color", gray);
                else
                    mats[6].SetColor("_Color", orange);
                if (holdObj[5] == null)
                    mats[7].SetColor("_Color", gray);
                else
                    mats[7].SetColor("_Color", orange);
            }
        }
        else if (index == 6 || index == 7)
        {
            if (holdObj[6] != null && holdObj[7] != null)
            {
                if (answerColor[3] == CalColor(holdObj[6].myColor, holdObj[7].myColor, false))
                {
                    //answer
                    mats[9].SetColor("_Color", green);
                    mats[10].SetColor("_Color", green);
                    if (!isAnswer)
                        SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                }
                else
                {
                    //no answer
                    mats[9].SetColor("_Color", red);
                    mats[10].SetColor("_Color", red);
                    SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                if (holdObj[6] == null)
                    mats[9].SetColor("_Color", gray);
                else
                    mats[9].SetColor("_Color", orange);
                if (holdObj[7] == null)
                    mats[10].SetColor("_Color", gray);
                else
                    mats[10].SetColor("_Color", orange);
            }
        }
        else if (index == 8 || index == 9)
        {
            if (holdObj[8] != null && holdObj[9] != null)
            {
                if (answerColor[4] == CalColor(holdObj[8].myColor, holdObj[9].myColor, true))
                {
                    //answer
                    mats[12].SetColor("_Color", green);
                    mats[13].SetColor("_Color", green);
                    if (!isAnswer)
                        SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                }
                else
                {
                    //no answer
                    mats[12].SetColor("_Color", red);
                    mats[13].SetColor("_Color", red);
                    SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                if (holdObj[8] == null)
                    mats[12].SetColor("_Color", gray);
                else
                    mats[12].SetColor("_Color", orange);
                if (holdObj[9] == null)
                    mats[13].SetColor("_Color", gray);
                else
                    mats[13].SetColor("_Color", orange);
            }
        }
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
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage008_FirstEnter");
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

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_8_1"));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Special_Moments_Loop", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
