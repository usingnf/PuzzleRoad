using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inter_SwtFunc : InteractableObject
{
    public UnityEvent func;
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        if(func != null )
            func.Invoke();
    }
}
