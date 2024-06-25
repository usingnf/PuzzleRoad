using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_RGBSwt : MonoBehaviour
{
    public enum RGB
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        End = 4,
    }
    public Stage002 stage;
    public RGB state;
    public GameObject[] Obj_rgby;
    public RGB[] rgbyOrder;
    private int order = 0;

    private void OnEnable()
    {
        foreach (GameObject obj in Obj_rgby)
        {
            obj.SetActive(false);
        }
        Obj_rgby[(int)rgbyOrder[order % rgbyOrder.Length]].SetActive(true);
    }
    public void Swt_SetState()
    {
        order += 1;
        if(order - 2 < 0)
            CloseState(rgbyOrder[^1]);
        else
            CloseState(rgbyOrder[(order - 2) % rgbyOrder.Length]);
        StartCoroutine( SetState(rgbyOrder[order % rgbyOrder.Length]));
    }

    public IEnumerator SetState(RGB rgb)
    {
        yield return null;
        foreach (GameObject obj in Obj_rgby)
        {
            obj.SetActive(false);
        }
        Obj_rgby[(int)rgb].SetActive(true);
        stage.SwtColor(state, false);
        state = rgb;
    }

    public void CloseState(RGB rgb)
    {
        stage.SwtColor(rgb, true);
    }
}
