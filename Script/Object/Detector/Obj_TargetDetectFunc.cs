using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Obj_TargetDetectFunc : MonoBehaviour
{
    public GameObject[] target;
    public UnityEvent func;
    public UnityEvent outFunc;
    private int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        bool b = false;
        if (target == null)
            return;
        if (target.Length == 0)
            return;
        foreach (GameObject obj in target)
        {
            if(obj == other.gameObject)
            {
                b = true;
                break;
            }
        }
        if (b)
        {
            count += 1;
            Check();
            //if (func != null)
            //    func.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool b = false;
        if (target == null)
            return;
        if (target.Length == 0)
            return;
        foreach (GameObject obj in target)
        {
            if (obj == other.gameObject)
            {
                b = true;
                break;
            }
        }
        if (b)
        {
            count += -1;
            Check();
            //if (outFunc != null)
            //    outFunc.Invoke();
        }
    }

    private void Check()
    {
        if(count > 0)
        {
            if (func != null)
                func.Invoke();
        }
        else
        {
            if (outFunc != null)
                outFunc.Invoke();
        }
    }
}
