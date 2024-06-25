using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Senario005 : Stage
{
    public bool isZone = false;
    public int zoneCount = 0;
    private bool isFall = false;

    [SerializeField] private ParticleSystem particle;

    private int fallCount = 0;
    protected void Start()
    {
        base.Start();
        SetQuestion(); ;
    }
    public override void ReStage()
    {
        base.ReStage();
        PlayerUnit.player.SetPos(startPos.position);
        PlayerUnit.player.SetIsCanControl(true);
        PlayerUnit.player.Revive();
        isFall = false;
        SetQuestion();
        
        SoundManager.Instance.StartSound("SE_Player_Revive", 0.4f);
        StartCoroutine(CoParticle());
    }
    public void SetQuestion()
    {
        
    }

    private IEnumerator CoParticle()
    {
        particle.gameObject.SetActive(true);
        particle.Play();
        yield return new WaitForSeconds(0.8f);
        particle.gameObject.SetActive(false);
    }

    public void EnterZone()
    {
        isZone = true;
        if (isZone && zoneCount <= 0 && !isFall)
        {
            PlayerUnit.player.Falling();
            DelayRestart();
        }
    }

    public void ExitZone()
    {
        isZone = false;
    }

    public void EnterTile()
    {
        zoneCount += 1;
    }

    public void OutTile()
    {
        zoneCount -= 1;
        if(isZone && zoneCount <= 0 && !isFall)
        {
            PlayerUnit.player.Falling();
            DelayRestart();
        }
    }

    public void Event_FirstEnter()
    {
        StartCoroutine(CoEvent_FirstEnter());
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        yield return new WaitForSeconds(0.1f);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario005_FirstEnter", null,
                ()=>
                {
                    UIManager.Toast(Language.Instance.Get("UI_Toast_Stop", MyKey.Stop.ToString()));
                });
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);

    }

    public void Event_EndEnter()
    {
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario005_End");
        StopMusic();
    }

    private void DelayRestart()
    {
        isFall = true;
        //if(coDelayRestart != null)
        //    StopCoroutine(coDelayRestart);
        coDelayRestart = CoDelayRestart();
        StartCoroutine(coDelayRestart);
    }

    private IEnumerator coDelayRestart = null;
    private IEnumerator CoDelayRestart()
    {
        yield return new WaitForSeconds(1.5f);
        ReStage();
        fallCount += 1;
        yield return new WaitForSeconds(1.0f);
        if (fallCount == 1)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario005_Die1");
        }
        else if (fallCount == 3)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario005_Die2", null, null, fallCount.ToString(), true);
        }
        else if (fallCount == 7)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario005_Die3", null, null, "", true);
        }
        else if (fallCount == 11)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario005_Die4", null, null, "", true);
        }
        else if (fallCount == 15)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario005_Die5", null, null, "", true);
        }

    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_1005_1", MyKey.Stop.ToString()));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Orchestral_String_Action", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
