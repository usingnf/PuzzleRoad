using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Stage005 : Stage
{
    private WaitForSeconds wait = new WaitForSeconds(0.1f);

    [SerializeField] private Obj_Lazer[] lazers;
    [SerializeField] private Inter_LazerWall[] walls;
    [SerializeField] private Door door;
    [SerializeField] private UI_Page page;
    private bool isFinalAnswer = false;

    private bool isFirstDie = true;
    private bool isEnd = false;
    
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        SetQuestion();
        
    }
    private void OnEnable()
    {
        StartCoroutine(CoCheckAnswer());
    }

    public override void ReStage()
    {
        base.ReStage();
        PlayerUnit.player.SetHoldObj(null);
        door.Close();
        door.Lock(true);
        SetQuestion();
        PlayerUnit.player.SetPos(startPos.position);
        if(isFirstDie)
        {
            isFirstDie = false;
            StartCoroutine(CoFirstDie());
        }
        //PlayerUnit.player.SetSpeed(PlayerUnit.player.DefaultSpeed, false);
        //PlayerUnit.player.Revive();
    }

    private IEnumerator CoFirstDie()
    {
        yield return new WaitForSeconds(0.5f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Stage005_Die");
    }
    public void SetQuestion()
    {
        foreach(Inter_LazerWall wall in walls)
        {
            wall.HoldOut();
        }
        walls[0].transform.position = this.transform.position + new Vector3(17, 0.5f, 16.5f);
        walls[1].transform.position = this.transform.position + new Vector3(23.5f, 0.5f, 2.5f);
        walls[2].transform.position = this.transform.position + new Vector3(26, 0.5f, 2.5f);
        walls[3].transform.position = this.transform.position + new Vector3(20, 0.5f, 16.5f);

        page.Clear();
        page.SetTitle("Stage005_Lazer", true);
        page.AddContent("Stage005_Content_1", true);
        page.AddContent("Stage005_Content_2", true);
        page.AddContent("Stage005_Content_3", true, MyKey.Walk.ToString());
    }

    private IEnumerator CoCheckAnswer()
    {
        while(true)
        {
            CheckAnswer();
            yield return wait;
        }
    }
    public void CheckAnswer()
    {
        if (isEnd)
            return;
        bool isAnswer = true;
        foreach(Obj_Lazer lazer in lazers)
        {
            if(!lazer.isAnswer)
            {
                isAnswer = false;
                isFinalAnswer = false;
                break;
            }
        }
        door.Lock(!isAnswer);
        if(isAnswer && !isFinalAnswer)
        {
            door.Open();
            SoundManager.Instance.StartSound("UI_Success3", 1.0f);
            StopMusic();
        }
        isFinalAnswer = isAnswer;
    }

    public void EndQuestion()
    {
        isEnd = true;
        foreach (Obj_Lazer lazer in lazers)
        {
            lazer.SwtLaser(false);
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
            float t = DialogManager.Instance.ShowDialog("Dialog_Stage005_FirstEnter");
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
            UIManager.Toast(Language.Instance.Get("Hint_5_1", MyKey.Walk.ToString()));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Mermaid_Short_Loop", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
