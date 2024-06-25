using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHide : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] renders;
    [SerializeField] private Material[] mats;
    [SerializeField] private BoxCollider coll;
    [SerializeField] private CamAngle angle;

    private void Awake()
    {
        mats = new Material[renders.Length];
        for(int i = 0; i < renders.Length; i++)
        {
            mats[i] = renders[i].material;
        }
    }

    private void Start()
    {
        CameraManager.Instance.action_angle += SetCamAngle;
    }

    public void SetAlpha(float alpha)
    {
        foreach(Material mat in mats)
        {
            mat.SetFloat("_Alpha", alpha);
        }
    }

    public void SetCamAngle(CamAngle ang)
    {
        Vector3 vec = coll.center;
        if(ang == CamAngle._1)
        {
            vec.x = 2.5f;
        }
        else if(ang == CamAngle._9)
        {
            vec.x = -2.5f;
        }
        else if(ang == CamAngle._3)
        {
            if(((int)ang + (int)angle) % 2 == 1)
            {
                vec.x = -2.5f;
            }
            else
            {
                vec.x = 2.5f;
            }
        }
        else if(ang == CamAngle._7)
        {
            if (((int)ang + (int)angle) % 2 == 1)
            {
                vec.x = 2.5f;
            }
            else
            {
                vec.x = -2.5f;
            }
        }
        coll.center = vec;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetAlpha(0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetAlpha(1.0f);
        }
    }
}
