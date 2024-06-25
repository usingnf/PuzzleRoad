using UnityEngine;
using UnityEngine.UI;

public class ButtonThreshold : MonoBehaviour
{
    [SerializeField] private Image img;
    void Start()
    {
        img.alphaHitTestMinimumThreshold = 0.5f;
    }
}
