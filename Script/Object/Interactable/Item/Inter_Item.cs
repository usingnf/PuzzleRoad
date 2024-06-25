using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_Item : InteractableObject
{
    [SerializeField] private SO_Item item;
    [SerializeField] private int count = 1;

    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        Inventory.Instance.AddItem(item, count);
        Destroy(this.gameObject);
    }
}
