using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Stage009;

public class Inter_Note : InteractableObject
{
    private Transform holdUnit;
    private bool isHold = false;
    [SerializeField] private Quaternion angle;
    [SerializeField] private Stage009 stage009;
    public PianoNote myNote;
    public int type;

    private Dictionary<int, float> noteTime = new Dictionary<int, float>(){ 
        { 0, 0.4f}, 
        { 1, 0.4f},
        { 2, 0.4f},
        { 3, 0.1f},
        { 4, 0.4f},
        { 5, 0.4f},
        { 6, 0.4f},
        { 7, 0.4f},
    };

    private void Start()
    {
        angle = transform.rotation;
        //SetColor();
    }

    public void SetNote(PianoNote newNote)
    {
        myNote = newNote;
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
            stage009.SetTile(this);
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
            {
                p.SetHoldObj(null);
                if(SoundManager.Exist())
                    SoundManager.Instance.StartSound("SE_Obj_Drop", 1.0f);
            }
        }
        else
        {
            holdUnit = u.transform;
            Swt_Interact(false);
            isHold = true;
            stage009.HoldOutObj(this);
            PlayerUnit p = u as PlayerUnit;
            if (p != null)
                p.SetHoldObj(this);
            if (SoundManager.Exist())
                SoundManager.Instance.StartSound(this.myNote.ToString(), 1.0f, noteTime[(int)myNote]);
        }
    }

    public void SetType(int type)
    {
        this.type = type;
        if(type == 0)
        {
            //fixed
            SetColor(Color.black);
            //mat.SetColor("_Color", Color.black);
        }
        else if(type == 1) 
        {
            SetColor(Color.red);
            //mat.SetColor("_Color", Color.red);
        }
        else if (type == 2)
        {
            SetColor(Color.green);
            //mat.SetColor("_Color", Color.green);
        }
        else if (type == 3)
        {
            SetColor(Color.blue);
            //mat.SetColor("_Color", Color.blue);
        }
        else if (type == 4)
        {
            SetColor(Color.cyan);
            //mat.SetColor("_Color", Color.cyan);
        }
    }

    private void LateUpdate()
    {
        if (holdUnit != null)
        {
            this.transform.position = holdUnit.position + new Vector3(0, 0.65f, 0) + holdUnit.forward * 0.85f;
            this.transform.rotation = angle;
        }
    }
}
