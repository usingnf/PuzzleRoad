using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Obj_RGBSwt;

public class Obj_RGBSwt2 : MonoBehaviour
{
    public Stage0021 stage;
    public RGB state;
    public GameObject[] Obj_rgby;


    private void OnEnable()
    {
        foreach (GameObject obj in Obj_rgby)
        {
            obj.SetActive(false);
        }
        Obj_rgby[(int)state].SetActive(true);
    }

    public void Swt()
    {
        stage.SwtColor(state);
    }
}
