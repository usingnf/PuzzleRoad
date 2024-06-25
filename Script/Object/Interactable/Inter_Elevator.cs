using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inter_Elevator : InteractableObject
{
    public Elevator elevator;
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        if (elevator != null)
            elevator.Btn_Excute(u);
    }
}
