using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inter_Lever : InteractableObject
{
    public UnityEvent func;
    [SerializeField] private Transform lever;
    [SerializeField] private MeshRenderer render;
    [SerializeField] private Material mat_on;
    [SerializeField] private Material mat_off;

    private bool isCanOnlyOn = true;
    private bool isOn = false;
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        if (isCanOnlyOn && isOn)
            return;
        if (func != null)
            func.Invoke();
        isOn = !isOn;
        LeverMove(isOn);
    }

    public void ForceLever(bool b)
    {
        isOn = b;
        LeverMove(b);
    }
    public void LeverMove(bool b)
    {
        if(b)
        {
            render.material = mat_on;
            lever.localRotation = Quaternion.Euler(0, 0, 45);
        }
        else
        {
            render.material = mat_off;
            lever.localRotation = Quaternion.Euler(0, 0, -45);
        }
    }


}
