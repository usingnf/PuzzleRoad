using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Light를 관리하는 클래스.
/// 빛과 관련된 연출 기능.
/// </summary>

public class LightManager : Singleton<LightManager>
{
    public enum LightState
    {
        Bright,
        LittleDark,
        Dark,
        PerfectDark,
    }

    // dirction:ambient:reflect
    // Bright       1 / 1 / 1
    // Little Dark  0.3 / 0.5 / 1
    // Dark         0 / 0.06 / 0.2
    // Perfect Dark 0 / 0 / 0

    [Header("Inspector")]
    [SerializeField] private Light directLight;
    [SerializeField] private LightState lightState = LightState.Bright;
    [SerializeField] private Light warningLight;
    private UnityAction<LightState> func;

    private bool isWarning = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLightState(LightState state)
    {
        lightState = state;
        if (state == LightState.Bright)
        {
            directLight.intensity = 1.0f;
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
        }
        else if(state == LightState.LittleDark)
        {
            directLight.intensity = 0.3f;
            RenderSettings.ambientIntensity = 0.5f;
            RenderSettings.reflectionIntensity = 1.0f;
        }
        else if(lightState == LightState.Dark)
        {
            directLight.intensity = 0.0f;
            RenderSettings.ambientIntensity = 0.06f;
            RenderSettings.reflectionIntensity = 0.2f;
        }
        else if(lightState == LightState.PerfectDark)
        {
            directLight.intensity = 0.0f;
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
        }
        if(func != null)
            func.Invoke(lightState);
    }

    public LightState GetLightState()
    {
        return lightState;
    }

    public void SetLightIntencity(float power, float time, Ease ease = Ease.Linear)
    {
        directLight.DOIntensity(power, time).SetEase(ease);
    }

    public void AddLightFunc(UnityAction<LightState> func)
    {
        this.func += func;
    }

    public void RemoveLightFunc(UnityAction<LightState> func)
    {
        if(this.func != null)
            this.func -= func;
    }


    //빛을 활용한 경고 연출.
    public void StartWarning()
    {
        warningLight.gameObject.SetActive(true);
        if(coWarning != null)
            StopCoroutine(coWarning);
        coWarning = CoWarning();
        StartCoroutine(coWarning);
        //warningAudio = SoundManager.Instance.StartSoundFadeLoop("SE_Warning_Loop2", 0.5f, 0, 0, 100000);
    }

    public void StopWarning()
    {
        warningLight.gameObject.SetActive(false);
        if (coWarning != null)
            StopCoroutine(coWarning);
        if(warningAudio != null)
        {
            SoundManager.Instance.StopSound(warningAudio);
            warningAudio = null;
        }
    }

    private AudioSource warningAudio = null;
    private IEnumerator coWarning = null;
    private IEnumerator CoWarning()
    {
        bool swt = true;
        float onSpeed = 3;
        float outSpeed = 1;
        float intensity = 0;
        SoundManager.Instance.StartSound("SE_Warning_Loop", 0.18f);
        while (true)
        {
            if(swt)
            {
                intensity += Time.deltaTime * onSpeed;
                if(intensity > 1)
                {
                    intensity = 1;
                    swt = false;
                }
            }
            else
            {
                intensity += -Time.deltaTime * outSpeed;
                if(intensity < 0)
                {
                    intensity = 0;
                    swt = true;
                    //SoundManager.Instance.StartSoundFadeLoop("SE_Warning_Loop", 0.5f, 0, 0, 0);
                    SoundManager.Instance.StartSound("SE_Warning_Loop", 0.18f);
                }
            }
            warningLight.intensity = intensity;
            yield return null;
        }
    }
}
