using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Divide_CheckAngle : MonoBehaviour
{
    public Transform red;
    public Transform green;
    public Transform blue;
    public SpriteRenderer render;
    private Material mat;

    public Vector3 start;
    public Vector3 targetVec;
    public Vector3 vec;

    [SerializeField] private float lastGreen;
    [SerializeField] private float lastBlue;
    [SerializeField] private float lastGreenRad;
    [SerializeField] private float lastBlueRad;
    [SerializeField] private float black1 = 0;
    [SerializeField] private float black2 = 0;
    [SerializeField] private float black3 = 0;

    private float rand1;
    private float rand2;
    private float rand3;
    public bool isUpdate = false;
    private float temp = 0;
    void Start()
    {
        mat = render.material;
        rand1 = Random.Range(0, 360);
        rand2 = Random.Range(0, 360);
        rand3 = Random.Range(0, 360);
        mat.SetFloat("_Black1", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUpdate)
            return;
        vec = red.position - this.transform.position;
        vec.y = 0;
        vec = vec.normalized;
        mat.SetFloat("_Rotate1", Mathf.Atan2(vec.z, vec.x));

        vec = green.position - this.transform.position;
        vec.y = 0;
        vec = vec.normalized;


        lastGreenRad = Mathf.Atan2(vec.z, vec.x);
        temp = lastGreenRad * Mathf.Rad2Deg;
        if (temp < 0)
            temp += 360;
        
        if(temp != lastGreen && Mathf.Abs(temp - lastGreen) < 120)
            black2 += (temp - lastGreen) * Time.deltaTime * 0.015f;
        if (black2 < 0)
            black2 = 0;
        else if (black2 > 1)
            black2 = 1;
        lastGreen = temp;
        mat.SetFloat("_Rotate2", lastGreenRad +rand2);
        mat.SetFloat("_Black2", black2);

        

        vec = blue.position - this.transform.position;
        vec.y = 0;
        vec = vec.normalized;

        lastBlueRad = Mathf.Atan2(vec.z, vec.x);
        temp = lastBlueRad * Mathf.Rad2Deg;
        if (temp < 0)
            temp += 360;
        if (temp != lastBlue && Mathf.Abs(temp - lastBlue) < 120)
            black3 += (lastBlue - temp) * Time.deltaTime * 0.015f;
        if (black3 < 0)
            black3 = 0;
        else if (black3 > 1)
            black3 = 1;

        lastBlue = temp;
        mat.SetFloat("_Rotate3", lastBlueRad + rand3);
        mat.SetFloat("_Black3", black3);

        mat.SetFloat("_Black1", 1);
        //mat.SetFloat("_Black2", 1);
        //mat.SetFloat("_Black3", 1);
    }

    public void Init()
    {
        black1 = 0;
        black2 = 0;
        lastBlue = 0;
        lastBlueRad = 0;
        lastGreen = 0;
        lastGreenRad = 0;
    }
}
