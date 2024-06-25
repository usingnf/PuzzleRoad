using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_SwtObj : InteractableObject
{
    [SerializeField] private GameObject ui_page;
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        ui_page.SetActive(!ui_page.activeSelf);
    }

    protected override void TriggerExitAction()
    {
        base.TriggerExitAction();
        if (ui_page.activeSelf)
            ui_page.SetActive(false);
    }
}
