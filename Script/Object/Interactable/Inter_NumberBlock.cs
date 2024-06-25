using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inter_NumberBlock : InteractableObject
{
    private Transform holdUnit;
    private bool isHold = false;
    [SerializeField] private Quaternion angle;
    [SerializeField] private Stage011 stage011;
    [SerializeField] private TextMeshPro text;
    public int number;

    private void Start()
    {
        angle = transform.rotation;
        //SetColor();
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
            stage011.SetTile(this);
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
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
            stage011.HoldOutObj(this);
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
            {
                p.SetHoldObj(this);
                SoundManager.Instance.StartSound("SE_Obj_Hold", 1.0f);
            }
        }
    }

    public void SetNumber(int number)
    {
        this.number = number;
        text.text = number.ToString();
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
