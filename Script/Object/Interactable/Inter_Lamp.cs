using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LightManager;

public class Inter_Lamp : InteractableObject
{
    [SerializeField] private bool isAutoLightOn = false;
    [SerializeField] private bool isAutoLightOff = false;
    [SerializeField] private bool isLight = false;
    [SerializeField] private GameObject lightObj;

    private void Start()
    {
        LightManager.Instance.AddLightFunc(OnLightChange);
    }
    private void OnDestroy()
    {
        if(LightManager.Exist())
            LightManager.Instance.RemoveLightFunc(OnLightChange);
    }
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        if (isLight)
        {
            LightOff();
        }
        else
        {
            LightOn();
        }
    }

    public void LightOn(bool isSound = true)
    {
        if (isLight)
            return;
        lightObj.SetActive(true);
        isLight = true;
        if(isSound)
            SoundManager.Instance.StartSound("SE_Lamp_Switch", 1.0f);
    }

    public void LightOff(bool isSound = true)
    {
        if (!isLight)
            return;
        lightObj.SetActive(false);
        isLight = false;
        if (isSound)
            SoundManager.Instance.StartSound("SE_Lamp_Switch", 1.0f);
    }

    public bool GetIsLight()
    {
        return isLight;
    }

    private void OnLightChange(LightState state)
    {
        if(state == LightState.Bright)
        {
            if (isAutoLightOff)
                LightOff(false);
        }
        else
        {
            if (isAutoLightOn)
                LightOn(false);
        }
    }
}
