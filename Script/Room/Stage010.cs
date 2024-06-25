using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage010 : Stage
{
    private int[,,] numbers = new int[10, 5, 5]
        {
            { 
                { 1,1,1,1,1},
                { 1,0,0,0,1},
                { 1,0,0,0,1},
                { 1,0,0,0,1},
                { 1,1,1,1,1}
            },
            {
                { 0,0,1,0,0},
                { 0,1,1,0,0},
                { 1,0,1,0,0},
                { 0,0,1,0,0},
                { 1,1,1,1,1}
            },
            {
                { 1,1,1,1,1},
                { 0,0,0,0,1},
                { 1,1,1,1,1},
                { 1,0,0,0,0},
                { 1,1,1,1,1}
            },
            {
                { 1,1,1,1,1},
                { 0,0,0,0,1},
                { 1,1,1,1,1},
                { 0,0,0,0,1},
                { 1,1,1,1,1}
            },
            {
                { 1,0,0,0,1},
                { 1,0,0,0,1},
                { 1,1,1,1,1},
                { 0,0,0,0,1},
                { 0,0,0,0,1}
            },
            {
                { 1,1,1,1,1},
                { 1,0,0,0,0},
                { 1,1,1,1,1},
                { 0,0,0,0,1},
                { 1,1,1,1,1}
            },
            {
                { 1,1,1,1,1},
                { 1,0,0,0,0},
                { 1,1,1,1,1},
                { 1,0,0,0,1},
                { 1,1,1,1,1}
            },
            {
                { 1,1,1,1,1},
                { 1,0,0,0,1},
                { 1,0,0,0,1},
                { 0,0,0,0,1},
                { 0,0,0,0,1}
            },
            {
                { 1,1,1,1,1},
                { 1,0,0,0,1},
                { 1,1,1,1,1},
                { 1,0,0,0,1},
                { 1,1,1,1,1}
            },
            {
                { 1,1,1,1,1},
                { 1,0,0,0,1},
                { 1,1,1,1,1},
                { 0,0,0,0,1},
                { 1,1,1,1,1}
            }
        };


    [SerializeField] private int leftAnswer;
    [SerializeField] private int rightAnswer;
    [SerializeField] private int count = 0;
    [SerializeField] private int[,] leftPicture;
    [SerializeField] private int[,] rightPicture;
    [SerializeField] private Obj_NumberTile[] tiles;
    [SerializeField] private UI_Keypad keypad;
    [SerializeField] private UI_Page page;
    [SerializeField] private Door door;

    private bool isEnd = false;
    private int enterTileCount = 0;

    protected void Start()
    {
        base.Start();          
        SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
        //PlayerUnit.player.transform.position = startPos.position;
    }

    public void SetQuestion()
    {
        leftAnswer = Random.Range(1, 10);
        rightAnswer = Random.Range(0, 10);
        keypad.SetAnswer(leftAnswer * 10 + rightAnswer);

        leftPicture = new int[5, 5];
        rightPicture = new int[5, 5];
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                leftPicture[y, x] = numbers[leftAnswer, y, x];
                rightPicture[y, x] = numbers[rightAnswer, y, x];
            }
        }

        page.Clear();
        page.SetTitle("Stage010_ColorTile", true);
        page.AddContent("Stage010_Content_1", true);
        page.AddContent("Stage010_Content_2", true);
        page.AddContent("Stage010_Content_3", true);
    }

    


    public void EnterTile(int index)
    {
        if(index < 25)
        {
            int temp = index;
            if (leftPicture[temp / 5, temp % 5] == 1)
                tiles[index].ActiveTile(Color.green, 0.5f, true);
            else
                tiles[index].ActiveTile(Color.red, 0.5f, true) ;

        }
        else
        {
            int temp = index - 25;
            if (rightPicture[temp / 5, temp % 5] == 0)
                tiles[index].ActiveTile(Color.green, 0.5f, true);
            else
                tiles[index].ActiveTile(Color.red, 0.5f, true);
        }

        enterTileCount += 1;
        if(enterTileCount == 50)
        {
            StartCoroutine(CoEvent_Delay());
        }
    }
    public void CheckAnswer(bool b)
    {
        if (isEnd)
            return;
        if(b)
        {
            count += 1;
        }
        else
        {
            count = 0;
        }
        if(count >= 1)
        {
            door.Lock(false);
            door.Open();
            StartSignal(b);
            EndQuestion();
        }
        else
        {
            door.Close();
            door.Lock(true);
            StartSignal(b);
            SetQuestion();
        }
    }

    private void EndQuestion()
    {
        if (!isEnd)
        {
            DialogManager.Instance.ShowDialog("Dialog_Stage010_End");
            StopMusic();
        }
        isEnd = true;
        //Color orange = new Color(1, 0.63f, 0);
        //for (int y = 0; y < 5; y++)
        //{
        //    for (int x = 0; x < 5; x++)
        //    {
        //        leftMat[x + y * 5].SetColor("_Color", Color.black);
        //        rightMat[x + y * 5].SetColor("_Color", orange);
        //        //random[y, x] = 0;
        //    }
        //}
    }

    private void StartSignal(bool isAnswer)
    {
        StartCoroutine(CoSignal(isAnswer));
    }

    private IEnumerator CoSignal(bool isAnswer)
    {
        Color c = Color.yellow;
        if(!isAnswer)
            c = Color.red;
        float speed = 0.35f;

        tiles[12].ActiveTile(c, speed);

        tiles[37].ActiveTile(c, speed);
        yield return new WaitForSeconds(0.15f);
        tiles[6].ActiveTile(c, speed);
        tiles[7].ActiveTile(c, speed);
        tiles[8].ActiveTile(c, speed);
        tiles[11].ActiveTile(c, speed);
        tiles[13].ActiveTile(c, speed);
        tiles[16].ActiveTile(c, speed);
        tiles[17].ActiveTile(c, speed);
        tiles[18].ActiveTile(c, speed);

        tiles[31].ActiveTile(c, speed);
        tiles[32].ActiveTile(c, speed);
        tiles[33].ActiveTile(c, speed);
        tiles[36].ActiveTile(c, speed);
        tiles[38].ActiveTile(c, speed);
        tiles[41].ActiveTile(c, speed);
        tiles[42].ActiveTile(c, speed);
        tiles[43].ActiveTile(c, speed);
        yield return new WaitForSeconds(0.15f);
        tiles[0].ActiveTile(c, speed);
        tiles[1].ActiveTile(c, speed);
        tiles[2].ActiveTile(c, speed);
        tiles[3].ActiveTile(c, speed);
        tiles[4].ActiveTile(c, speed);
        tiles[5].ActiveTile(c, speed);
        tiles[9].ActiveTile(c, speed);
        tiles[10].ActiveTile(c, speed);
        tiles[14].ActiveTile(c, speed);
        tiles[15].ActiveTile(c, speed);
        tiles[19].ActiveTile(c, speed);
        tiles[20].ActiveTile(c, speed);
        tiles[21].ActiveTile(c, speed);
        tiles[22].ActiveTile(c, speed);
        tiles[23].ActiveTile(c, speed);
        tiles[24].ActiveTile(c, speed);

        tiles[25].ActiveTile(c, speed);
        tiles[26].ActiveTile(c, speed);
        tiles[27].ActiveTile(c, speed);
        tiles[28].ActiveTile(c, speed);
        tiles[29].ActiveTile(c, speed);
        tiles[30].ActiveTile(c, speed);
        tiles[34].ActiveTile(c, speed);
        tiles[35].ActiveTile(c, speed);
        tiles[39].ActiveTile(c, speed);
        tiles[40].ActiveTile(c, speed);
        tiles[44].ActiveTile(c, speed);
        tiles[45].ActiveTile(c, speed);
        tiles[46].ActiveTile(c, speed);
        tiles[47].ActiveTile(c, speed);
        tiles[48].ActiveTile(c, speed);
        tiles[49].ActiveTile(c, speed);
    }

    public void Event_FirstEnter()
    {
        //StartCoroutine(CoEvent_Delay());
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
            DialogManager.Instance.ShowDialog("Dialog_Stage010_FirstEnter");
    }

    private IEnumerator CoEvent_Delay()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage010_Delay");
    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_10_1"));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Acoustic_Happy_Fun_Loop", 0.2f, true, 0.5f);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
