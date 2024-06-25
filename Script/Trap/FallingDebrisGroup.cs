using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDebrisGroup : MonoBehaviour
{
    [SerializeField] private FallingDebris[] debris;
    [SerializeField] private float timeSpace = 2.0f;
    [SerializeField] private Vector2 range = Vector2.zero;

    private GameObject[] obj_debris;
    private float lastTime = 0;

    void Start()
    {
        obj_debris = new GameObject[debris.Length];
        for (int i = 0; i < debris.Length; i++)
        {
            obj_debris[i] = debris[i].gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lastTime + timeSpace < Time.time)
        {
            Falling();
        }
    }

    private void Falling()
    {
        lastTime = Time.time;
        int index = -1;
        int rand = Random.Range(0, obj_debris.Length);
        for (int i = 0; i < obj_debris.Length; ++i)
        {
            if (obj_debris[(i + rand) % obj_debris.Length].activeSelf)
                continue;
            index = (i + rand) % obj_debris.Length;
            break;
        }
        if (index >= 0)
        {
            debris[index].Falling(this.transform.position + new Vector3(Random.Range(-range.x, range.x), 0, Random.Range(-range.y, range.y)));
        }
    }
}
