using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obj_PlayerDetectWater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUnit.player.SetIsInWater(true);
            SoundManager.Instance.StartSound("SE_Water_InStart", 0.25f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUnit.player.SetIsInWater(false);
        }
    }
}
