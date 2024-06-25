using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_LazerMirror : InteractableObject
{
    private Transform holdUnit;
    private bool isHold = false;
    [SerializeField] private Quaternion angle;

    private void Start()
    {
        angle = transform.rotation;
    }

    public override void Interact(Unit u)
    {
        if (!isCanInteract)
            return;
        if (isHold)
        {
            holdUnit = null;
            isHold = false;
            //this.transform.parent = stage;
            if (Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out RaycastHit hit, 10.0f, LayerMask.GetMask("Ground")))
            {
                Vector3 vec = this.transform.position;
                this.transform.position = new Vector3(vec.x, hit.point.y + 0.5f, vec.z);
            }
            //PlayerUnit p = u as PlayerUnit;
            //if (p != null)
            //{
            //    p.SetHoldObj(null);
            //}
        }
        else
        {
            holdUnit = u.transform;
            //this.transform.parent = holdUnit;
            //this.transform.position = holdUnit.position + new Vector3(0, 0.65f, 0) + holdUnit.forward * 0.65f;
            Swt_Interact(false);
            isHold = true;
            //PlayerUnit p = u as PlayerUnit;
            //if (p != null)
            //{
            //    p.SetHoldObj(this);
            //}
        }
    }

    private void LateUpdate()
    {
        if(holdUnit != null)
        {
            this.transform.position = holdUnit.position + new Vector3(0, 0.65f, 0) + holdUnit.forward * 0.65f;
            this.transform.rotation = angle;
        }
    }

}
