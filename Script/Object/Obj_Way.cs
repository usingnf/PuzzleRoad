using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Way : MonoBehaviour
{

    public enum WayType
    {
        None = 0,
        Straight = 1,
        Curve = 2,
        Three = 3,
        Four = 4,
        Start = 5,
        End = 6,
    }

    public MeshRenderer render;
    private Material mat;
    public bool left = false;
    public bool right = false;
    public bool up = false;
    public bool down = false;
    public WayType wayType = WayType.None;
    public Texture2D[] wayTextures;
    public float angle = 270;

    private void Awake()
    {
        mat = render.material;
    }
    public void SetWayType(WayType type)
    {
        wayType = type;
        if(wayType == WayType.None)
        {
            mat.SetTexture("_MainTex", null);
            mat.SetColor("_Color", Color.white);
        }
        else if(wayType == WayType.Start)
        {
            mat.SetTexture("_MainTex", null);
            mat.SetColor("_Color", Color.green);
        }
        else if (wayType == WayType.End)
        {
            mat.SetTexture("_MainTex", null);
            mat.SetColor("_Color", Color.red);
        }
        else
        {
            mat.SetTexture("_MainTex", wayTextures[(int)type]);
            mat.SetColor("_Color", Color.white);
        }
    }

    public void SetAngle(float angle)
    {
        if (Mathf.Abs(angle - 90) < 3)
            angle = 90;
        else if (Mathf.Abs(angle - 180) < 3)
            angle = 180;
        else if (Mathf.Abs(angle - 270) < 3)
            angle = 270;
        else if (Mathf.Abs(angle - 360) < 3)
            angle = 0;
        else if (Mathf.Abs(angle) < 3)
            angle = 0;
        angle = angle % 360;
        this.angle = angle;
        if(wayType == WayType.Four ||
            wayType == WayType.Start ||
            wayType == WayType.End)
        {
            left = true;
            right = true;
            up = true;
            down = true;
            return;
        }
        if(wayType == WayType.None)
        {
            left = false;
            right = false;
            up = false;
            down = false;
            return;
        }
        if(angle == 0)
        {
            if(wayType == WayType.Straight)
            {
                left = true;
                right = true;
                up = false;
                down = false;
            }
            else if(wayType == WayType.Curve)
            {
                left = true;
                right = false;
                up = true;
                down = false;
            }
            else if(wayType == WayType.Three)
            {
                left = true;
                right = true;
                up = true;
                down = false;
            }
            
        }
        else if(angle == 90)
        {
            if (wayType == WayType.Straight)
            {
                left = false;
                right = false;
                up = true;
                down = true;
            }
            else if (wayType == WayType.Curve)
            {
                left = false;
                right = true;
                up = true;
                down = false;
            }
            else if (wayType == WayType.Three)
            {
                left = false;
                right = true;
                up = true;
                down = true;
            }
        }
        else if (angle == 180)
        {
            if (wayType == WayType.Straight)
            {
                left = true;
                right = true;
                up = false;
                down = false;
            }
            else if (wayType == WayType.Curve)
            {
                left = false;
                right = true;
                up = false;
                down = true;
            }
            else if (wayType == WayType.Three)
            {
                left = true;
                right = true;
                up = false;
                down = true;
            }
        }
        else if (angle == 270)
        {
            if (wayType == WayType.Straight)
            {
                left = false;
                right = false;
                up = true;
                down = true;
            }
            else if (wayType == WayType.Curve)
            {
                left = true;
                right = false;
                up = false;
                down = true;
            }
            else if (wayType == WayType.Three)
            {
                left = true;
                right = false;
                up = true;
                down = true;
            }
        }
    }
}
