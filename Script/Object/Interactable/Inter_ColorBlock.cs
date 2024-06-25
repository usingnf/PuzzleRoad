using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_ColorBlock : InteractableObject
{
    private Transform holdUnit;
    private bool isHold = false;
    [SerializeField] private Quaternion angle;
    [SerializeField] private Stage008 stage008;
    public ColorBlock myColor;

    private void Start()
    {
        angle = transform.rotation;
        //SetColor();
    }

    public void SetColor(ColorBlock newColor)
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
            Interact(null);
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
            stage008.SetTile(this);
            PlayerUnit p = u as PlayerUnit;
            if(p != null)
            {
                p.SetHoldObj(null);
                SoundManager.Instance.StartSound("SE_Obj_Drop", 1.0f);
            }
        }
        else
        {
            holdUnit = u.transform;
            Swt_Interact(false);
            isHold = true;
            stage008.HoldOutObj(this);
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
            {
                p.SetHoldObj(this);
                SoundManager.Instance.StartSound("SE_Obj_Hold", 1.0f);
            }
        }
    }

    private void LateUpdate()
    {
        if (holdUnit != null)
        {
            this.transform.position = holdUnit.position + new Vector3(0, 0.85f, 0) + holdUnit.forward * 0.85f;
            this.transform.rotation = angle;
        }
    }
}
