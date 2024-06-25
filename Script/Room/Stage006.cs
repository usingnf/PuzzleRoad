using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Stage006 : Stage
{
    [SerializeField] private Transform[] defaultWalls;
    [SerializeField] private Transform[,] walls;
    [SerializeField] private UI_Page page;
    [SerializeField] private Door door;
    private bool isFirstForceRestage = true;
    private bool isFirstAnswer = true;

    private bool isAnswer = false;
    protected void Start()
    {
        base.Start();
        SetQuestion();
    }

    //private void Update()
    //{
    //    if (isAnswer)
    //        return;

    //    //if(Input.GetKeyDown(MyKey.Reset))
    //    //{
    //    //    if (GameManager.Exist())
    //    //        if (GameManager.Instance.GetCurrentStage() != this.GetStageNum())
    //    //            return;
    //    //    ReStage();
    //    //    PlayerUnit.player.SetPos(startPos.position);
    //    //    SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
    //    //}
    //}

    public void ForceReStage()
    {
        if(coReStage != null)
            StopCoroutine(coReStage);
        coReStage = CoReStage();
        StartCoroutine(coReStage);
    }

    private IEnumerator coReStage = null;
    private IEnumerator CoReStage()
    {
        if (isFirstForceRestage)
        {
            isFirstForceRestage = false;
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage006_Restage");
            yield return new WaitForSeconds(t + 1.0f);

            SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
            PlayerUnit.player.SetPos(startPos.position);
            ReStage();
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
            PlayerUnit.player.SetPos(startPos.position);
            ReStage();
        }
        
    }
    public override void ReStage()
    {
        base.ReStage();
        if (coReStage != null)
            StopCoroutine(coReStage);
        door.Close();
        door.Lock(true);
        isAnswer = false;
        SetQuestion();
        //PlayerUnit.player.SetPos(startPos.position);
    }
    public void SetQuestion()
    {
        walls = new Transform[13, 13];
        for(int x = 0; x < 13; x++)
        {
            for (int y = 0; y < 13; y++)
            {
                walls[x,y] = defaultWalls[x + y * 13].transform;
                walls[x, y].localPosition = new Vector3(26 - y * 2, 0.5f, 12 - x * 2);
                if (walls[x,y].TryGetComponent<Inter_PushWall>(out Inter_PushWall pushWall))
                {
                    pushWall.x = x; pushWall.y = y;
                }
            }
        }

        page.Clear();
        page.SetTitle("Stage006_BoxMaze", true);
        page.AddContent("Stage006_Content_1", true);
        page.AddContent("Stage006_Content_2", true);
        page.AddContent("Stage006_Content_3", true);
        page.AddContent("Stage006_Content_4", true);
        //page.AddContent("Stage006_Content_5", true, MyKey.Reset.ToString());
    }

    public void PushWall(Vector3 unitPos, Inter_PushWall wall)
    {
        Vector3 vec = unitPos - wall.transform.position;
        float ang = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;
        if (ang < 0)
            ang += 360;
        int x = wall.x;
        int y = wall.y;
        Transform tempTrans = null;
        Vector3 tempVec;
        // 왼쪽위가 0,0
        if(ang < 45)
        {
            //위에서 아래
            if (y+1 < 13 && !walls[x,y+1].gameObject.activeSelf)
            {
                tempTrans = walls[x, y];
                tempVec = tempTrans.position;
                walls[x, y].position = walls[x, y + 1].position;
                walls[x, y + 1].position = tempVec;
                walls[x, y] = walls[x, y + 1];
                walls[x, y + 1] = tempTrans;
                wall.y = y + 1;
                if (y + 1 == 11 && x != 6)
                    SoundManager.Instance.StartSound("UI_Success2", 0.8f);
                if(y+1 == 11 && x == 6)
                    ForceReStage();
            }
        }
        else if(ang < 135)
        {
            // 왼쪽에서 오른쪽
            if (x + 1 < 13 && !walls[x + 1, y].gameObject.activeSelf)
            {
                tempTrans = walls[x, y];
                tempVec = tempTrans.position;
                walls[x, y].position = walls[x + 1, y].position;
                walls[x + 1, y].position = tempVec;
                walls[x, y] = walls[x + 1, y];
                walls[x + 1, y] = tempTrans;
                wall.x = x + 1;
                if(x + 1 == 11)
                    SoundManager.Instance.StartSound("UI_Success2", 0.8f);
            }
        }
        else if (ang < 225)
        {
            //아래에서 위
            if (y - 1 > 0 && !walls[x, y - 1].gameObject.activeSelf)
            {
                tempTrans = walls[x, y];
                tempVec = tempTrans.position;
                walls[x, y].position = walls[x, y - 1].position;
                walls[x, y - 1].position = tempVec;
                walls[x, y] = walls[x, y - 1];
                walls[x, y - 1] = tempTrans;
                wall.y = y - 1;
                if(y - 1 == 1 && x != 6)
                    SoundManager.Instance.StartSound("UI_Success2", 0.8f);
                if(y - 1 == 1 && x == 6)
                    ForceReStage();
            }
        }
        else if (ang < 315)
        {
            //오른쪽에서 왼쪽
            if (x - 1 > 0 && !walls[x - 1, y].gameObject.activeSelf)
            {
                tempTrans = walls[x, y];
                tempVec = tempTrans.position;
                walls[x, y].position = walls[x - 1, y].position;
                walls[x - 1, y].position = tempVec;
                walls[x, y] = walls[x - 1, y];
                walls[x - 1, y] = tempTrans;
                wall.x = x - 1;
                if(x - 1 == 1)
                    SoundManager.Instance.StartSound("UI_Success2", 0.8f);
            }
        }
        else
        {
            //위에서 아래
            if (y + 1 < 13 && !walls[x, y + 1].gameObject.activeSelf)
            {
                tempTrans = walls[x, y];
                tempVec = tempTrans.position;
                walls[x, y].position = walls[x, y + 1].position;
                walls[x, y + 1].position = tempVec;
                walls[x, y] = walls[x, y + 1];
                walls[x, y + 1] = tempTrans;
                wall.y = y + 1;
                if(y + 1 == 11 && x != 6)
                    SoundManager.Instance.StartSound("UI_Success2", 0.8f);
                if(y + 1 == 11 && x == 6)
                    ForceReStage();
            }
        }
        CheckAnswer();
    }

    public void CheckAnswer()
    {
        if (walls[2, 1].gameObject.activeSelf 
            && walls[4, 1].gameObject.activeSelf
            && walls[8, 1].gameObject.activeSelf
            && walls[10, 1].gameObject.activeSelf
            && walls[1, 2].gameObject.activeSelf
            && walls[11, 2].gameObject.activeSelf
            && walls[1, 4].gameObject.activeSelf
            && walls[11, 4].gameObject.activeSelf
            && walls[1, 6].gameObject.activeSelf
            && walls[11, 6].gameObject.activeSelf
            && walls[1, 8].gameObject.activeSelf
            && walls[11, 8].gameObject.activeSelf
            && walls[1, 10].gameObject.activeSelf
            && walls[11, 10].gameObject.activeSelf
            && walls[2, 11].gameObject.activeSelf
            && walls[4, 11].gameObject.activeSelf
            && walls[8, 11].gameObject.activeSelf
            && walls[10, 11].gameObject.activeSelf
            )
        {
            door.Lock(false);
            door.Open();
            isAnswer = true;
            SoundManager.Instance.StartSound("UI_Success3", 0.8f);
            StopMusic();
            if (isFirstAnswer)
            {
                isFirstAnswer = false;
                DialogManager.Instance.ShowDialog("Dialog_Stage006_End");
            }
        }
        else
        {
            isAnswer = false;
            door.Lock(true);
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
            DialogManager.Instance.ShowDialog("Dialog_Stage006_FirstEnter");
    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_6_1", MyKey.Reset.ToString()));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Happy_Little_Tune_Loop3", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }

    public void StartMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.5f);
        StartCoroutine(coMusic);
    }
}
