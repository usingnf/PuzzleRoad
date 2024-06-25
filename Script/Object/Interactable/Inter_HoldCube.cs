using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inter_HoldCube : InteractableObject
{
    private Transform holdUnit;
    private bool isHold = false;
    [SerializeField] private Quaternion angle;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.0f, 0);
    public UnityEvent holdFunc;
    public UnityEvent holdoutFunc;
    // Start is called before the first frame update
    private void Start()
    {
        angle = transform.rotation;
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
            //stage008.SetTile(this);
            if (holdoutFunc != null)
                holdoutFunc.Invoke();
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
            if (holdFunc != null)
                holdFunc.Invoke();
            //stage008.HoldOutObj(this);
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
            this.transform.position = holdUnit.position + offset + holdUnit.forward * 0.85f;
            this.transform.rotation = angle;
        }
    }
}
