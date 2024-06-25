using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDebris2 : MonoBehaviour
{
    private Transform trans;
    private Transform debris;

    [SerializeField] private GameObject prefab_debris;
    [SerializeField] private Transform startPos;
    [SerializeField] private float speed;
    void Start()
    {
        trans = transform;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) 
        {
            Debug.Log("T");
            Falling();
        }
    }

    public void Falling()
    {
        if (debris != null)
            return;
        debris = Instantiate(prefab_debris, startPos.position, Quaternion.identity).transform;
        //debris.DOMove(trans.position + trans.position - startPos.position, speed * 0.5f).OnComplete(() =>
        //{
        //    if (debris != null)
        //        Destroy(debris.gameObject);
        //}).SetEase(Ease.Linear);
        debris.GetComponent<Rigidbody>().AddForce((trans.position - startPos.position).normalized * speed, ForceMode.Impulse);
        Destroy(debris.gameObject, 5.0f);
        StartCoroutine(CoRotate(debris));
    }

    private IEnumerator CoRotate(Transform deb)
    {
        Vector3 ang = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        while (true)
        {
            if (deb == null)
                break;
            deb.Rotate(ang * 1.0f);
            yield return null;
        }
    }
}
