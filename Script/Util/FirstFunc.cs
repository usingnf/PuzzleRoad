using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstFunc : MonoBehaviour
{
    public UnityEvent func;
    void Start()
    {
        if(func != null) 
            func.Invoke();
    }
}
