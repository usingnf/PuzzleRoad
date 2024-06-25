using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using static Inter_SwtCore;
using static UnityEngine.ParticleSystem;

public class SenarioTrue : Stage
{
    private enum ProgressType
    {
        Off = 0,
        Success = 1,
        Failure = 2
    }

    [SerializeField] private Senario017Last lastStage;

    [SerializeField] private Obj_FallingTile[] fallingTiles;
    [SerializeField] private Obj_FallingTile[] fallingChoices;
    [SerializeField] private Obj_FallingTile[] fallingTiles2;
    [SerializeField] private GameObject[] fallingDetectors;
    [SerializeField] private ParticleSystem reviveParticle;

    [SerializeField] private Transform cubeGroup;
    [SerializeField] private Transform cubeGroup2;

    [SerializeField] private Transform water_falling;
    [SerializeField] private Transform water_flow;
    [SerializeField] private Transform water_falling2;
    [SerializeField] private GameObject detector_waterfalling;
    [SerializeField] private GameObject detector_water;
    [SerializeField] private GameObject detector_waterSound;

    [SerializeField] private Transform secondRevivePos;

    [SerializeField] private Material mat_core;
    [SerializeField] private MeshRenderer render_core;
    [SerializeField] private AutoSoundVolume sound_core;

    [SerializeField] private float corePower_correct;
    [SerializeField] private float corePower_incorrect;
    [SerializeField] private Color[] coreColors;
    [SerializeField] private GameObject obj_bossRoom;
    [SerializeField] private GameObject obj_bossRoom_broken;
    [SerializeField] private MeshRenderer[] progressRenders;
    [SerializeField] private Material[] progressMaterials;
    [SerializeField] private Inter_Gate gate;
    [SerializeField] private Transform failedPos;

    [SerializeField] private GameObject trueObj;
    [SerializeField] private GameObject normalObj;
    [SerializeField] private GameObject spaceObj;
    [SerializeField] private GameObject spaceNoiseObj;
    [SerializeField] private Transform camTarget;
    [SerializeField] private Animator ani_shield;
    [SerializeField] private GameObject obj_narration_shield;

    [SerializeField] private Blackhole blackhole;
    [SerializeField] private Transform narration_event;
    [SerializeField] private Transform narration_broken;
    [SerializeField] private Narration_Boss narration_boss;
    [SerializeField] private GameObject bossTraps;


    private bool isBossStart = false;
    private bool isGateOpen = false;
    private bool isPerfectSuccess = false;
    private bool isEndBoss = false;
    private bool isCanExit = true;
    private int bossPhase = 0;
    private SwtCoreType coreTargetColor;
    private ProgressType[] progressStates = new ProgressType[10];

    private bool isFalling = false;
    private int reviveCount = 0;
    private bool isMeetBoss = false;
    protected void Start()
    {
        base.Start();
        mat_core = render_core.material;
        spaceObj.SetActive(false);
        SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        PlayerUnit.player.Revive();
        PlayerUnit.player.SetPos(startPos.position);
        detector_water.SetActive(true);
        detector_waterSound.SetActive(false);
        if (isBossStart)
        {
            Fail_Boss();
        }
        StartCoroutine(CoParticle());
        SetQuestion();

        reviveCount += 1;
        if(fallCount == 2)
        {
            fallCount += 1;
        }
        else
        {
            if(!isMeetBoss)
            {
                if (reviveCount == 5)
                {
                    DialogManager.Instance.ShowDialog("Dialog_Senario016_Revive1");
                }
                else if (reviveCount == 10)
                {
                    DialogManager.Instance.ShowDialog("Dialog_Senario016_Revive2");
                }
            }
        }
        
    }
    public void SetQuestion()
    {
        isFalling = false;
        isBossStart = false;
        
        water_falling.localScale = new Vector3(0, water_falling.localScale.y, water_falling.localScale.z);
        water_flow.localScale = new Vector3(water_flow.localScale.x, 0, 0);
        water_falling2.localScale = new Vector3(0, water_falling2.localScale.y, water_falling2.localScale.z);
    }

    private bool isBadEnd2 = false;
    private bool isCanBackElevator = false;

    public void Event_BadEnd2()
    {
        if (isBadEnd2)
            return;
        if (!isCanBackElevator)
            return;
        if (startElevator.GetIsProgress())
            return;
        isBadEnd2 = true;
        StartCoroutine(BadEnd2());
        StopMusic();
        LightManager.Instance.StopWarning();
    }

    private IEnumerator BadEnd2()
    {
        startElevator.ExcuteSoloBadEnd2(PlayerUnit.player, true);
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario016_BadEnd2");
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.StartBadEnd2();
    }
    public void Event_FirstEnter()
    {
        //LightManager.Instance.StopWarning();
        isCanBackElevator = true;
        LightManager.Instance.StartWarning();
        PlayerUnit.player.SetIsCanControl(false);
        PlayerUnit.player.SetDestination(startPos.position);
        StartCoroutine(CoEvent_FirstEnter());   
    }

    private IEnumerator CoEvent_FirstEnter()
    {
        yield return new WaitForSeconds(1.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FirstEnter", null, (() =>
        {
            PlayerUnit.player.SetIsCanControl(true);
            startPos.gameObject.SetActive(true);
            UIManager.Toast(Language.Instance.Get("UI_Toast_CheckPoint1"));
            StartCoroutine(CoParticle());
        }));
    }

    private int fallCount = 0;
    public void FallingPlayer(PlayerUnit player)
    {
        if (isFalling)
            return;
        fallCount += 1;
        isFalling = true;
        DecreaseDifficultFallingTile();
        player.Falling(FallRestage);
        if(fallCount == 2)
        {
            StartCoroutine(CoFallingPlayer());
        }
    }

    private IEnumerator CoFallingPlayer()
    {
        yield return new WaitForSeconds(1.0f);
        if (isMeetBoss)
            yield break;
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FallingDie");
    }

    private void FallRestage()
    {
        ReStage();
        //SoundManager.Instance.StartSound("SE_Player_Revive", 0.4f);
        
    }
    private IEnumerator CoParticle()
    {
        SoundManager.Instance.StartSound("SE_Player_Revive", 0.4f);
        reviveParticle.gameObject.SetActive(true);
        reviveParticle.Play();
        yield return new WaitForSeconds(1.0f);
        reviveParticle.gameObject.SetActive(false);
    }

    public void Event_FallingTile()
    {
        FallingTiles();
        LightManager.Instance.StopWarning();
    }
    public void FallingTiles()
    {
        CameraManager.Instance.Shake(0.5f, 2.0f, 30);
        SoundManager.Instance.StartSound("SE_Rock_Explosion", 1.0f);
        PlayerUnit.player.SetIsCanControl(false);
        foreach(Obj_FallingTile tile in fallingTiles)
            tile.FallingTile();
        StartCoroutine(CoFallingTiles());
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
    }

    private IEnumerator CoFallingTiles()
    {
        yield return new WaitForSeconds(1.0f);
        PlayerUnit.player.SetIsCanControl(true);
        yield return new WaitForSeconds(0.5f);
        if(DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario016_FallingTile1");
    }

    public void Event_FallingTile2(int index)
    {
        if (isFalling)
            return;
        FallingTiles2(index);
    }

    public void FallingTiles2(int index)
    {
        CameraManager.Instance.Shake(0.5f, 2.0f, 30);
        SoundManager.Instance.StartSound("SE_Rock_Explosion", 1.0f);
        PlayerUnit.player.SetIsCanControl(false);
        foreach (GameObject obj in fallingDetectors)
            obj.SetActive(false);

        for (int i = 0; i < fallingChoices.Length; i++)
        {
            if (index == i)
                continue;
            fallingChoices[i].FallingTile();
        }
        foreach (Obj_FallingTile tile in fallingTiles2)
            tile.FallingTile();
        
        StartCoroutine (CoFallingTiles2());
    }

    private IEnumerator CoFallingTiles2()
    {
        yield return new WaitForSeconds(1.0f);
        PlayerUnit.player.SetIsCanControl(true);
        yield return new WaitForSeconds(0.5f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FallingTile2");
    }

    public void DecreaseDifficultFallingTile()
    {
        foreach (Obj_FallingTile tile in fallingTiles)
            tile.DecreaseDifficult();
        foreach (Obj_FallingTile tile in fallingTiles2)
            tile.DecreaseDifficult();
        foreach (Obj_FallingTile tile in fallingChoices)
            tile.DecreaseDifficult();
    }

    public void Event_FallingBolt()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FallingBolt");
    }

    public void Event_FallingCube()
    {
        CameraManager.Instance.Shake(0.5f, 2.0f, 30);
        SoundManager.Instance.StartSound("SE_Rock_Explosion", 1.0f);
        StartCoroutine(CoFallingCube());
    }
    private IEnumerator CoFallingCube()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Transform trans in cubeGroup)
        {
            trans.rotation = Quaternion.Euler(new Vector3(Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f)));
            trans.gameObject.SetActive(true);
        }
        foreach (Transform trans in cubeGroup2)
        {
            //trans.rotation = Quaternion.Euler(new Vector3(Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f)));
            trans.gameObject.SetActive(true);
        }
    }

    public void Event_FallingCubeDialog()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FallingCube");
    }

    public void Event_Water()
    {
        water_falling.DOScaleX(0.8f, 0.5f).OnComplete(() =>
        {
            water_flow.DOScale(Vector3.one, 1.5f).OnComplete(() =>
            {
                detector_waterfalling.SetActive(true);
                water_falling2.DOScaleX(0.8f, 0.5f).OnComplete(() =>
                {

                }).SetEase(Ease.Linear);
            }).SetEase(Ease.Linear);
        }).SetEase(Ease.Linear);
    }

    public void FallingPlayerWater()
    {
        if (isFalling)
            return;

        isFalling = true;
        PlayerUnit.player.Falling(ReStage);
    }

    public void Event_LazerDie()
    {
        ReStage();
    }

    public void Event_LazerDialog()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario016_Lazer");
    }

    public void Event_StopWarning()
    {
        LightManager.Instance.StopWarning();
    }

    public void Event_EndTrap()
    {
        startPos.position = secondRevivePos.position;
        UIManager.Toast(Language.Instance.Get("UI_Toast_CheckPoint2"));
        StartCoroutine(CoEvent_EndTrap());
        StopMusic();
    }

    private IEnumerator CoEvent_EndTrap()
    {
        yield return new WaitForSeconds(0.8f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_EndTrap");
    }

    public void EnterRoom(Transform target)
    {
        PlayerUnit.player.SetPos(target.position);
        InteractableObject obj = PlayerUnit.player.GetHoldObj();
        if (obj != null)
        {
            PlayerUnit.player.DestroyHoldObj();
        }
    }

    public void ExitRoom(Transform target)
    {
        if (!isCanExit)
            return;
        PlayerUnit.player.SetPos(target.position);
        if (!isEndBoss)
            Fail_Boss();
        else
            Stop_Boss();
    }

    //private Inter_SwtCore.SwtCoreType[] swtcore_types;
    [SerializeField] private Inter_SwtCore[] swt_cores;
    public void SwtCore(int index)
    {
        if(!isBossStart)
        {
            swt_cores[index].SetSwt(SwtCoreType.Yellow);
        }
        else
        {
            int coreType = ((int)swt_cores[index].state + 1) % 6;
            if (coreType < 2)
                coreType = 2;
            swt_cores[index].SetSwt((SwtCoreType)coreType);
        }
        
        //swtcore_types[index] = Inter_SwtCore.SwtCoreType.On;
        int count = GetActiveSwtCore();
        SetCorePower();
        if(!isBossStart && count >= 4)
        {
            Start_Boss();
        }
    }

    private int GetActiveSwtCore()
    {
        int index = 0;
        foreach(Inter_SwtCore core in swt_cores)
        {
            if (core.isOn)
                index += 1;
        }
        return index;
    }

    private void SetCorePower(float power = 0.0f, float speed = 0.0f, float t = 0.0f)
    {
        if(power == 0.0f)
        {
            float on = 0;
            foreach (Inter_SwtCore core in swt_cores)
            {
                if (core.isOn)
                    on += 1;
            }
            if (on == 0)
                on = 0.8f;

            //mat_core.SetColor("_Color", coreColors[over]);
            mat_core.DOFloat(on * 12.5f, "_EmissionStrength", 1.0f);
            mat_core.DOFloat(on * 2.5f, "_UVSpeed", 1.0f);
            //mat_core.SetFloat("_EmissionStrength", on * 12.5f);
            //mat_core.SetFloat("_UVSpeed", on * 2.5f);
            if (on > 0)
                sound_core.SwtSound(true);
            sound_core.SetVolume(on * 0.25f * 0.5f, 0.5f);
        }
        else
        {
            if(power != -1.0f)
                mat_core.DOFloat(power, "_EmissionStrength", t);
            if(speed != -1.0f)
                mat_core.DOFloat(speed, "_UVSpeed", t);
            if (power <= 1)
                sound_core.SetVolume(0, 0.5f);
            else
                sound_core.SetVolume(0.5f, 0.5f);
        }
        // power : 0~50

    }

    private void SetCoreColor(SwtCoreType colorIndex, float t = 1.0f)
    {
        mat_core.DOColor(coreColors[(int)colorIndex], t);
        coreTargetColor = colorIndex;
        //mat_core.SetColor("_Color", coreColors[over]);
    }

    [SerializeField] private Transform coreCenterPos;
    [SerializeField] private Transform bossAppearPos;
    private void Start_Boss()
    {
        if (isBossStart)
            return;
        isMeetBoss = true;
        isGateOpen = false;
        bossPhase = 0;
        isEndBoss = false;
        isPerfectSuccess = false;
        bossTraps.SetActive(true);
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        p.LookAt(coreCenterPos.position, 1.0f);
        CameraManager.Instance.SetTarget(null);
        CameraManager.Instance.transform.DOMove(coreCenterPos.position + new Vector3(1.0f, 1.0f, 2.0f), 1.0f);
        if (coStart_Boss != null)
            StopCoroutine(coStart_Boss);
        coStart_Boss = CoStart_Boss(p);
        StartCoroutine(coStart_Boss);
    }

    private IEnumerator coStart_Boss = null;
    private IEnumerator CoStart_Boss(PlayerUnit p)
    {
        yield return new WaitForSeconds(2.0f);
        narration_boss.Appear();
        yield return new WaitForSeconds(0.5f);
        p.GetAnimator().SetTrigger("backdown");
        yield return new WaitForSeconds(1.3f);
        p.LookAt(bossAppearPos.position, 0.3f);
    }

    public void Start_Fight()
    {
        PlayerUnit.player.SetIsCanControl(true);
        narration_boss.StartFight();
        CameraManager.Instance.SetTarget(PlayerUnit.player.transform, 0.5f);
        isBossStart = true;
        if (coBoss != null)
            StopCoroutine(coBoss);
        coBoss = CoBoss();
        StartCoroutine(coBoss);
        if (coMusic2 != null)
            StopCoroutine(coMusic2);
        coMusic2 = CoMusic2(0.1f);
        StartCoroutine(coMusic2);
    }



    private IEnumerator coBoss = null;
    private IEnumerator CoBoss()
    {
        float t = 0.1f;
        WaitForSeconds wait = new WaitForSeconds(t);
        int correctCount = 0;
        int incorrectCount = 0;
        float startCount = 20.0f;
        float accCount = startCount;
        coreTargetColor = SwtCoreType.Yellow;
        while (true)
        {
            if(!isGateOpen)
            {
                //Debug.Log(accCount + ":" + corePower_correct + "/" + corePower_incorrect);
                correctCount = 0;
                incorrectCount = 0;
                foreach (Inter_SwtCore core in swt_cores)
                {
                    if (core.state == coreTargetColor)
                        correctCount += 1;
                    else
                        incorrectCount += 1;
                }
                corePower_correct += correctCount * t;
                corePower_incorrect += incorrectCount * t;
                //corePower_correct += 100;
                accCount -= t;
                if(accCount <= 0)
                {
                    accCount = startCount;
                    if (corePower_correct >= corePower_incorrect)
                        Progress(true);
                    else
                        Progress(false);
                    corePower_correct = 0;
                    corePower_incorrect = 0;
                }
                
            }
            yield return wait;
        }
    }

    private void Progress(bool correct)
    {
        bossPhase += 1;
        narration_boss.SetProgress(bossPhase);
        int index = 0;
        
        for(int i = 0; i < progressStates.Length; i++)
        {
            if (progressStates[i] == 0)
            {
                index = i;
                break;
            }
        }
        if (correct)
        {
            progressStates[index] = ProgressType.Success;
            progressRenders[index].material = progressMaterials[(int)ProgressType.Success];
            SoundManager.Instance.StartSound("SE_Core_ColorChange", 0.5f);
        }
        else
        {
            progressStates[index] = ProgressType.Failure;
            progressRenders[index].material = progressMaterials[(int)ProgressType.Failure];
            SoundManager.Instance.StartSound("SE_Core_ColorChange_2", 0.5f);

        }

        if(index == progressStates.Length - 1)
        {
            bool isPerfect = true;
            for (int i = 0; i < progressStates.Length; i++)
            {
                if (progressStates[i] == ProgressType.Failure)
                {
                    isPerfect = false;
                }
            }
            Success_Boss(isPerfect);
        }
        else
        {
            int rand = Random.Range(2, 6);
            if((SwtCoreType)rand == coreTargetColor)
            {
                rand += 1;
                if (rand == 6)
                    rand = 2;
            }
            SetCoreColor((SwtCoreType)rand);       
        }
    }

    

    private void Stop_Boss()
    {
        //Debug.Log("Stop");
        narration_boss.PerfectEnd();
        bossTraps.SetActive(false);

        if (!isBossStart)
            return;
        isBossStart = false;
        if (coBoss != null)
            StopCoroutine(coBoss);
        sound_core.SwtSound(false);
        StopMusic();
    }

    private void Fail_Boss()
    {
        //Debug.Log("Fail");
        Stop_Boss();
        foreach (Inter_SwtCore core in swt_cores)
        {
            core.SetSwt(SwtCoreType.Black);
        }
        SetCoreColor(SwtCoreType.Yellow);
        SetCorePower(1.0f);
        gate.CloseForce();
        for(int i = 0; i < progressStates.Length; i++)
        {
            progressStates[i] = ProgressType.Off;
            progressRenders[i].material = progressMaterials[(int)ProgressType.Off];
        }

        //PlayerUnit.player.SetPos(failedPos.position);
    }

    private void Success_Boss(bool isPerfect)
    {
        Debug.Log("Success_Boss: " + isPerfect);
        isGateOpen = true;
        gate.Open();
        narration_boss.DisAppear();
        isPerfectSuccess = isPerfect;
    }

    public void End_Boss()
    {
        PlayerUnit.player.IsShield = true;
        isEndBoss = true;
        isCanExit = false;
        Stop_Boss();
        sound_core.SwtSound(false);
        //Event_NormalEndBoss();
        //return;
        if (isPerfectSuccess)
            Event_PerfectEndBoss();
        else
            Event_NormalEndBoss();
    }

    private void Event_NormalEndBoss()
    {
        trueObj.SetActive(false);
        normalObj.SetActive(true);
        PlayerUnit.player.IsShield = true;
        isEndBoss = true;
        isCanExit = false;

        PlayerUnit p = PlayerUnit.player;
        p.SetIsInGrass(true);
        camTarget.position = p.transform.position;
        CameraManager.Instance.SetDistance(7, 1.0f);
        CameraManager.Instance.SetTarget(camTarget);
        camTarget.DOLocalMove(new Vector3(37.0f, 0, 10.0f), 1.0f);
        p.SetIsCanControl(false);
        p.SetDestination(p.transform.position + new Vector3(3.0f, 0, 0));

        StartCoroutine(CoEvent_NormalEndBoss(p));
    }

    private IEnumerator CoEvent_NormalEndBoss(PlayerUnit p)
    {
        yield return new WaitForSeconds(1.5f);
        p.transform.DORotate(new Vector3(0, 270, 0), 1.0f);
        SetCorePower(50.0f, 100.0f, 5.0f);
        SetCoreColor(SwtCoreType.Yellow, 1.0f);
        AudioSource audio = SoundManager.Instance.StartSound("SE_Core_Ex_Loop", 0.5f);
        yield return new WaitForSeconds(1.5f);
        SetCoreColor(SwtCoreType.Explode, 3.0f);
        LightManager.Instance.SetLightIntencity(50.0f, 4.5f, Ease.InExpo);
        UIManager.Instance.FadeIn(4.5f, Color.white, null);
        SoundManager.Instance.StartSound("SE_Fade_Start", 0.8f);
        yield return new WaitForSeconds(2.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_FailEnd");
        narration_event.gameObject.SetActive(true);
        narration_event.position = narration_broken.position + new Vector3(0, 20, 0);
        narration_event.DOMove(narration_broken.position + new Vector3(0, 0.5f, 0), 0.6f);
        yield return new WaitForSeconds(2.6f);
        SoundManager.Instance.StopSound(audio, 1.0f);
        //SoundManager.Instance.StopSound(audio2);
        obj_bossRoom.SetActive(false);
        obj_bossRoom_broken.SetActive(true);
        narration_event.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.StartSound("SE_Explosion_1", 0.8f);
        yield return new WaitForSeconds(0.8f);
        SoundManager.Instance.StartSound("SE_Explosion_1", 0.8f);
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.StartSound("SE_Rock_Explosion", 0.8f);
        yield return new WaitForSeconds(1.4f);
        LightManager.Instance.SetLightIntencity(1.0f, 1.5f);
        UIManager.Instance.FadeOut(1.5f, null);


        yield return new WaitForSeconds(3.0f);
        camTarget.DOMove(p.transform.position, 1.0f);
        yield return new WaitForSeconds(1.0f);
        CameraManager.Instance.SetTarget(p.transform);
        yield return new WaitForSeconds(1.0f);
        p.transform.DORotate(new Vector3(0, 90, 0), 1.0f);
        yield return new WaitForSeconds(1.0f);
        p.SetDestination(p.transform.position + new Vector3(50, 0, 0));
        UIManager.Instance.StartNormalEnd();
    }

    private void Event_PerfectEndBoss()
    {
        normalObj.SetActive(false);
        trueObj.SetActive(true);
        PlayerUnit.player.IsShield = true;
        isEndBoss = true;
        isCanExit = false;

        StartCoroutine(CoEvent_PerfectBoss());
    }

    private IEnumerator CoEvent_PerfectBoss()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        p.SetDestination(p.transform.position + new Vector3(5.0f, 0, 0));
        yield return new WaitForSeconds(1.0f);
        p.transform.DORotate(new Vector3(0, 270, 0), 0.5f);
        yield return new WaitForSeconds(0.3f);
        ani_shield.gameObject.SetActive(true);
        //ani_shield.SetTrigger("Appear");
        ani_shield.SetTrigger("TrueAppear");
        obj_narration_shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        p.SetIsCanControl(true);
        yield return new WaitForSeconds(1.0f);
        DialogManager.Instance.ShowDialog("Dialog_Senario016_TrueShield");
    }

    public void Event_FrontWall()
    {
        obj_narration_shield.SetActive(false);
    }

    [SerializeField] private GameObject sound_TVnoise;
    [SerializeField] private GameObject sound_shake;
    public void Event_BrokenWall()
    {
        normalObj.SetActive(false);
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        UIManager.Instance.FadeIn(2.0f, Color.white, null);
        Time.timeScale = 0.5f;
        StartCoroutine(CoEvent_BrokenWall(p));
    }

    private IEnumerator CoEvent_BrokenWall(PlayerUnit p)
    {
        yield return new WaitForSeconds(4.0f);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(4.0f);
        spaceObj.SetActive(true);
        spaceNoiseObj.SetActive(true);
        sound_TVnoise.SetActive(true);
        UIManager.Instance.FadeOut(2.0f, null);
        p.SetIsCanControl(true);
        yield return new WaitForSeconds(4.0f);
        sound_shake.SetActive(true);
        CameraManager.Instance.Shake(1000000, 0.2f, 30);
    }

    [SerializeField] private GameObject interactText;
    public void Event_Blackhole()
    {
        GameManager.Instance.GetStage(1017).gameObject.SetActive(true);
        PlayerUnit p = PlayerUnit.player;
        p.SetIsCanControl(false);
        p.transform.DORotate(new Vector3(0, 90, 0), 1.0f);
        blackhole.Open();
        StartCoroutine(CoEvent_Blackhole(p));
    }

    private IEnumerator CoEvent_Blackhole(PlayerUnit p)
    {
        yield return new WaitForSeconds(3.0f);
        p.SetDestination(p.transform.position + new Vector3(-10.0f, 0, 0));
        yield return new WaitForSeconds(1.0f);
        p.SetSpeed(1.0f, 10.0f);
        StartBlackhole();
        interactText.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        detector_blackholeEscape.gameObject.SetActive(true);
    }


    private bool isBlackhole = false;
    private void StartBlackhole()
    {
        isBlackhole = true;
        if(coBlackhole != null)
            StopCoroutine(coBlackhole);
        coBlackhole = CoBlackhole();
        StartCoroutine(coBlackhole);
    }

    private IEnumerator coBlackhole = null;
    private float flowSpeed = 1.0f;
    private float maxFlowSpeed = 3.0f;
    private IEnumerator CoBlackhole()
    {
        Vector3 dest;
        Vector3 flowAngle = new Vector3(1, 0, 0);
        
        float time = 0;
        WaitForSeconds wait = new WaitForSeconds(0.01f);
        yield return wait;
        PlayerUnit player = PlayerUnit.player;
        while (true)
        {
            dest = player.GetDest();
            player.SwtAgent(false);
            float delta = Time.time - time;
            if (delta >= 0.1f)
                delta = 0.1f;
            player.transform.position += flowAngle * delta * flowSpeed;
            player.SwtAgent(true);
            player.SetDestination(dest);
            time = Time.time;
            yield return wait;
        }
    }

    private void StopBlackhole()
    {
        isBlackhole = false;
        if (coBlackhole != null)
            StopCoroutine(coBlackhole);
    }

    [SerializeField] private Transform detector_blackholeEscape;
    [SerializeField] private GroundFall groundFall;
    public void Event_BlackholeFall()
    {
        isBlackhole = false;
        PlayerUnit p = PlayerUnit.player;
        interactText.SetActive(false);
        p.SetIsCanControl(false);
        narration_event.position =  p.transform.position + new Vector3(2, 15, 0);
        narration_event.rotation = Quaternion.Euler(0,0,0);
        narration_event.gameObject.SetActive(true);
        p.SetForceRun(true);
        narration_event.DOMoveY(detector_blackholeEscape.position.y, 0.5f).OnComplete(() =>
        {
            StopBlackhole();
            SoundManager.Instance.StartSound("SE_Rock_Explosion", 1.0f);
            Time.timeScale = 0.5f;
            p.SwtAgent(false);
            p.GetAnimator().SetTrigger("fall");
            p.GetAnimator().SetBool("isfall", true);
            groundFall.Broken();
            narration_event.DOMove(new Vector3(narration_event.position.x + 2, p.transform.position.y - 80.0f + 1.5f, narration_event.position.z), 2.5f).OnComplete(() =>
            {

            }).SetEase(Ease.Linear);
            p.transform.DOMoveY(p.transform.position.y + 1.0f, 0.5f).OnComplete( () =>
            {
                StartCoroutine(CoEvent_BlackholeFall1(p));
            }).SetEase(Ease.OutCubic);
            sound_shake.SetActive(false);
            CameraManager.Instance.StopShake();
        }).SetEase(Ease.Linear);
    }

    private IEnumerator CoEvent_BlackholeFall1(PlayerUnit p)
    {
        p.transform.DOMoveY(p.transform.position.y - 80.0f, 2.0f).OnComplete(() =>
        {
            StartCoroutine(CoEvent_BlackholeFall2(p));
        }).SetEase(Ease.Linear);
        sound_TVnoise.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 1.0f;
        blackhole.StopSound();
    }

    private IEnumerator CoEvent_BlackholeFall2(PlayerUnit p)
    {
        p.Shield(true);
        Vector3 vec = narration_event.position - p.transform.position;
        p.transform.position = lastStage.GetStartPos().position + new Vector3(0, 40, 0);
        narration_event.position = p.transform.position + vec;
        p.transform.DOMove(p.transform.position - new Vector3(0, 40, 0), 2.0f).SetEase(Ease.Linear);
        narration_event.DOMove(narration_event.position - new Vector3(0, 40, 0), 2.0f).SetEase(Ease.Linear);
        p.SetForceRun(false);
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.FadeIn(0, Color.black, null);
        yield return new WaitForSeconds(0.2f);
        p.GetAnimator().SetBool("isfall", false);
        narration_event.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        p.GetAnimator().SetTrigger("lie1");
        p.transform.DORotate(new Vector3(0, 90, 0), 1.0f);
        p.Shield(false);
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
        lastStage.SetStage();
        UIManager.Instance.FadeOut(3.0f, null);
    }

    private void Update()
    {
        if(isBlackhole)
        {
            if (flowSpeed > maxFlowSpeed)
            {
                flowSpeed = maxFlowSpeed;
            }
            else
            {
                flowSpeed += Time.deltaTime * 8.0f;
            }
            if (Input.GetKeyDown(MyKey.Interact))
            {
                flowSpeed = 1.0f;
                SoundManager.Instance.StartSound("UI_Key_Click", 1.0f);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T");
            Event_BlackholeFall();
        }
    }

    public override void Hint()
    {
        if (hintIndex == 0)
            UIManager.Toast(Language.Instance.Get("Hint_1016_1"));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Speed_Force_Loop", 0.2f, true);
    }

    private IEnumerator coMusic2 = null;
    private IEnumerator CoMusic2(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Spirit_Of_The_Warrior_Loop", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        if (coMusic2 != null)
            StopCoroutine(coMusic2);
        SoundManager.Instance.StopMusic(0.5f);
    }
}

