using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Stage009 : Stage
{
    public enum PianoNote
    {
        Piano_Do = 0,
        Piano_Re = 1,
        Piano_Me = 2,
        Piano_Pa = 3,
        Piano_Sol = 4,
        Piano_La = 5,
        Piano_Si = 6,
        Piano_DoHigh = 7,
    }

    [SerializeField] private MeshRenderer[] keyBoards;
    [SerializeField] private Transform[] holdTiles;
    [SerializeField] private Inter_Note[] defaultNote;
    [SerializeField] private Inter_Note[] holdNote;
    [SerializeField] private Door door;
    [SerializeField] private Door[] doors;
    [SerializeField] private Material[] keyBoardsMats;
    [SerializeField] private UI_Page page;
    [SerializeField] private SoundSupport[] preventSound;

    [SerializeField] List<int> list = new List<int>();
    [SerializeField] List<int> list2 = new List<int>();

    private bool isAnswer = false;
    private bool isFirstAnswer = true;
    protected void Start()
    {
        base.Start();
        keyBoardsMats = new Material[keyBoards.Length];
        for(int i = 0; i  < keyBoards.Length; i++)
        {
            keyBoardsMats[i] = keyBoards[i].material;
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
        holdNote = new Inter_Note[holdTiles.Length];
        list = new List<int>();
        list2 = new List<int>();
        for (int i = 0; i < defaultNote.Length; i++)
        {
            defaultNote[i].SetCanInteract(true);
            list.Add(i);
        }
        int temp = 0;
        for (int i = 0; i < defaultNote.Length; i++)
        {
            int rand = Random.Range(0, defaultNote.Length);
            temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
        for(int i = 0; i < 8; i++)
        {
            defaultNote[list[i]].SetNote((PianoNote)i);
        }
        for (int i = 8; i < defaultNote.Length; i++)
        {
            defaultNote[list[i]].SetNote((PianoNote)Random.Range(0, 8));
        }

        
        for (int i = 0; i < 8; i++)
        {
            list2.Add(i);
        }

        for (int i = 0; i < 8; i++)
        {
            int rand = Random.Range(0, 8);
            temp = list2[i];
            list2[i] = list2[rand];
            list2[rand] = temp;
        }

        for(int i = 0; i < 4; i++)
        {
            defaultNote[list[list2[i]]].SetType(0);
            SetTile(defaultNote[list[list2[i]]], (int)defaultNote[list[list2[i]]].myNote);
            defaultNote[list[list2[i]]].SetCanInteract(false);
        }

        for (int i = 0; i < 4; i++)
        {
            defaultNote[list[list2[i + 4]]].SetType(i + 1);
            defaultNote[list[i + 8]].SetType(i + 1);
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                SetTile(defaultNote[list[list2[i + 4]]], i*2 + 8 + rand);
                SetTile(defaultNote[list[i + 8]], i*2 + 8 + rand + 1);   
            }
            else
            {
                SetTile(defaultNote[list[list2[i + 4]]], i * 2 + 8 + rand);
                SetTile(defaultNote[list[i + 8]], i * 2 + 8 + rand - 1);
            }
            if (defaultNote[list[list2[i + 4]]].myNote == defaultNote[list[i + 8]].myNote)
            {
                defaultNote[list[i + 8]].SetNote((PianoNote)((int)(defaultNote[list[i + 8]].myNote + 1) % 8));
            }

        }

        foreach(SoundSupport s in preventSound)
        {
            s.SetOnOff(true);
        }

        foreach(Door d in doors)
        {
            d.SwtLockSound(true);
        }

        page.Clear();
        page.SetTitle("Stage009_Note", true);
        page.AddContent("Stage009_Content_1", true);
        page.AddContent("Stage009_Content_2", true);
        page.AddContent("Stage009_Content_3", true);
    }

    private IEnumerator coPiano = null;
    private IEnumerator CoPiano()
    {
        for(int i = 0; i < 8; i++)
        {
            if (holdNote[i] == null)
                yield break;
            SoundManager.Instance.StartSound(holdNote[i].myNote.ToString(), 1.0f);
            StartCoroutine(CoKeyBoard(i));
            yield return new WaitForSeconds(1.0f);
        }


        bool swt = true;
        for (int i = 0; i < 8; i++)
        {
            if (holdNote[i] == null || (int)holdNote[i].myNote != i)
            {
                door.Lock(true);
                swt = false;
                break;
            }
        }
        if(swt)
        {
            isAnswer = true;
            door.Lock(false);
        }
        else
        {
            isAnswer = false;
            door.Lock(true);
        }

        if (isAnswer)
        {
            door.Open();
            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
            if(isFirstAnswer)
            {
                isFirstAnswer = false;
                StartCoroutine(CoEvent_End());
            }
        }
        else
        {
            door.Close();
            SoundManager.Instance.StartSound("SE_Button_Reset2", 1.0f);
        }    
    }

    private IEnumerator CoKeyBoard(int index)
    {
        Color color = keyBoardsMats[index].color;
        keyBoardsMats[index].color = new Color(1, 0.63f, 0);
        yield return new WaitForSeconds(1.0f);
        keyBoardsMats[index].color = color;
    }

    public void SetTile(Inter_Note holdNote, int forceIndex = -1)
    {
        if(forceIndex < 0)
        {
            Transform target = null;
            float max = 9999;
            float dis = 0;
            int index = 0;
            for (int i = 0; i < holdTiles.Length; i++)
            {
                dis = Vector3.Distance(holdTiles[i].position, holdNote.transform.position);
                if (dis <= 1.25f && dis < max && this.holdNote[i] == null)
                {
                    max = dis;
                    target = holdTiles[i];
                    index = i;
                }
            }
            if (target != null)
            {
                holdNote.transform.position = new Vector3(target.position.x, holdNote.transform.position.y, target.position.z);
                this.holdNote[index] = holdNote;
                HoldSwt(index, true, holdNote);
            }
        }
        else
        {
            Transform target = holdTiles[forceIndex];
            holdNote.transform.position = new Vector3(target.position.x, holdNote.transform.position.y, target.position.z);
            this.holdNote[forceIndex] = holdNote;
            HoldSwt(forceIndex, true, holdNote);
        }
        
        CheckAnswer();
    }

    public void HoldOutObj(Inter_Note obj)
    {
        for (int i = 0; i < holdNote.Length; i++)
        {
            if (obj == holdNote[i])
            {
                holdNote[i] = null;
                HoldSwt(i, false, obj);
                CheckAnswer();
                return;
            }
        }
    }

    private void HoldSwt(int index, bool isOn, Inter_Note note)
    {
        if((note.type + 3) * 2 != index && (note.type + 3) * 2 + 1 != index)
        {
            return;
        }

        if(index == 8)
        {
            doors[1].DoorSwt(isOn);
            doors[1].Lock(!isOn);
        }
        if (index == 9)
        {
            doors[0].DoorSwt(isOn);
            doors[0].Lock(!isOn);
        }
        if (index == 10)
        {
            doors[3].DoorSwt(isOn);
            doors[3].Lock(!isOn);
        }
        if (index == 11)
        {
            doors[2].DoorSwt(isOn);
            doors[2].Lock(!isOn);
        }
        if (index == 12)
        {
            doors[5].DoorSwt(isOn);
            doors[5].Lock(!isOn);
        }
        if (index == 13)
        {
            doors[4].DoorSwt(isOn);
            doors[4].Lock(!isOn);
        }
        if (index == 14)
        {
            doors[7].DoorSwt(isOn);
            doors[7].Lock(!isOn);
        }
        if (index == 15)
        {
            doors[6].DoorSwt(isOn);
            doors[6].Lock(!isOn);
        }
    }

    private void CheckAnswer()
    {
        for(int i = 0; i < 8; i++)
        {
            if (holdNote[i] == null)
            {
                door.Lock(true);
                return;
            }
        }

        if (coPiano != null)
            StopCoroutine(coPiano);
        coPiano = CoPiano();
        StartCoroutine(coPiano);

        //for (int i = 0; i < 8;i++)
        //{
        //    if ((int)holdNote[i].myNote != i)
        //    {
        //        door.Lock(true);
        //        isAnswer = false;
        //        return;
        //    }
        //}
        //isAnswer = true;
        //door.Lock(false);
        
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage009_FirstEnter");
    }

    private IEnumerator CoEvent_End()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage009_End");
    }

    public void Event_PianoEnter()
    {
        StartCoroutine(CoEvent_PianoEnter());
    }

    private IEnumerator CoEvent_PianoEnter()
    {
        yield return new WaitForSeconds(1.0f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage009_Piano");
    }

    public override void Hint()
    {
        string str1 = defaultNote[list[list2[4]]].myNote.ToString();
        string str2 = defaultNote[list[8]].myNote.ToString();
        str1 = Language.Instance.Get(str1);
        str2 = Language.Instance.Get(str2);
        if (hintIndex == 0)
            UIManager.Toast($"{Language.Instance.Get("Hint_9_1")} : '{str1}', '{str2}'");
        else
            base.Hint();
    }
}
