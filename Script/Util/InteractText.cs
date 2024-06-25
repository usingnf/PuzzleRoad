using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private void OnEnable()
    {
        text.text = MyKey.Interact.ToString();
    }
}
