using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_FallingCube : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    private bool isReady = false;
    private float lastTime = 0;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        isReady = true;
        yield return new WaitForSeconds(7.0f);
        if (this.transform.position.y < -50)
            this.gameObject.SetActive(false);
        isReady = false;
        this.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isReady)
            return;
        string tag = collision.gameObject.tag;
        if(tag == "Wall")
        {
            if (lastTime + 0.5f > Time.time)
                return;
            if(rigid.velocity.magnitude > 0.4f)
            {
                lastTime = Time.time;
                float v = Mathf.Lerp(0, 1, rigid.velocity.magnitude);
                SoundManager.Instance.StartSound("SE_Obj_Drop", v);
            }
        }
        else if(tag == "Ground")
        {
            if (rigid.velocity.magnitude > 0.1f)
            {
                float v = Mathf.Lerp(0, 1, rigid.velocity.magnitude);
                SoundManager.Instance.StartSound("SE_Obj_Drop", v);
            }
            this.enabled = false;
        }
    }
}
