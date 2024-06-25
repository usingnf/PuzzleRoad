using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door_Elevator : Door
{
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    private bool process = false;

    public override void Open()
    {
        if (process)
            return;
        if (isOpen)
            return;
        if (!left.activeSelf || !right.activeSelf)
            return;

        process = true;
        left.transform.DOScaleX(0, 0.5f).OnComplete(() =>
        {
            left.SetActive(false);
            process = false;
            isOpen = true;
        });
        right.transform.DOScaleX(0, 0.5f).OnComplete(() =>
        {
            right.SetActive(false);
        });
        
        if (openFunc != null)
            openFunc.Invoke();
    }

    public override void Close()
    {
        if (process)
            return;
        if (!isOpen)
            return;
        if (left.activeSelf || right.activeSelf)
            return;
        process = true;
        left.SetActive(true);
        right.SetActive(true);
        left.transform.DOScaleX(1.25f, 0.5f).OnComplete(() =>
        {
            isOpen = false;
            process = false;
        });
        right.transform.DOScaleX(1.25f, 0.5f);
        
        if (closeFunc != null)
            closeFunc.Invoke();
    }

    public override void Lock(bool b)
    {
        isLock = b;
    }
}
