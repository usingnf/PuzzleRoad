using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_MoveColorTile : MonoBehaviour
{
    private float max = 5.0f;
    private float min = 1.0f;
    [SerializeField] private Transform ground;
    [SerializeField] private float pos_progress = 0.0f;
    [SerializeField] private GameObject[] navMin;
    [SerializeField] private GameObject[] navMax;

    //public void SetProgress(float t)
    //{
    //    progress = t;
    //    if(progress < min)
    //        progress = min;
    //    else if(progress > max)
    //        progress = max;
    //    outline.localPosition = outlineOffset + new Vector3(progress * 3 - 9.0f, 0, 0);
    //}

    public void SetLocalPos(float t)
    {
        pos_progress = t;
        if (pos_progress <= min)
        {
            pos_progress = min;
            foreach (GameObject obj in navMin)
                obj.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in navMin)
                obj.SetActive(true);
        }
        if (pos_progress >= max)
        {
            pos_progress = max;
            foreach (GameObject obj in navMax)
                obj.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in navMax)
                obj.SetActive(true);
        }
        ground.localPosition = new Vector3((pos_progress - 1) * 3, 0, 0);
    }

    public void Enter(Transform p)
    {
        p.parent = ground;
    }

    public void Exit(Transform p)
    {
        if (p.parent == ground)
            p.parent = null;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        Transform p = PlayerUnit.player.transform;
    //        if (p.parent == null)
    //            p.parent = ground;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Transform p = PlayerUnit.player.transform;
    //        if (p.parent == ground)
    //            p.parent = null;
    //    }
    //}
}
