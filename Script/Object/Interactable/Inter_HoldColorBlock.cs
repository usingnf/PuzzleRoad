using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inter_HoldColorBlock : InteractableObject
{
    private Transform holdUnit;
    private bool isHold = false;
    [SerializeField] private Quaternion angle;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.0f, 0);
    public ColorBlock myColor;
    public UnityEvent holdFunc;
    public UnityEvent holdoutFunc;

    private void Start()
    {
        angle = transform.rotation; 
        //SetColor();
    }

    public void SetColorValue(ColorBlock newColor)
    {
        myColor = newColor;
        Color c = Color.black;
        if (myColor == ColorBlock.Black)
            c = Color.black;
        else if (myColor == ColorBlock.Red)
            c = Color.red;
        else if (myColor == ColorBlock.Green)
            c = Color.green;
        else if (myColor == ColorBlock.Yellow)
            c = Color.yellow;
        else if (myColor == ColorBlock.Blue)
            c = Color.blue;
        else if (myColor == ColorBlock.Purple)
            c = new Color(1, 0, 1);
        else if (myColor == ColorBlock.Emerald)
            c = Color.cyan;
        else if (myColor == ColorBlock.White)
            c = Color.white;
        else
            c = Color.white;
        //mat.SetColor("_Color", c);
        SetColor(c);
    }

    public void HoldOut()
    {
        if (isHold)
            Interact(PlayerUnit.player);
    }

    public override void Interact(Unit u)
    {
        if (!isCanInteract)
            return;
        if (isHold)
        {
            holdUnit = null;
            isHold = false;
            if (Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out RaycastHit hit, 10.0f, LayerMask.GetMask("Ground")))
            {
                Vector3 vec = this.transform.position;
                this.transform.position = new Vector3(vec.x, hit.point.y + 0.5f, vec.z);
            }
            else
            {
                Vector3 vec = u.transform.position;
                this.transform.position = new Vector3(vec.x, vec.y + 0.5f, vec.z);
            }
            //stage008.SetTile(this);
            if (holdoutFunc != null)
                holdoutFunc.Invoke();
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
                p.SetHoldObj(null);
        }
        else
        {
            holdUnit = u.transform;
            Swt_Interact(false);
            isHold = true;
            if (holdFunc != null)
                holdFunc.Invoke();
            //stage008.HoldOutObj(this);
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
                p.SetHoldObj(this);
        }
    }

    private void LateUpdate()
    {
        if (holdUnit != null)
        {
            this.transform.position = holdUnit.position + offset + holdUnit.forward * 0.85f;
            this.transform.rotation = angle;
        }
    }

    public void RemoveHoldOutFunc()
    {
        holdoutFunc = null;
    }
}
