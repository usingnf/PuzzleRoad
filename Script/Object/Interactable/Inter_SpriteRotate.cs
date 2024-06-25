using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_SpriteRotate : InteractableObject
{
    [SerializeField] private SpriteRenderer spriteRender;
    private bool isSwt = false;
    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        this.transform.localRotation = Quaternion.Euler(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y + 90, this.transform.localRotation.eulerAngles.z);
    }

    public override void Swt_Interact(bool b)
    {
        isSwt = b;
        SwtSprite(b);
    }

    Tweener tween = null;
    private void SwtSprite(bool b)
    {
        if (tween != null)
            tween.Kill();
        if(b)
        {
            tween = spriteRender.DOColor(color, 0.5f).OnComplete(()=>
            {
                if(isSwt)
                    SwtSprite(false);
            });
        }
        else
        {
            tween = spriteRender.DOColor(Color.white, 0.5f).OnComplete(() =>
            {
                if (isSwt)
                    SwtSprite(true);
            });
        }
    }
}
