using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Stage011 : Stage
{
    [SerializeField] private Transform[] holdTiles;
    [SerializeField] private Inter_NumberBlock[] defaultBlocks;
    [SerializeField] private Inter_NumberBlock[] holdBlocks;
    [SerializeField] private TextMeshPro[] answerText;
    [SerializeField] private int[] answer;
    [SerializeField] private int[,] answerList;
    [SerializeField] private Door door;
    [SerializeField] private UI_Page page;

    [SerializeField] private MeshRenderer[] tilesRender;
    private Material[] tilesMat;
    private Color defaultColor;
    private bool[] isAnswers = new bool[5];

    private bool isFirstAnswer = true;
    private float enterTime = 0;
    protected void Start()
    {
        base.Start();
        tilesMat = new Material[tilesRender.Length];
        for(int i = 0; i < tilesRender.Length; i++)
            tilesMat[i] = tilesRender[i].material;
        if (tilesMat[0] != null)
            defaultColor = tilesMat[0].GetColor("_Color");
        SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }
    public void SetQuestion()
    {
        holdBlocks = new Inter_NumberBlock[holdTiles.Length];
        answer = new int[defaultBlocks.Length];
        for(int i = 0; i < defaultBlocks.Length; i++)
        {
            answer[i] = i;
            defaultBlocks[i].SetNumber(i);
        }
        for (int i = 0; i < isAnswers.Length; i++)
            isAnswers[i] = false;

        int temp = 0;
        for(int i = 0; i < answer.Length; i++)
        {
            int rand = Random.Range(0, answer.Length);
            temp = answer[i];
            answer[i] = answer[rand];
            answer[rand] = temp;
        }

        if (answer[3] > answer[2])
        {
            temp = answer[3];
            answer[3] = answer[2];
            answer[2] = temp;
        }

        if (answer[4] == 0)
        {
            temp = answer[4];
            answer[4] = answer[0];
            answer[0] = temp;
        }
        if (answer[5] == 0)
        {
            temp = answer[5];
            answer[5] = answer[1];
            answer[1] = temp;
        }

        if (answer[7] == 0)
        {
            temp = answer[7];
            answer[7] = answer[0];
            answer[0] = temp;
        }
        if (answer[9] == 0) 
        {
            temp = answer[9];
            answer[9] = answer[1];
            answer[1] = temp;
        }
        answerText[0].text = (answer[0] + answer[1]).ToString();
        answerText[1].text = (answer[2] - answer[3]).ToString();
        answerText[2].text = (answer[4] * answer[5]).ToString();
        answerText[3].text = ((int)(answer[6] / answer[7])).ToString();
        answerText[4].text = (answer[8] % answer[9]).ToString();

        page.Clear();
        page.SetTitle("Stage011_Formula", true);
        page.AddContent("Stage011_Content_1", true);
        page.AddContent("Stage011_Content_2", true);
        page.AddContent("Stage011_Content_3", true);
    }

    public void SetTile(Inter_NumberBlock holdBlock, int forceIndex = -1)
    {
        if (forceIndex < 0)
        {
            Transform target = null;
            float max = 9999;
            float dis = 0;
            int index = 0;
            for (int i = 0; i < holdTiles.Length; i++)
            {
                dis = Vector3.Distance(holdTiles[i].position, holdBlock.transform.position);
                if (dis <= 1.25f && dis < max && this.holdBlocks[i] == null)
                {
                    max = dis;
                    target = holdTiles[i];
                    index = i;
                }
            }
            if (target != null)
            {
                holdBlock.transform.position = new Vector3(target.position.x, holdBlock.transform.position.y, target.position.z);
                this.holdBlocks[index] = holdBlock;
                SetTileColor(index, true);
            }
        }
        else
        {
            Transform target = holdTiles[forceIndex];
            holdBlock.transform.position = new Vector3(target.position.x, holdBlock.transform.position.y, target.position.z);
            this.holdBlocks[forceIndex] = holdBlock;
            SetTileColor(forceIndex, true);
        }

        CheckAnswer();
        
    }

    public void HoldOutObj(Inter_NumberBlock obj)
    {
        for (int i = 0; i < holdBlocks.Length; i++)
        {
            if (obj == holdBlocks[i])
            {
                holdBlocks[i] = null;
                CheckAnswer();
                SetTileColor(i, false);
                return;
            }
        }
    }

    private void CheckAnswer()
    {
        foreach(Inter_NumberBlock block in this.holdBlocks)
        {
            if(block == null)
            {
                door.Close();
                door.Lock(true);
                return;
            }
        }

        if (holdBlocks[0].number + holdBlocks[1].number != answer[0] + answer[1])
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (holdBlocks[2].number - holdBlocks[3].number != answer[2] - answer[3])
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (holdBlocks[4].number * holdBlocks[5].number != answer[4] * answer[5])
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (holdBlocks[7].number != 0 && (int)(holdBlocks[6].number / holdBlocks[7].number) != (int)(answer[6] / answer[7]))
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (holdBlocks[9].number != 0 && (int)(holdBlocks[8].number % holdBlocks[9].number) != (int)(answer[8] % answer[9]))
        {
            door.Close();
            door.Lock(true);
            return;
        }

        door.Open();
        door.Lock(false);
        if(isFirstAnswer)
        {
            isFirstAnswer = false;
            StartCoroutine(CoFirstAnswer());
        }
        
    }

    private IEnumerator CoFirstAnswer()
    {
        StopMusic();
        yield return new WaitForSeconds(0.5f);
        if (DialogManager.Exist())
        {
            if(Time.time - enterTime <= 120.0f)
                DialogManager.Instance.ShowDialog("Dialog_Stage011_End_1");
            else
                DialogManager.Instance.ShowDialog("Dialog_Stage011_End_2");
        }
            
    }

    private void SetTileColor(int index, bool isOn)
    {
        Color gray = new Color(0.4622641f, 0.4622641f, 0.4622641f);
        Color orange = new Color(1.0f, 0.62f, 0.0f);
        Color red = new Color(1.0f, 0.04f, 0.04f);
        Color green = new Color(0.04f, 1.0f, 0.04f);

        if(index == 0 || index == 1)
        {
            if (holdBlocks[0] != null && holdBlocks[1] != null)
            {
                if (holdBlocks[0].number + holdBlocks[1].number == answer[0] + answer[1])
                {
                    tilesMat[0].SetColor("_Color", green);
                    tilesMat[1].SetColor("_Color", green);
                    if (isOn)
                    {
                        if (isAnswers[0] && isAnswers[1] && isAnswers[2] && isAnswers[3] && isAnswers[4])
                            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
                        else
                        if (!isAnswers[0])
                            SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                    }
                    isAnswers[0] = true;
                }
                else
                {
                    tilesMat[0].SetColor("_Color", red);
                    tilesMat[1].SetColor("_Color", red);
                    isAnswers[0] = false;
                    if(isOn)
                        SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                isAnswers[0] = false;
                if (holdBlocks[0] == null)
                    tilesMat[0].SetColor("_Color", gray);
                else
                    tilesMat[0].SetColor("_Color", orange);

                if (holdBlocks[1] == null)
                    tilesMat[1].SetColor("_Color", gray);
                else
                    tilesMat[1].SetColor("_Color", orange);
            }
        }
        else if(index == 2 || index == 3)
        {
            if (holdBlocks[2] != null && holdBlocks[3] != null)
            {
                if (holdBlocks[2].number - holdBlocks[3].number == answer[2] - answer[3])
                {
                    tilesMat[2].SetColor("_Color", green);
                    tilesMat[3].SetColor("_Color", green);
                    if (isOn)
                    {
                        if (isAnswers[0] && isAnswers[1] && isAnswers[2] && isAnswers[3] && isAnswers[4])
                            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
                        else
                        if (!isAnswers[1])
                            SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                    }

                    isAnswers[1] = true;
                }
                else
                {
                    tilesMat[2].SetColor("_Color", red);
                    tilesMat[3].SetColor("_Color", red);
                    isAnswers[1] = false;
                    if (isOn)
                        SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                isAnswers[1] = false;
                if (holdBlocks[2] == null)
                    tilesMat[2].SetColor("_Color", gray);
                else
                    tilesMat[2].SetColor("_Color", orange);

                if (holdBlocks[3] == null)
                    tilesMat[3].SetColor("_Color", gray);
                else
                    tilesMat[3].SetColor("_Color", orange);
            }
        }
        else if (index == 4 || index == 5)
        {
            if (holdBlocks[4] != null && holdBlocks[5] != null)
            {
                if (holdBlocks[4].number * holdBlocks[5].number == answer[4] * answer[5])
                {
                    tilesMat[4].SetColor("_Color", green);
                    tilesMat[5].SetColor("_Color", green);
                    if (isOn)
                    {
                        if (isAnswers[0] && isAnswers[1] && isAnswers[2] && isAnswers[3] && isAnswers[4])
                            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
                        else
                        if (!isAnswers[2])
                            SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                    }

                    isAnswers[2] = true;
                }
                else
                {
                    tilesMat[4].SetColor("_Color", red);
                    tilesMat[5].SetColor("_Color", red);
                    isAnswers[2] = false;
                    if (isOn)
                        SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                isAnswers[2] = false;
                if (holdBlocks[4] == null)
                    tilesMat[4].SetColor("_Color", gray);
                else
                    tilesMat[4].SetColor("_Color", orange);

                if (holdBlocks[5] == null)
                    tilesMat[5].SetColor("_Color", gray);
                else
                    tilesMat[5].SetColor("_Color", orange);
            }
        }
        else if (index == 6 || index == 7)
        {
            if (holdBlocks[6] != null && holdBlocks[7] != null)
            {
                if (holdBlocks[7].number != 0 && holdBlocks[6].number / holdBlocks[7].number == answer[6] / answer[7])
                {
                    tilesMat[6].SetColor("_Color", green);
                    tilesMat[7].SetColor("_Color", green);
                    if (isOn)
                    {
                        if (isAnswers[0] && isAnswers[1] && isAnswers[2] && isAnswers[3] && isAnswers[4])
                            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
                        else
                        if (!isAnswers[3])
                            SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                    }

                    isAnswers[3] = true;
                }
                else
                {
                    tilesMat[6].SetColor("_Color", red);
                    tilesMat[7].SetColor("_Color", red);
                    isAnswers[3] = false;
                    if (isOn)
                        SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                isAnswers[3] = false;
                if (holdBlocks[6] == null)
                    tilesMat[6].SetColor("_Color", gray);
                else
                    tilesMat[6].SetColor("_Color", orange);

                if (holdBlocks[7] == null)
                    tilesMat[7].SetColor("_Color", gray);
                else
                    tilesMat[7].SetColor("_Color", orange);
            }
        }
        else if (index == 8 || index == 9)
        {
            if (holdBlocks[8] != null && holdBlocks[9] != null)
            {
                if (holdBlocks[9].number != 0 && holdBlocks[8].number % holdBlocks[9].number == answer[8] % answer[9])
                {
                    tilesMat[8].SetColor("_Color", green);
                    tilesMat[9].SetColor("_Color", green);
                    if (isOn)
                    {
                        if (isAnswers[0] && isAnswers[1] && isAnswers[2] && isAnswers[3] && isAnswers[4])
                            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
                        else
                        if (!isAnswers[4])
                            SoundManager.Instance.StartSound("UI_Success2", 1.0f);
                    }

                    isAnswers[4] = true;
                }
                else
                {
                    tilesMat[8].SetColor("_Color", red);
                    tilesMat[9].SetColor("_Color", red);
                    isAnswers[4] = false;
                    if (isOn)
                        SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
                }
            }
            else
            {
                isAnswers[4] = false;
                if (holdBlocks[8] == null)
                    tilesMat[8].SetColor("_Color", gray);
                else
                    tilesMat[8].SetColor("_Color", orange);

                if (holdBlocks[9] == null)
                    tilesMat[9].SetColor("_Color", gray);
                else
                    tilesMat[9].SetColor("_Color", orange);
            }
        }
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
        enterTime = Time.time;
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
        {
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage011_FirstEnter");
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
            UIManager.Toast(Language.Instance.Get("Hint_11_1", answer[0].ToString()));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_The_Hope_full", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
