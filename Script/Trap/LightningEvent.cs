using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LightningEvent : MonoBehaviour
{
    [SerializeField] private VisualEffect effect;
    private void OnEnable()
    {
        effect.SendEvent(0);
    }
}
