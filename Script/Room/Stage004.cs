using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage004 : Stage
{
    [SerializeField] private MeshRenderer[] renders;
    [SerializeField] private Material[] mats;
    [SerializeField] private Texture2D[] textures;
    private Transform[,] pictures;
    [SerializeField] private Door door;
    private bool isReady = false;
    [SerializeField] private UI_Page page;

    [SerializeField] private List<int> answerList = new List<int>();

    [SerializeField] private Renderer[] arrowRender;
    [SerializeField] private Material[] arrowMat;

    private bool isFirstAnswer = true;
    private bool isStart = false;

    protected void Start()
    {
        base.Start();
        mats = new Material[renders.Length];
        for(int i = 0; i < renders.Length; i++)
        {
            mats[i] = renders[i].material;
        }
        arrowMat = new Material[arrowRender.Length];
        for(int i = 0; i< arrowRender.Length; i++)
        {
            arrowMat[i] = arrowRender[i].material;
        }
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[0];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x].SetTexture("_MainTex", puzzleTexture);
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[1];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x + 3].SetTexture("_MainTex", puzzleTexture);
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[2];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x + 18].SetTexture("_MainTex", puzzleTexture);
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[3];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x + 21].SetTexture("_MainTex", puzzleTexture);
            }
        }
        page.Clear();
        page.SetTitle("Stage004_Picture", true);
        page.AddContent("Stage004_Content_1", true);
        //SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }
    public void SetQuestion()
    {
        isStart = true;
        isReady = false;
        pictures = new Transform[6, 6];
        for(int x = 0; x < 6; x++)
        {
            for(int y = 0; y < 6; y++)
            {
                pictures[x, y] = renders[y * 6 + x].transform;
            }
        }

        List<int> list = new List<int>();
        for(int i = 0; i < textures.Length; i++)
        {
            list.Add(i);
        }

        //Random
        //int temp = 0;
        //for (int i = 0; i < textures.Length; i++)
        //{
        //    int rand = Random.Range(0, textures.Length);
        //    temp = list[rand];
        //    list[rand] = list[i];
        //    list[i] = temp;
        //}

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[list[0]];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x].SetTexture("_MainTex", puzzleTexture);
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[list[1]];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x + 3].SetTexture("_MainTex", puzzleTexture);
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[list[2]];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x + 18].SetTexture("_MainTex", puzzleTexture);
            }
        }

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Texture2D answer = textures[list[3]];
                int width = (int)(answer.width / 3);
                int height = (int)(answer.height / 3);
                int startWidth = width * x;
                int startHeight = height * y;

                Texture2D puzzleTexture = new Texture2D(width, height);
                puzzleTexture.filterMode = FilterMode.Bilinear;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        puzzleTexture.SetPixel(x1, y1, answer.GetPixel(startWidth + x1, startHeight + y1));
                    }
                }
                puzzleTexture.Apply();
                mats[y * 6 + x + 21].SetTexture("_MainTex", puzzleTexture);
            }
        }

        answerList.Clear();
        for(int i = 0; i < 8; i++)
        {
            int rand = Random.Range(0, 8);
            Swt(rand);
        }
        
        for(int i = 0; i < 5; i++)
        {
            if (answerList.Count < 6)
            {
                int rand = Random.Range(0, 8);
                if (rand == answerList[^1])
                {
                    rand += 1;
                    rand = rand % 8;
                }
                Swt(rand);
            }
            else
            {
                break;
            }
        }
        isReady = true;
    }

    public void Swt(int index)
    {
        if (!isStart)
            return;
        Transform trans;
        Vector3 vec;
        if (index == 0)
        {
            for(int i = 0; i < 3; i++)
            {
                trans = pictures[i, 0];
                vec = pictures[i, 0].position;
                //pictures[i, 0].DOMove(pictures[i + 3, 0].position, 0.5f);
                pictures[i, 0].position = pictures[i+3, 0].position;
                //pictures[i + 3, 0].DOMove(vec, 0.5f);
                pictures[i+3, 0].position = vec;
                pictures[i, 0] = pictures[i+3, 0];
                pictures[i+3, 0] = trans;
            }
        }
        else if (index == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                trans = pictures[i, 2];
                vec = pictures[i, 2].position;
                pictures[i, 2].position = pictures[i + 3, 2].position;
                pictures[i + 3, 2].position = vec;
                pictures[i, 2] = pictures[i + 3, 2];
                pictures[i + 3, 2] = trans;
            }
        }
        else if (index == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                trans = pictures[i, 3];
                vec = pictures[i, 3].position;
                pictures[i, 3].position = pictures[i + 3, 3].position;
                pictures[i + 3, 3].position = vec;
                pictures[i, 3] = pictures[i + 3, 3];
                pictures[i + 3, 3] = trans;
            }
        }
        else if (index == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                trans = pictures[i, 5];
                vec = pictures[i, 5].position;
                pictures[i, 5].position = pictures[i + 3, 5].position;
                pictures[i + 3, 5].position = vec;
                pictures[i, 5] = pictures[i + 3, 5];
                pictures[i + 3, 5] = trans;
            }
        }
        else if (index == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                trans = pictures[0, i];
                vec = pictures[0, i].position;
                pictures[0, i].position = pictures[0, i + 3].position;
                pictures[0, i + 3].position = vec;
                pictures[0, i] = pictures[0, i + 3];
                pictures[0, i + 3] = trans;
            }
        }
        else if (index == 5)
        {
            for (int i = 0; i < 3; i++)
            {
                trans = pictures[2, i];
                vec = pictures[2, i].position;
                pictures[2, i].position = pictures[2, i + 3].position;
                pictures[2, i + 3].position = vec;
                pictures[2, i] = pictures[2, i + 3];
                pictures[2, i + 3] = trans;
            }
        }
        else if (index == 6)
        {
            for (int i = 0; i < 3; i++)
            {
                trans = pictures[3, i];
                vec = pictures[3, i].position;
                pictures[3, i].position = pictures[3, i + 3].position;
                pictures[3, i + 3].position = vec;
                pictures[3, i] = pictures[3, i + 3];
                pictures[3, i + 3] = trans;
            }
        }
        else if (index == 7)
        {
            for (int i = 0; i < 3; i++)
            {
                trans = pictures[5, i];
                vec = pictures[5, i].position;
                pictures[5, i].position = pictures[5, i + 3].position;
                pictures[5, i + 3].position = vec;
                pictures[5, i] = pictures[5, i + 3];
                pictures[5, i + 3] = trans;
            }
        }
        if(isReady)
        {
            SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
            CheckAnswer();
        }

        if (answerList.Count > 0 && index == answerList[answerList.Count - 1])
            answerList.RemoveAt(answerList.Count-1);
        else
            answerList.Add(index);
    }


    public void CheckAnswer()
    {
        if (!isStart)
            return;
        bool answer = true;
        for(int x = 0; x < 6; x++)
        {
            for(int y = 0; y < 6; y++)
            {
                if (pictures[x, y] != renders[y * 6 + x].transform)
                {
                    answer = false;
                }
            }
        }
        door.Lock(!answer);
        if(answer)
        {
            door.Open();
            if(isFirstAnswer)
            {
                isFirstAnswer = false;
                StartCoroutine(CoAnswer());
            }
            SoundManager.Instance.StartSound("UI_Success2", 1.0f);
        }
        else
        {
            door.Close();
        }
    }

    private IEnumerator CoAnswer()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
        {
            DialogManager.Instance.ShowDialog("Dialog_Stage004_End");
        }
        StopMusic();
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.2f);
        StartCoroutine(coMusic);
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
        {
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage004_FirstEnter");
            StartCoroutine(CoSetQuestion(t + 1.0f));
        }
        else
        {
            StartCoroutine(CoSetQuestion(3.0f));
        }
    }

    private IEnumerator CoSetQuestion(float t)
    {
        yield return new WaitForSeconds(t);
        SetQuestion();
        SoundManager.Instance.StartSound("UI_Button_Click4", 1.0f);
        yield return new WaitForSeconds(1.0f);
        DialogManager.Instance.ShowDialog("Dialog_Stage004_Start");
    }

    public override void Hint()
    {
        if (hintIndex == 0)
        {
            if (answerList.Count > 3)
            {
                DisplayHint();
                UIManager.Toast(Language.Instance.Get("Hint_4_1"));
            }
            else
            {
                UIManager.Toast(Language.Instance.Get("Hint_4_2", answerList.Count.ToString()));
            }
            
        }
        else
            base.Hint();
    }

    private void DisplayHint()
    {
        if (answerList.Count == 0)
            return;
        Color c = arrowMat[answerList[^1]].GetColor("_Color");
        Material mat1 = arrowMat[answerList[^1] * 2];
        mat1.DOColor(Color.green, "_Color", 1.0f).OnComplete(()=>
        {
            StartCoroutine(CoDisplayHint1(mat1, c));
        });
        Material mat2 = arrowMat[answerList[^1] * 2 + 1];
        mat2.DOColor(Color.green, "_Color", 1.0f).OnComplete(() =>
        {
            StartCoroutine(CoDisplayHint2(mat2, c));
        });
        //arrowMat[answerList[^1]*2].SetColor("_Color", Color.green);
        //arrowMat[answerList[^1]*2+1].SetColor("_Color", Color.green);
        //yield return new WaitForSeconds(1.5f);
        //arrowMat[answerList[^1] * 2].SetColor("_Color", c);
        //arrowMat[answerList[^1] * 2 + 1].SetColor("_Color", c);
    }

    private IEnumerator CoDisplayHint1(Material mat, Color c)
    {
        yield return new WaitForSeconds(1.0f);
        mat.DOColor(c, "_Color", 1.0f).OnComplete(() =>
        {
            mat.SetColor("_Color", c);
        });
    }

    private IEnumerator CoDisplayHint2(Material mat, Color c)
    {
        yield return new WaitForSeconds(1.0f);
        mat.DOColor(c, "_Color", 1.0f);
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Ultimate_Happiness", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
