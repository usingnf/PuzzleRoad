using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSizeFitter : MonoBehaviour
{
    public RectTransform parentRect;
    public RectTransform rect;
    public float maxWidth = 100;
    public LayoutElement layout;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rect.rect.width > parentRect.rect.width)
        {
            layout.enabled = true;
            layout.preferredWidth = parentRect.rect.width;
        }
        else
        {
            layout.enabled = false;
        }
    }
}
