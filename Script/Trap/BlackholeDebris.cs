using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeDebris : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 8.0f;
    [SerializeField] private float speed = 2.0f;

    private Vector3 targetPos;
    private Transform trans;
    
    void Start()
    {
        trans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        trans.Rotate((targetPos - trans.position).normalized * 1.0f);
    }

    public void Shoot()
    {
        if (gameObject.activeSelf)
            return;
        targetPos = target.position;
        transform.position = targetPos + (target.forward * distance) + (new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized * distance * 0.5f);
        this.gameObject.SetActive(true);
        transform.DOMove(targetPos, speed * Random.Range(0.9f, 1.1f)).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        }).SetEase(Ease.Linear);
    }
}
