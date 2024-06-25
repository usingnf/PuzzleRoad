using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Stage012 : Stage
{
    [SerializeField] private Door door;
    [SerializeField] private Door[] doors;
    [SerializeField] private List<Door> westDoors;
    [SerializeField] private List<Door> eastDoors;
    [SerializeField] private TextMeshPro[] answerText;
    [SerializeField] private MyTextTMP[] answerTextTmp;
    [SerializeField] private UI_Page page;
    [SerializeField] private GameObject endDetector;

    [SerializeField] private List<Door> redDoors;
    [SerializeField] private List<Door> greenDoors;
    [SerializeField] private List<Door> blueDoors;
    [SerializeField] private List<Door> blackDoors;

    [SerializeField] private List<Door> answerDoor;
    [SerializeField] private int[] answer;

    private bool isFirstAnswer = true;
    private bool isAnswerCondition = false;

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
        door.Lock(true);
        foreach (Door door in doors)
        {
            //door.SetCanInteract(true);
            door.DoorSwt(false);
        }

        redDoors = new List<Door>();
        greenDoors = new List<Door>();
        blueDoors = new List<Door>();
        int rand = 0;

        doors[0].SetColor(Color.red);
        redDoors.Add(doors[0]);
        doors[1].SetColor(Color.green);
        greenDoors.Add(doors[1]);
        doors[2].SetColor(Color.blue);
        blueDoors.Add(doors[2]);
        doors[3].SetColor(Color.black);
        blackDoors.Add(doors[3]);
        for (int i = 4; i < doors.Length; i++)
        {
            rand = Random.Range(0, 4);
            if (rand == 0)
            {
                doors[i].SetColor(Color.red);
                redDoors.Add(doors[i]);
            }
            else if (rand == 1)
            {
                doors[i].SetColor(Color.green);
                greenDoors.Add(doors[i]);
            }
            else if (rand == 2)
            {
                doors[i].SetColor(Color.blue);
                blueDoors.Add(doors[i]);
            }
            else if (rand == 3)
            {
                doors[i].SetColor(Color.black);
                blackDoors.Add(doors[i]);
            }
        }
        //foreach (Door d in doors)
        //{
        //    rand = Random.Range(0, 4);
        //    if (rand == 0)
        //    {
        //        d.SetColor(Color.red);
        //        redDoors.Add(d);
        //    }
        //    else if(rand == 1)
        //    {
        //        d.SetColor(Color.green);
        //        greenDoors.Add(d);
        //    }
        //    else if (rand == 2)
        //    {
        //        d.SetColor(Color.blue);
        //        blueDoors.Add(d);
        //    }
        //    else if (rand == 3)
        //    {
        //        d.SetColor(Color.black);
        //        blackDoors.Add(d);
        //    }
        //}

        answer = new int[5];
        answerDoor = new List<Door>();
        rand = Random.Range(0, 5);
        if (rand == 0)
        {
            answerDoor.Add(doors[1]);
            answerDoor.Add(doors[0]);
            answerDoor.Add(doors[4]);
            answerDoor.Add(doors[10]);
            answerDoor.Add(doors[14]);
            answerDoor.Add(doors[15]);
            answerDoor.Add(doors[8]);

            answerDoor.Add(doors[16]);
            answerDoor.Add(doors[12]);
            answerDoor.Add(doors[9]);
            answerDoor.Add(doors[7]);

        }
        else if (rand == 1)
        {
            answerDoor.Add(doors[1]);
            answerDoor.Add(doors[0]);
            answerDoor.Add(doors[5]);
            answerDoor.Add(doors[11]);
            answerDoor.Add(doors[14]);

            answerDoor.Add(doors[16]);
            answerDoor.Add(doors[12]);
            answerDoor.Add(doors[9]);
            answerDoor.Add(doors[6]);
        }
        else if (rand == 2)
        {
            answerDoor.Add(doors[1]);
            answerDoor.Add(doors[0]);
            answerDoor.Add(doors[4]);
            answerDoor.Add(doors[5]);
            answerDoor.Add(doors[8]);

            answerDoor.Add(doors[16]);
            answerDoor.Add(doors[17]);
            answerDoor.Add(doors[13]);
            answerDoor.Add(doors[9]);
        }
        else if (rand == 3)
        {
            answerDoor.Add(doors[15]);
            answerDoor.Add(doors[14]);
            answerDoor.Add(doors[10]);
            answerDoor.Add(doors[8]);

            answerDoor.Add(doors[2]);
            answerDoor.Add(doors[6]);
            answerDoor.Add(doors[9]);
            answerDoor.Add(doors[13]);
            answerDoor.Add(doors[17]);
        }
        else if (rand == 4)
        {
            answerDoor.Add(doors[15]);
            answerDoor.Add(doors[11]);
            answerDoor.Add(doors[5]);
            answerDoor.Add(doors[0]);

            answerDoor.Add(doors[2]);
            answerDoor.Add(doors[6]);
            answerDoor.Add(doors[12]);
            answerDoor.Add(doors[17]);
            answerDoor.Add(doors[13]);
            answerDoor.Add(doors[9]);
        }
        //if (rand == 0)
        //{
        //    answerDoor.Add(doors[1]);
        //    answerDoor.Add(doors[0]);
        //    answerDoor.Add(doors[4]);
        //    answerDoor.Add(doors[12]);
        //    answerDoor.Add(doors[16]);
        //    answerDoor.Add(doors[17]);
        //    answerDoor.Add(doors[8]);

        //    answerDoor.Add(doors[2]);
        //    answerDoor.Add(doors[6]);
        //    answerDoor.Add(doors[10]);
        //}
        //else if(rand == 1)
        //{
        //    answerDoor.Add(doors[1]);
        //    answerDoor.Add(doors[0]);
        //    answerDoor.Add(doors[4]);
        //    answerDoor.Add(doors[8]);
        //    answerDoor.Add(doors[9]);

        //    answerDoor.Add(doors[10]);
        //    answerDoor.Add(doors[11]);
        //    answerDoor.Add(doors[15]);
        //    answerDoor.Add(doors[19]);
        //    answerDoor.Add(doors[18]);
        //    answerDoor.Add(doors[14]);
        //}
        //else if(rand == 2)
        //{
        //    answerDoor.Add(doors[9]);
        //    answerDoor.Add(doors[8]);
        //    answerDoor.Add(doors[12]);
        //    answerDoor.Add(doors[16]);
        //    answerDoor.Add(doors[17]);
        //    answerDoor.Add(doors[13]);

        //    answerDoor.Add(doors[2]);
        //    answerDoor.Add(doors[3]);
        //    answerDoor.Add(doors[7]);
        //    answerDoor.Add(doors[11]);
        //    answerDoor.Add(doors[10]);
        //}
        //else if (rand == 3)
        //{
        //    answerDoor.Add(doors[1]);
        //    answerDoor.Add(doors[5]);
        //    answerDoor.Add(doors[13]);
        //    answerDoor.Add(doors[17]);
        //    answerDoor.Add(doors[8]);
        //    answerDoor.Add(doors[4]);

        //    answerDoor.Add(doors[2]);
        //    answerDoor.Add(doors[3]);
        //    answerDoor.Add(doors[7]);
        //    answerDoor.Add(doors[15]);
        //    answerDoor.Add(doors[19]);
        //    answerDoor.Add(doors[18]);
        //}
        //else if (rand == 4)
        //{
        //    answerDoor.Add(doors[4]);
        //    answerDoor.Add(doors[12]);
        //    answerDoor.Add(doors[16]);
        //    answerDoor.Add(doors[17]);
        //    answerDoor.Add(doors[8]);

        //    answerDoor.Add(doors[10]);
        //    answerDoor.Add(doors[11]);
        //    answerDoor.Add(doors[15]);
        //    answerDoor.Add(doors[7]);
        //    answerDoor.Add(doors[19]);
        //    answerDoor.Add(doors[6]);
        //}

        int west = 0;
        int east = 0;
        foreach(Door d in answerDoor)
        {
            if (redDoors.Contains(d))
                answer[0] += 1;
            if (greenDoors.Contains(d))
                answer[1] += 1;
            if (blueDoors.Contains(d))
                answer[2] += 1;
            if (blackDoors.Contains(d))
                answer[4] += 1;
            if (westDoors.Contains(d))
                west += 1;
            if (eastDoors.Contains(d))
                east += 1;
        }
        if (west > east)
            answer[3] = 1;
        else
            answer[3] = 2;
        //answerText[0].text = $"빨간문은 {answer[0]}개 열려있다.";
        //answerText[1].text = $"초록문은 {answer[1]}개 열려있다.";
        //answerText[2].text = $"파란문은 {answer[2]}개 열려있다.";
        //answerText[4].text = $"검은문은 {answer[4]}개 열려있다.";
        //if (answer[3] == 1)
        //    answerText[3].text = $"서쪽문이 동쪽문보다 많이 열려있다.";
        //else
        //    answerText[3].text = $"동쪽문이 서쪽문보다 많이 열려있다.";
        
        answerTextTmp[0].SetText("Stage012_RedDoor", true, answer[0].ToString());
        answerTextTmp[1].SetText("Stage012_GreenDoor", true, answer[1].ToString());
        answerTextTmp[2].SetText("Stage012_BlueDoor", true, answer[2].ToString());
        answerTextTmp[4].SetText("Stage012_BlackDoor", true, answer[4].ToString());
        if (answer[3] == 1)
            answerTextTmp[3].SetText("Stage012_WestDoor", true);
        else
            answerTextTmp[3].SetText("Stage012_EastDoor", true);

        page.Clear();
        page.SetTitle("Stage012_Door", true);
        page.AddContent("Stage012_Content_1", true);
        page.AddContent("Stage012_Content_2", true);
        //page.AddContent("2. 갈색 버튼은 재시작버튼입니다.");
    }

    public void Event_OpenDoor(Door d)
    {
        //d.SetCanInteract(false);
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        int red = 0;
        int green = 0;
        int blue = 0;
        int black = 0;
        int west = 0;
        int east = 0;
        isAnswerCondition = false;
        foreach (Door d in doors)
        {
            if (!d.IsOpen)
                continue;
            if (redDoors.Contains(d))
                red += 1;
            if (greenDoors.Contains(d))
                green += 1;
            if (blueDoors.Contains(d))
                blue += 1;
            if (blackDoors.Contains(d))
                black += 1;
            if (westDoors.Contains(d))
                west += 1;
            if (eastDoors.Contains(d))
                east += 1;
        }
        if (answer[0] != red)
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (answer[1] != green)
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (answer[2] != blue)
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (answer[4] != black)
        {
            door.Close();
            door.Lock(true);
            return;
        }
        if (answer[3] == 1)
        {
            if(west < east)
            {
                door.Close();
                door.Lock(true);
                return;
            }
        }
        else if (answer[3] == 2)
        {
            if (west > east)
            {
                door.Close();
                door.Lock(true);
                return;
            }
        }
        isAnswerCondition = true;
        door.Lock(false);
        door.Open();
        SoundManager.Instance.StartSound("UI_Success3", 1.0f);
        if(isFirstAnswer)
        {
            isFirstAnswer = false;
            StartCoroutine(CoFirst_Answer());
        }
    }

    private IEnumerator CoFirst_Answer()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage012_End");
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
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage012_FirstEnter");
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

    public void Event_End()
    {
        if (isAnswerCondition)
        {
            StopMusic();
            endDetector.SetActive(false);
        }
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Discover_Your_World", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
