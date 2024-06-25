using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : InteractableObject
{
    [SerializeField] private Room room;
    [SerializeField] private Transform door;
    [SerializeField] private bool isPull = false;
    [SerializeField] protected bool isLock = false;
    [SerializeField] protected bool isOpen = false;
    [SerializeField] protected UnityEvent doorFunc;
    [SerializeField] protected UnityEvent openFunc;
    [SerializeField] protected UnityEvent closeFunc;
    [SerializeField] private bool isLockSound = true;

    public bool IsOpen { get { return isOpen; } }

    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        if (isLock)
        {
            SoundManager.Instance.StartSound("SE_Door_FailOpen", 0.6f);
            return;
        }
        if(isOpen)
        {
            Close();
            //door.localRotation = Quaternion.Euler(0, 0, 0);
            //isOpen = false;
        }
        else
        {
            Open();
            //if(isPull)
            //{
            //    door.localRotation = Quaternion.Euler(0, -90, 0);
            //}
            //else
            //{
            //    door.localRotation = Quaternion.Euler(0, 90, 0);
            //}
            //isOpen = true;
        }
        if (doorFunc != null)
            doorFunc.Invoke();
    }

    public override void Swt_Interact(bool b)
    {
        base.Swt_Interact(b);
    }

    public virtual void Lock(bool b)
    {
        if (isLock != b) 
        {
            if(b)
            {
                if(isLockSound)
                {
                    if (GameManager.Exist())
                    {
                        if (GameManager.GetState() == GameState.Start)
                            if (SoundManager.Exist())
                                SoundManager.Instance.StartSound("SE_Door_Lock", 1.0f);
                    }
                    else
                    {
                        if(SoundManager.Exist())
                            SoundManager.Instance.StartSound("SE_Door_Lock", 1.0f);
                    }
                }
            }
            else
            {
                if (isLockSound)
                {
                    if (GameManager.Exist())
                    {
                        if (GameManager.GetState() == GameState.Start)
                            if (SoundManager.Exist())
                                SoundManager.Instance.StartSound("SE_Door_Unlock", 1.0f);
                    }
                    else
                    {
                        if (SoundManager.Exist())
                            SoundManager.Instance.StartSound("SE_Door_Unlock", 1.0f);
                    }
                }
            }
        }
        
        isLock = b;
    }

    public virtual void Open()
    {
        if (isOpen)
            return;
        if (isPull)
        {
            door.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            door.localRotation = Quaternion.Euler(0, 90, 0);
        }
        isOpen = true;
        if(openFunc != null)
            openFunc.Invoke();
    }

    public virtual void Close()
    {
        if (!isOpen)
            return;
        door.localRotation = Quaternion.Euler(0, 0, 0);
        isOpen = false;
        if(closeFunc != null)
            closeFunc.Invoke();
    }

    public void DoorSwt(bool open)
    {
        if(open)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void SwtLockSound(bool b)
    {
        isLockSound = b;
    }
}
