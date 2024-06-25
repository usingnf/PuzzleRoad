using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSide : MonoBehaviour
{
    [SerializeField] private MeshRenderer render;
    [SerializeField] private Material mat;
    [SerializeField] private float speed = 1.0f;

    private void OnEnable()
    {
        mat = render.material;
        StartCoroutine(CoWhile());
    }

    private IEnumerator CoWhile()
    {
        float intensity = mat.GetFloat("_Float");
        while(true)
        {
            intensity += Time.deltaTime * speed;
            if (intensity > 10000)
                intensity = 0;
            mat.SetFloat("_Float", intensity);
            yield return null;
        }
    }

}
