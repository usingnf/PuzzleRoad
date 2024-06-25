using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Obj_KeyWay : MonoBehaviour
{
    public enum WayAngle
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
    }

    public GameObject[] walls;
    public bool[] isOpen;
    public int x;
    public int y;

    private void OnEnable()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            isOpen[i] = !walls[i].activeSelf;
        }
    }

    private void Start()
    {
        
    }

    public void OpenWay(WayAngle angle)
    {
        walls[(int)angle].gameObject.SetActive(false);
    }

    public void CloseWay(WayAngle angle)
    {
        walls[(int)angle].gameObject.SetActive(true);
    }
}
