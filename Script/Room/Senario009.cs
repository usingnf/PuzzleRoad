using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Senario009 : Stage
{
    [SerializeField] private Transform detector9;
    [SerializeField] private Transform detector9_dest;
    [SerializeField] private Transform detector1;
    [SerializeField] private Transform detector1_dest;
    [SerializeField] private Transform detector3;
    [SerializeField] private Transform detector3_dest;
    [SerializeField] private Transform detector7;
    [SerializeField] private Transform detector7_dest;

    [SerializeField] private GameObject exitRoute;
    [SerializeField] private GameObject pageTable9;
    [SerializeField] private GameObject detectorSecond9;
    [SerializeField] private GameObject navmeshWall;
    [SerializeField] private Inter_Elevator btn_endElevator;

    [SerializeField] private Inter_Lamp[] lamps;
    [SerializeField] private Light[] lights;

    [SerializeField] private UI_Page page1;
    [SerializeField] private UI_Page page3;
    [SerializeField] private UI_Page page7;
    [SerializeField] private UI_Page page9;

    private bool isSecond9 = false;
    private bool isOpenRoute = false;
    private bool isEnd = false;

    private float intensity = 0;
    [SerializeField] private float maxIntensity = 3.0f;
    [SerializeField] private float intensitySpeed = 0.1f;

    private int loopCount = 0;

    protected void Start()
    {
        base.Start();
        SetQuestion();
        btn_endElevator.AddInterFunc(ElevatorOn);
        PlayerUnit.player.Add_LightEvent(CheckLight);
    }
    public override void ReStage()
    {
        base.ReStage();
        //PlayerUnit.player.SetPos(startPos.position);
        SetQuestion();
    }
    public void SetQuestion()
    {
        page1.Clear();
        page3.Clear() ;
        page7.Clear() ;
        page9.Clear() ;

        page1.SetTitle("1.");
        page1.AddContent("Senario009_Content_1_1", true);
        page1.AddContent("Senario009_Content_1_2", true);
        page1.AddContent("Senario009_Content_1_3", true);
        page3.SetTitle("2.");
        page3.AddContent("Senario009_Content_2_1", true);
        page3.AddContent("Senario009_Content_2_2", true);
        page3.AddContent("Senario009_Content_2_3", true);
        page7.SetTitle("3.");
        page7.AddContent("Senario009_Content_3_1", true);
        page7.AddContent("Senario009_Content_3_2", true);
        page7.AddContent("Senario009_Content_3_3", true);
        page9.SetTitle("4.");
        page9.AddContent("Senario009_Content_4_1", true);
        page9.AddContent("Senario009_Content_4_2", true);
        page9.AddContent("Senario009_Content_4_3", true);
    }

    private void Update()
    {
        if(isOpenRoute)
        {
            intensity += Time.deltaTime * intensitySpeed;
            if(intensity > maxIntensity)
                intensity = maxIntensity;
            foreach(Light light in lights)
            {
                light.intensity = intensity;
            }
        }
        else
        {
            intensity -= Time.deltaTime * intensitySpeed * 2;
            if (intensity < 0)
                intensity = 0;
        }
    }

    public void EnterDectector9()
    {
        PlayerUnit player = PlayerUnit.player;
        Vector3 dest = PlayerUnit.player.GetDest();
        PlayerUnit.player.SetPos(player.transform.position - detector9.position + detector9_dest.position);
        dest = (dest - detector9.position) + detector9_dest.position;
        PlayerUnit.player.SetDestination(dest);
        loopCount += 1;
        if (loopCount == 6)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop");
        }
        else if (loopCount == 13)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop_2");
        }
    }

    public void EnterDectector7()
    {
        PlayerUnit player = PlayerUnit.player;
        Vector3 dest = PlayerUnit.player.GetDest();
        PlayerUnit.player.SetPos(player.transform.position - detector7.position + detector7_dest.position);
        dest = (dest - detector7.position) + detector7_dest.position;
        PlayerUnit.player.SetDestination(dest);
        loopCount += 1;
        if (loopCount == 6)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop");
        }
        else if (loopCount == 13)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop_2");
        }
    }

    public void EnterDectector3()
    {
        PlayerUnit player = PlayerUnit.player;
        Vector3 dest = PlayerUnit.player.GetDest();
        PlayerUnit.player.SetPos(player.transform.position - detector3.position + detector3_dest.position);
        dest = (dest - detector3.position) + detector3_dest.position;
        PlayerUnit.player.SetDestination(dest);
        loopCount += 1;
        if (loopCount == 6)
        {
            if (DialogManager.Exist())
               DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop");
        }
        else if (loopCount == 13)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop_2");
        }
    }

    public void EnterDectector1()
    {
        PlayerUnit player = PlayerUnit.player;
        Vector3 dest = PlayerUnit.player.GetDest();
        PlayerUnit.player.SetPos(player.transform.position - detector1.position + detector1_dest.position);
        dest = (dest - detector1.position) + detector1_dest.position;
        PlayerUnit.player.SetDestination(dest);
        loopCount += 1;
        if (loopCount == 6)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop");
        }
        else if(loopCount == 13)
        {
            if (DialogManager.Exist())
                DialogManager.Instance.ShowDialog("Dialog_Senario009_Loop_2");
        }
    }

    public void EnterDectectorCenter()
    {
        detector1.parent.gameObject.SetActive(true);
        hintIndex = 1;
    }

    public void EnterDectectorSecond()
    {
        Event_Light();
        hintIndex = 2;
    }

    public void EnterDetectorSecond9()
    {
        isSecond9 = true;
    }

    public void ExitDetectorSecond9()
    {
        isSecond9 = false;
        CheckLight(PlayerUnit.player.GetIsLight());
    }

    private void Event_Light()
    {
        DialogManager.Instance.StopDialog();
        PlayerUnit.player.SetIsCanControl(false);

        LightManager.Instance.SetLightState(LightManager.LightState.LittleDark);
        
        StartCoroutine(CoEvent_Light());
        if(SoundManager.Exist())
        {
            SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);
            StopMusic();
        }
    }

    private IEnumerator CoEvent_Light()
    {
        yield return new WaitForSeconds(0.1f);
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
        yield return new WaitForSeconds(0.4f);
        LightManager.Instance.SetLightState(LightManager.LightState.LittleDark);
        SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);
        yield return new WaitForSeconds(0.1f);
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
        yield return new WaitForSeconds(0.4f);
        LightManager.Instance.SetLightState(LightManager.LightState.LittleDark);
        SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);
        yield return new WaitForSeconds(0.1f);
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
        yield return new WaitForSeconds(0.8f);
        LightManager.Instance.SetLightState(LightManager.LightState.Dark);
        SoundManager.Instance.StartSound("SE_Light_Off", 0.6f);

        if (UIManager.Exist())
            UIManager.Toast(MyText.ConvertText(Language.Instance.Get("UI_Toast_LightTutorial"), MyKey.Light.ToString()));

        PlayerUnit.player.SetIsCanControl(true);
        yield return new WaitForSeconds(2.0f);
        float t = DialogManager.Instance.ShowDialog("Dialog_Senario009_LightOff");
        if (coMusic2 != null)
            StopCoroutine(coMusic2);
        coMusic2 = CoMusic2(t + 0.5f);
        StartCoroutine(coMusic2);
    }

    public void Event_FirstEnter()
    {
        DialogManager.Instance.ShowDialog("Dialog_Senario009_FirstEnter");
    }

    public void Event_FirstEnterMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        coMusic = CoMusic(0.1f);
        StartCoroutine(coMusic);
    }

    
    public void CheckLight(bool b)
    {
        if (!this.gameObject.activeSelf)
            return;
        if (b)
        {
            TurnOffRoute();
            return;
        }
        else
        {
            StartCoroutine(CoCheck());
        }
        
    }

    private IEnumerator CoCheck()
    {
        yield return null;
        if (isSecond9)
            yield break;
        if (PlayerUnit.player.GetIsLight())
        {
            TurnOffRoute();
            yield break;
        }
        if (LightManager.Instance.GetLightState() == LightManager.LightState.Bright)
        {
            TurnOffRoute();
            yield break;
        }
        foreach (Inter_Lamp lamp in lamps)
        {
            if (lamp.GetIsLight())
            {
                TurnOffRoute();
                yield break;
            }
        }
        TurnOnRoute();
    }

    public void TurnOnRoute()
    {
        isOpenRoute = true;
        foreach(Light light in lights)
        {
            light.gameObject.SetActive(true);
        }
        pageTable9.SetActive(false);
        exitRoute.SetActive(true);
        navmeshWall.SetActive(false);
        
        //detectorSecond9.SetActive(false);
    }

    

    public void TurnOffRoute()
    {
        isOpenRoute = false;
        foreach (Light light in lights)
        {
            light.gameObject.SetActive(false);
        }
        if (!isSecond9 && !isEnd)
        {
            exitRoute.SetActive(false);
            pageTable9.SetActive(true);
            navmeshWall.SetActive(true);
        }
    }

    public void ElevatorOn()
    {
        isEnd = true;
        LightManager.Instance.SetLightState(LightManager.LightState.Bright);
        PlayerUnit.player.Swt_Light(false);
        SoundManager.Instance.StartSound("SE_Light_On", 0.6f);
    }

    public void EndStage()
    {
        isEnd = true;
        if (DialogManager.Exist())
            DialogManager.Instance.ShowDialog("Dialog_Senario009_End");
        StopMusic();
    }

    public override void Hint()
    {
        if (hintIndex == 1)
            UIManager.Toast(Language.Instance.Get("Hint_1009_1"));
        else if(hintIndex == 2)
            UIManager.Toast(Language.Instance.Get("Hint_1009_2"));
        else
            base.Hint();
    }

    private IEnumerator coMusic = null;
    private IEnumerator CoMusic(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Simple_and_Bright_Advertising_Loop1", 0.2f, true);
    }
    private IEnumerator coMusic2 = null;
    private IEnumerator CoMusic2(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartMusic("Music_Halloween_Trap", 0.2f, true);
    }

    public void StopMusic()
    {
        if (coMusic != null)
            StopCoroutine(coMusic);
        if(coMusic2 != null)
            StopCoroutine(coMusic2);
        SoundManager.Instance.StopMusic(0.5f);
    }
}
