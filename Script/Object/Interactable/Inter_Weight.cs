using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Inter_Weight : InteractableObject
{
    [SerializeField] private Stage001 stage001;
    private Transform holdUnit;
    private bool isHold = false;
    public int weight = 1;
    public bool isSlow = false;

    [SerializeField] private Quaternion angle;
    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0);

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
            if(Physics.Raycast(this.transform.position, new Vector3(0,-1,0), out RaycastHit hit, 10.0f, LayerMask.GetMask("Ground")))
            {
                Vector3 vec = this.transform.position;
                this.transform.position = new Vector3(vec.x, hit.point.y, vec.z);
            }
            if (isSlow)
                u.SetSpeed(2, true);
            PlayerUnit p = u as PlayerUnit;
            if(p != null)
            {
                p.SetHoldObj(null);
            }
            SoundManager.Instance.StartSound("SE_Obj_Drop", 1.0f);
        }
        else
        {
            holdUnit = u.transform;
            //this.transform.parent = holdUnit;
            this.transform.position = holdUnit.position + offset + holdUnit.forward * 0.65f;
            Swt_Interact(false);
            isHold = true;
            if (isSlow)
                u.SetSpeed(-2, true);
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
            {
                p.SetHoldObj(this);
            }
            stage001.AddHoldCount();
            SoundManager.Instance.StartSound("SE_Obj_Hold", 1.0f);
        }
    }

    public void SetWeight(int weight, bool b = false)
    {
        this.weight = weight;
        isSlow = b;
    }

    private void LateUpdate()
    {
        if (holdUnit != null)
        {
            this.transform.position = holdUnit.position + offset + holdUnit.forward * 0.65f;
            this.transform.rotation = angle;
        }
    }
}
