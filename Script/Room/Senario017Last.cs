using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public struct ChoiceList
{
    public string str;
    public int index;
}
public class Senario017Last : Stage
{
    [SerializeField] private GameObject obj_narration;
    [SerializeField] private Narration_Broken narration;
    [SerializeField] private GameObject tvNoise;
    [SerializeField] private Transform moveElevator;
    [SerializeField] private Transform realMove;
    [SerializeField] private GameObject obj_terrain;
    [SerializeField] private GameObject lastWall;
    [SerializeField] private Transform terrainTarget;
    [SerializeField] private ParticleSystem bird;
    [SerializeField] private List<ChoiceList> choiceList = new List<ChoiceList>();

    private void Start()
    {
        base.Start();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        Debug.Log("Y");
    //        isLastReady = true;
    //        Event9();
    //    }
    //}
    public void SetStage()
    {
        obj_terrain.SetActive(false);
        obj_narration.SetActive(true);
        PlayerUnit p = PlayerUnit.player;
        narration.Action(true);
        narration.StartSound();
        StartCoroutine(CoSetStage1(p));
    }

    private IEnumerator CoSetStage1(PlayerUnit p)
    {
        yield return new WaitForSeconds(0.5f);
        p.SwtAgent(true);
        p.GetAnimator().SetTrigger("lie1");
        yield return new WaitForSeconds(0.5f);
        p.GetAnimator().SetTrigger("lie2");
        yield return new WaitForSeconds(3.0f);

        DialogManager.Instance.ShowDialog("Dialog_Senario017_0", null, ()=>
        {
            StartCoroutine(CoSetStage2());
        });
        
    }

    private IEnumerator CoSetStage2()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
        yield return new WaitForSeconds(1.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario017_1", null, () =>
        {
            Struct_Choice[] choices = new Struct_Choice[2];
            choices[0] = new Struct_Choice(Language.Instance.Get("Narration_1017_Choice_1_1"), Event1, 0);
            choices[1] = new Struct_Choice(Language.Instance.Get("Narration_1017_Choice_1_2"), Event1, 1);
            DialogManager.Instance.ShowChoice(choices);
        });
    }

    private void Event1(int index)
    {
        PlayerUnit p = PlayerUnit.player;
        p.SetSpeed(p.DefaultSpeed, false);
        p.SetIsCanControl(true);
        if (index == 0)
            DialogManager.Instance.ShowDialog("Dialog_Senario017_2", null, Event2);
        else
            DialogManager.Instance.ShowDialog("Dialog_Senario017_3", null, Event2);
    }

    private void Event2()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario017_4", null, Event3);
        StartCoroutine(CoEvent2());
    }

    private IEnumerator CoEvent2()
    {
        yield return new WaitForSeconds(2.0f);
        CameraManager.Instance.Shake(1.0f, 0.7f, 30.0f);
        SoundManager.Instance.StartSound("SE_Screen_Shake", 1.0f);
    }

    private void Event3()
    {
        StartCoroutine(CoEvent3());
    }

    private IEnumerator CoEvent3()
    {
        yield return new WaitForSeconds(1.0f);
        tvNoise.SetActive(true);
        SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Loop", 0.5f, 1.0f, 1.0f, 3.0f);
    }

    private bool isEvent4 = false;
    //Inspector
    public void Event4()
    {
        if (isEvent4)
            return;
        isEvent4 = true;
        CameraManager.Instance.Shake(0.3f, 0.7f, 30.0f);
        SoundManager.Instance.StartSound("SE_Screen_Shake", 1.0f);
        StartCoroutine(CoEvent4());
    }

    private IEnumerator CoEvent4()
    {
        yield return new WaitForSeconds(1.0f);
        SoundManager.Instance.StartSound("SE_LastElevator_Start", 0.8f);
        realMove.DOMoveZ(realMove.position.z + 2, 2.0f).OnComplete(() =>
        {
            realMove.DOMoveZ(realMove.position.z + 128, 30.0f).OnComplete(() =>
            {
                //Event5();
            }).SetEase(Ease.Linear);
        }).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(5.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario017_5", null, Event5);
    }

    //private bool event5 = false;
    private void Event5()
    {
        //if (event5)
        //    return;
        //event5 = true;
        
        if(choiceList.Count > 0)
        {
            Struct_Choice[] choices = new Struct_Choice[choiceList.Count];
            for (int i = 0; i < choiceList.Count; i++)
            {
                choices[i] = new Struct_Choice(Language.Instance.Get(choiceList[i].str), Event6, choiceList[i].index);
            }
            DialogManager.Instance.ShowChoice(choices);
        }
        else
        {
            Event7();
        }
        //choices[0] = new Struct_Choice("여기는 뭐하는 곳이야?", Event6, 0);
        //choices[1] = new Struct_Choice("당신의 정체는 뭐야?", Event6, 1);
        //choices[2] = new Struct_Choice("탈출한 다음에는 어떻게 해?", Event6, 2);
    }

    private void Event6(int index)
    {
        for (int i = 0; i < choiceList.Count; i++)
        {
            if (choiceList[i].index == index)
            {
                choiceList.Remove(choiceList[i]);
                break;
            }
        }
        if (index == 0)
            DialogManager.Instance.ShowDialog("Dialog_Senario017_6", null, Event5);
        else if (index == 1)
            DialogManager.Instance.ShowDialog("Dialog_Senario017_7", null, Event5);
        else if (index == 2)
            DialogManager.Instance.ShowDialog("Dialog_Senario017_8", null, Event5);
        else
            Event7();
    }

    private void Event7()
    {
        StartCoroutine(CoEvent7());
    }

    private IEnumerator CoEvent7()
    {
        yield return new WaitForSeconds(1.5f);
        DialogManager.Instance.ShowDialog("Dialog_Senario017_9", null, Event8);
    }

    private void Event8()
    {
        StartCoroutine (CoEvent8());
    }

    private IEnumerator CoEvent8()
    {
        yield return new WaitForSeconds(1.0f);
        narration.Action(false);
        narration.StopSound();
        SoundManager.Instance.StartSound("SE_Narration_Stop", 0.8f);
        realMove.DOMoveZ(realMove.position.z + 33, 10.0f).OnComplete(() =>
        {
            realMove.DOMoveZ(realMove.position.z + 2, 1.0f).OnComplete(() =>
            {
                lastWall.SetActive(false);
                isLastReady = true;
                SoundManager.Instance.StartSound("SE_Impact_3", 0.4f);
            }).SetEase(Ease.InCubic);
            StopMusic();
        }).SetEase(Ease.Linear);
    }

    private bool isLastReady = false;
    public void Event9()
    {
        if (!isLastReady)
            return;
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        UIManager.Instance.FadeIn(3.0f, Color.white, null);
        SoundManager.Instance.StartSound("SE_Fade_Start", 0.8f);
        StartCoroutine(CoEvent9(p));
    }

    private IEnumerator CoEvent9(PlayerUnit p)
    {
        yield return new WaitForSeconds(3.0f);
        moveElevator.gameObject.SetActive(false);
        obj_terrain.SetActive(true);
        p.SetPos(terrainTarget.position);
        yield return new WaitForSeconds(1.0f);
        p.transform.rotation = Quaternion.Euler(0, 180, 0);
        UIManager.Instance.FadeOut(3.0f, null);
        yield return new WaitForSeconds(1.2f);
        p.GetAnimator().SetTrigger("around");
        p.SetForceWalk(true);
        AudioSource audio = SoundManager.Instance.StartSoundFadeLoop("SE_River_Loop", 0.6f, 1.0f, 0, 100);
        yield return new WaitForSeconds(3.0f);
        SoundManager.Instance.StartSound("SE_Bird_1", 0.6f);
        yield return new WaitForSeconds(4.5f);
        CameraManager.Instance.SetTarget(null);
        SoundManager.Instance.StartSound("SE_Bird_2", 0.6f);
        p.SetSpeed(2, false);
        p.SetDestination(p.transform.position + new Vector3(0, 0, -3));
        bird.Play();
        yield return new WaitForSeconds(3.0f);
        SoundManager.Instance.StartSound("SE_Bird_1", 0.6f);
        p.SetForceWalk(false);
        p.SetSpeed(p.DefaultSpeed, false);
        p.SetDestination(p.transform.position + new Vector3(0, 0, -30));
        UIManager.Instance.StartTrueEnd();
        yield return new WaitForSeconds(2.0f);
        SoundManager.Instance.StopSound(audio, 3.0f);
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Feel_My_Heart_Beating", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
