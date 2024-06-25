using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Balance : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;
    [SerializeField] private Button btn_close;
    [SerializeField] private MyText msg;

    private void Start()
    {
        btn_close.onClick.AddListener(Close);
    }
    public void SetWeight(float left, float right)
    {
        if(left == right)
        {
            arrow.localRotation = Quaternion.Euler(0f, 0f, 0f);
            msg.SetText("Stage001_Weight_Same", true);
            return;
        }
        else if(left == 0)
        {
            arrow.localRotation = Quaternion.Euler(0f, 0f, -60f);
            msg.SetText("Stage001_Weight_Right", true);
            return;
        }
        else if(right == 0)
        {
            arrow.localRotation = Quaternion.Euler(0f, 0f, 60f);
            msg.SetText("Stage001_Weight_Left", true);
            return;
        }
        float max = Mathf.Max(left, right);
        float min = Mathf.Min(left, right);
        float angle = (max / min) - 1;
        if (angle > 1)
            angle = 1;
        angle *= 60;
        if(left > right)
        {
            arrow.localRotation = Quaternion.Euler(0f, 0f, angle);
            msg.SetText("Stage001_Weight_Left", true);
        }
        else
        {
            arrow.localRotation = Quaternion.Euler(0f, 0f, -angle);
            msg.SetText("Stage001_Weight_Right", true);
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
