using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage002 : Stage
{
    public GameObject[] redObj;
    public GameObject[] greenObj;
    public GameObject[] blueObj;
    public GameObject[] yellowObj;

    public GameObject[] lockObj;

    protected void Start()
    {
        base.Start();
        SetQuestion();
    }

    public override void ReStage()
    {
        base.ReStage();
        SetQuestion();
    }
    public void SetQuestion()
    {
 
    }

    public void SwtReverse()
    {
        foreach (GameObject obj in redObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        foreach (GameObject obj in greenObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        foreach (GameObject obj in blueObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
        foreach (GameObject obj in yellowObj)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void SwtColor(Obj_RGBSwt.RGB rgb, bool b)
    {
        if(rgb == Obj_RGBSwt.RGB.Red)
        {
            foreach(GameObject obj in redObj)
            {
                obj.SetActive(b);
            }
        }
        else if(rgb == Obj_RGBSwt.RGB.Green)
        {
            foreach (GameObject obj in greenObj)
            {
                obj.SetActive(b);
            }
        }
        else if(rgb == Obj_RGBSwt.RGB.Blue)
        {
            foreach (GameObject obj in blueObj)
            {
                obj.SetActive(b);
            }
        }
        else if (rgb == Obj_RGBSwt.RGB.Yellow)
        {
            foreach (GameObject obj in yellowObj)
            {
                obj.SetActive(b);
            }
        }
    }

    public void OpenLock(int index)
    {
        lockObj[index].SetActive(false);
    }
}
