using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senario010 : Stage
{
    [SerializeField] private Door[] doors;

    private int lastIndex = -10;
    private int openIndex = 0;
    private bool isFirst = true;

    private int openCount = 0;
    protected void Start()
    {
        base.Start();
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        //PlayerUnit.player.SetPos(startPos.position);
        SetQuestion();
    }
    public void SetQuestion()
    {
        isFirst = true;
        lastIndex = -1;
        openIndex = 0;
        doors[0].Open();
    }

    public void EnterDoor(int index)
    {
        if(isFirst && index != 0)
        {
            return;
        }
        isFirst = false;
        if (lastIndex == index)
            return;
        lastIndex = index;
        openIndex += 1;
        foreach (Door door in doors)
        {
            door.Close();
        }
        doors[openIndex % doors.Length].Open();

        openCount += 1;
        if (openCount == 5)
            DialogManager.Instance.ShowDialog("Dialog_Senario010_FirstDoor");
    }

    public void Event_FirstEnter()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
    }

    public void Event_ClearFirst()
    {
        hintIndex = 1;
    }

    private bool secretDoor = false;
    public void Event_OpenSecretDoor()
    {
        if (secretDoor)
            return;
        secretDoor = true;
        DialogManager.Instance.ShowDialog("Dialog_Senario010_Secret");
    }

    public void Event_End()
    {
        StopMusic();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Happy_Fun", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
