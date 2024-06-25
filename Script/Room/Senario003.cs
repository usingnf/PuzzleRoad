using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Senario003 : Stage
{
    [SerializeField] private GameObject ui_save;
    [SerializeField] private GameObject[] noise;
    [SerializeField] private Transform rock;
    [SerializeField] private Transform btnFloor;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem particle_revive;
    protected void Start()
    {
        base.Start();
        SetQuestion();
    }
    public override void ReStage()
    {
        base.ReStage();
        PlayerUnit.player.SetPos(startPos.position);
        PlayerUnit.player.SetIsCanControl(true);
        rock.localPosition = new Vector3(31, 50, 10);
        SetQuestion();
        SoundManager.Instance.StartSound("SE_Player_Revive", 0.4f);
        StartCoroutine(CoParticle());
    }
    public void SetQuestion()
    {


    }

    private IEnumerator CoParticle()
    {
        particle_revive.gameObject.SetActive(true);
        particle_revive.Play();
        yield return new WaitForSeconds(0.8f);
        particle_revive.gameObject.SetActive(false);
    }



    public void Event_Save(Material mat)
    {
        float t = 3.5f;
        mat.SetFloat("_Step", 1.0f);
        mat.DOFloat(0, "_Step", t);
        StartCoroutine(CoEvent_Save(mat, t));
    }

    private IEnumerator CoEvent_Save(Material mat, float t)
    {
        IEnumerator coWhile = CoWhile(mat);
        StartCoroutine(coWhile);
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Save", 0.8f, 0.4f, 0, t + 1f);
        SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Loop", 0.8f, 0.4f, 0.4f, t + 3.5f);
        yield return new WaitForSeconds(t + 1);
        SoundManager.Instance.StopSound(audio);
        noise[0].SetActive(true);
        //foreach (GameObject obj in noise)
        //{
        //    obj.SetActive(true);
        //}
        ui_save.SetActive(false);
        if (coWhile != null)
            StopCoroutine(coWhile);
        PlayerUnit.player.SetIsCanControl(true);
        yield return new WaitForSeconds(t);
        DialogManager.Instance.ShowDialog("Dialog_Senario003_Save", null, () => 
        {
            noise[1].SetActive(true);
            SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Loop", 0.5f, 0.4f, 0.4f, 2.5f);
        });
    }

    private bool isEventSave = false;
    public void Event_Save2()
    {
        if (isEventSave)
            return;
        isEventSave = true;
        StartCoroutine(CoEvent_Save2());
    }

    private IEnumerator CoEvent_Save2()
    {
        PlayerUnit p = PlayerUnit.player;
        SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Loop", 0.8f, 0.4f, 0.4f, 3.5f);
        noise[0].SetActive(true);
        p.SetIsCanControl(false);
        p.GetAnimator().SetTrigger("backdown");
        //foreach (GameObject obj in noise)
        //{
        //    obj.SetActive(true);
        //}
        yield return new WaitForSeconds(3.5f);
        p.SetIsCanControl(true);
        DialogManager.Instance.ShowDialog("Dialog_Senario003_Save", null, () =>
        {
            noise[1].SetActive(true);
            SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Loop", 0.5f, 0.4f, 0.4f, 2.5f);
        });
    }

    private IEnumerator CoWhile(Material mat)
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (true)
        {
            mat.SetFloat("_Pos", Random.Range(0.0f, 1.0f));
            yield return wait;
        }
    }

    public void FallingRock()
    {
        PlayerUnit.player.SetIsCanControl(false);
        PlayerUnit.player.SetDestination(btnFloor.position);
        SoundManager.Instance.StartSound("SE_Rock_Start", 1.0f);
        SoundManager.Instance.StartSound("SE_Rock_Fall", 1.0f);
        rock.DOLocalMoveY(2, 0.45f).OnComplete(() =>
        {
            particle.gameObject.SetActive(true);
            particle.Play();
            Transform trans = PlayerUnit.player.CreateBlood();
            PlayerUnit.player.Death();
            trans.parent = Obj_stage;
            SoundManager.Instance.StartSound("SE_Rock_Explosion", 1.0f);
        });
        RockRestart();
        hintIndex = 1;
    }

    private int dieCount = 0;
    private void RockRestart()
    {
        if(coRockRestart != null)
            StopCoroutine(coRockRestart);
        coRockRestart = CoRockRestart();
        StartCoroutine(coRockRestart);
    }

    private IEnumerator coRockRestart = null;
    private IEnumerator CoRockRestart()
    {
        yield return new WaitForSeconds(1.5f);
        particle.gameObject.SetActive(false);
        ReStage();
        if (dieCount == 0)
        {
            dieCount += 1;
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario003_Rock");
        }
        else if (dieCount == 1)
        {
            dieCount += 1;
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario003_Rock2");
        }
        else if(dieCount == 2)
        {
            dieCount += 1;
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario003_Rock3");
        }
    }


    public override void Hint()
    {
        if (hintIndex == 1)
            UIManager.Toast(Language.Instance.Get("Hint_1003_1"));
        else
            base.Hint();
    }
}
