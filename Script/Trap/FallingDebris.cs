using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform trans;
    
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 angle;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    void Start()
    {
        trans = transform;
        angle = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        trans.Rotate(angle * 1.0f);
    }

    public void Falling(Vector3 startPos)
    {
        if (gameObject.activeSelf)
            return;
        transform.position = startPos;
        this.gameObject.SetActive(true);
        transform.DOMove(startPos + new Vector3(0, -distance, 0), speed * Random.Range(0.8f, 1.2f)).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        }).SetEase(Ease.Linear);
    }
}
