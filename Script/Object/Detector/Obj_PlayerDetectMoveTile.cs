using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Obj_PlayerDetectMoveTile : MonoBehaviour
{
    public Obj_MoveColorTile tile;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tile.Enter(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tile.Exit(other.transform);
        }
    }
}
