using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_LazerMirror : MonoBehaviour
{
    public int type = 0;
    public bool isEndWall = false;
    public int colorType = 0;

    public Vector3 CalAngle(Vector3 ang)
    {
        if(type == 0)
        {
            if (ang == Vector3.back)
                return Vector3.right;
            else if (ang == Vector3.left)
                return Vector3.forward;
            else
                return Vector3.zero;
        }
        else if(type == 1)
        {
            if (ang == Vector3.forward)
                return Vector3.right;
            else if (ang == Vector3.left)
                return Vector3.back;
            else
                return Vector3.zero;
        }
        else if (type == 2)
        {
            if (ang == Vector3.forward)
                return Vector3.left;
            else if (ang == Vector3.right)
                return Vector3.back;
            else
                return Vector3.zero;
        }
        else if (type == 3)
        {
            if (ang == Vector3.back)
                return Vector3.left;
            else if (ang == Vector3.right)
                return Vector3.forward;
            else
                return Vector3.zero;
        }
        return Vector3.zero;
    }
}
