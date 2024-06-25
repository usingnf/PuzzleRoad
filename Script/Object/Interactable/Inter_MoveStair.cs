using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;

public class Inter_MoveStair : InteractableObject
{
    private Transform holdUnit;
    private bool isHold = false;
    [SerializeField] private Quaternion angle;
    [SerializeField] private Transform targetPos;
    [SerializeField] private bool isTarget = false;
    [SerializeField] private NavMeshLink link;

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
            isHold = false;
            if (isTarget)
            {
                this.transform.position = targetPos.position;
                isCanInteract = false;
            }
            else
                this.transform.position = holdUnit.position;
            holdUnit = null;
            //if (Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), out RaycastHit hit, 10.0f, LayerMask.GetMask("Ground")))
            //{
            //    Vector3 vec = this.transform.position;
            //    this.transform.position = new Vector3(vec.x, hit.point.y, vec.z);
            //}
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
            {
                p.SetHoldObj(null);
                SoundManager.Instance.StartSound("SE_Obj_Drop", 1.0f);
            }
            link.enabled = true;
        }
        else
        {
            holdUnit = u.transform;
            Swt_Interact(false);
            isHold = true;
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
            {
                p.SetHoldObj(this);
                SoundManager.Instance.StartSound("SE_Obj_Hold", 1.0f);
            }
            link.enabled = false;
        }
    }

    public void SetIsTarget(bool b)
    {
        isTarget = b;
    }

    private void LateUpdate()
    {
        if (holdUnit != null)
        {
            this.transform.position = holdUnit.position + new Vector3(0, 0.25f, 0) + holdUnit.forward * 0.85f;
            this.transform.rotation = angle;
        }
    }
}
