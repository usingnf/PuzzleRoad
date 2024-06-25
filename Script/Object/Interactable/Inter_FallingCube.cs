using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inter_FallingCube : InteractableObject
{
    [SerializeField] private Transform parent;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Collider parentColl;
    [SerializeField] private NavMeshObstacle obstacle;
    
    private Transform holdUnit;
    private bool isHold = false;

    public override void Interact(Unit u)
    {
        if (!isCanInteract)
            return;
        if (isHold)
        {
            holdUnit = null;
            isHold = false;
            if (Physics.Raycast(parent.position, new Vector3(0, -1, 0), out RaycastHit hit, 10.0f, LayerMask.GetMask("Ground")))
            {
                Vector3 vec = parent.position;
                parent.position = new Vector3(vec.x, hit.point.y + 0.5f, vec.z);
            }
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
            {
                p.SetHoldObj(null);
                SoundManager.Instance.StartSound("SE_Obj_Drop", 1.0f);
            }

            rigid.useGravity = true;
            obstacle.enabled = true;
            parentColl.enabled = true;
        }
        else
        {
            rigid.useGravity = false;
            obstacle.enabled = false;
            parentColl.enabled = false;

            holdUnit = u.transform;
            Swt_Interact(false);
            isHold = true;
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
            parent.position = holdUnit.position + new Vector3(0, 0.65f, 0) + holdUnit.forward * 0.85f;
            parent.rotation = Quaternion.identity;
        }
    }
}
