using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDebris : MonoBehaviour
{
    public Transform debris;

    private bool isBroken = false;
    private int layer;
    private void Start()
    {
        layer = LayerMask.NameToLayer("Ground");
    }
    public void Broken()
    {
        if (isBroken)
            return;
        isBroken = true;
        debris.gameObject.SetActive(true);
        debris.parent = this.transform.parent;
        foreach(Transform trans in debris)
        {
            trans.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 4.0f, ForceMode.Impulse);
        }
        Destroy(debris.gameObject, 3.0f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == layer)
        {
            Broken();
        }
    }
}
