using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowColor
{
    None = -1,
    Red = 0,
    Green = 1,
    Blue = 2,
}

public enum ArrowAngle
{
    Up,
    Down,
    Left,
    Right,
}
public class Obj_ArrowTile : MonoBehaviour
{
    [SerializeField] private Senario016 senario16;
    [SerializeField] private ArrowColor colorType;
    [SerializeField] private ArrowAngle angleType;
    [SerializeField] private SpriteRenderer render;

    public void SetColor(ArrowColor c)
    {
        if (c == ArrowColor.None)
            render.color = Color.black;
        else if (c == ArrowColor.Red)
            render.color = Color.red;
        else if (c == ArrowColor.Green)
            render.color = Color.green;
        else if (c == ArrowColor.Blue)
            render.color = Color.blue;
    }

    public void SetColor()
    {
        if (colorType == ArrowColor.None)
            render.color = Color.black;
        else if (colorType == ArrowColor.Red)
            render.color = Color.red;
        else if (colorType == ArrowColor.Green)
            render.color = Color.green;
        else if (colorType == ArrowColor.Blue)
            render.color = Color.blue;
    }

    public ArrowColor GetColor()
    {
        return colorType;
    }

    public ArrowAngle GetAngle()
    {
        return angleType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            senario16.EnterArrow(this);
        }
    }

}
