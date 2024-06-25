using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTrans;
    private Vector3 vec;
    [SerializeField] private float offset = 0.0f;
    void Start()
    {
        if(Camera.main != null)
            camTrans = Camera.main.transform;
        vec = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (camTrans != null)
            vec.y = camTrans.eulerAngles.y + offset;
        else
            camTrans = Camera.main.transform;
        this.transform.eulerAngles = vec;
        //this.transform.LookAt(camTrans);
    }
}
