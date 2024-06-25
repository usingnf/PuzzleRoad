using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Player의 유닛을 감지하여 특정 함수를 실행하는 기능.
/// </summary>
public class Obj_PlayerDetectFunc : MonoBehaviour
{
    public UnityEvent func;
    public UnityEvent outFunc;
    public bool isOnce = false;
    [SerializeField] protected MeshRenderer render;
    [SerializeField] private Material mat;
    [SerializeField] private Color color;
    [SerializeField][Range(0, 10.0f)] private float speed = 5.0f;
    [SerializeField][Range(0, 1.0f)] private float intencity = 1.0f;
    [SerializeField][Range(0, 1.0f)] private float spaceTime = 0.2f;

    private void Start()
    {
        if(render != null)
            mat = render.material;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(func != null)
                func.Invoke();
            if(isOnce)
                this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (outFunc != null)
                outFunc.Invoke();
        }
    }

    public void Active(Color color)
    {
        if (mat == null)
            return;
        this.color = color;
        if (coActive != null)
            StopCoroutine(coActive);
        coActive = CoActive();
        StartCoroutine(coActive);
    }

    private IEnumerator coActive = null;
    private IEnumerator CoActive()
    {
        bool b = true;
        float _intencity = 0;
        WaitForSeconds wait = new WaitForSeconds(spaceTime);
        mat.SetColor("_ActiveColor", color);
        mat.SetInt("_IsActive", 1);
        while (true)
        {
            if (b)
            {
                _intencity += Time.deltaTime * speed;
            }
            else
            {
                _intencity += -Time.deltaTime * speed;
            }
            if (_intencity >= intencity)
            {
                b = false;
                _intencity = intencity;
                mat.SetFloat("_Intencity", _intencity);
                yield return wait;
            }
            else if (_intencity <= 0)
            {
                b = true;
                _intencity = 0;
                mat.SetFloat("_Intencity", _intencity);
                mat.SetInt("_IsActive", 0);
                yield break;
            }
            else
            {
                mat.SetFloat("_Intencity", _intencity);
            }
            yield return null;
        }
    }

    public void SetEndColor()
    {
        mat.SetColor("_Color", Color.black);
        mat.SetInt("_IsActive", 0);
    }
}
