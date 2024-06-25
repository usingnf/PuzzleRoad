using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowWater : MonoBehaviour
{
    [SerializeField] private bool inRange;
    [SerializeField] private Vector3 flowAngle;
    [SerializeField] private float flowSpeed;
    [SerializeField] private float maxFlowSpeed;

    private float lastSwimTime = 0;

    private void OnEnable()
    {
        StartCoroutine(CoFlow());
        flowSpeed = 1.0f;
    }
    private void Update()
    {
        if(inRange)
        {
            if (flowSpeed > maxFlowSpeed)
            {
                flowSpeed = maxFlowSpeed;
            }
            else
            {
                flowSpeed += Time.deltaTime * 12.0f;
            }
            if (Input.GetKeyDown(MyKey.Interact))
            {
                flowSpeed = 0.5f;
                lastSwimTime = Time.time;
                SoundManager.Instance.StartSound("SE_Water_Swim", 0.5f);
            }
        }
    }

    private IEnumerator CoFlow()
    {
        Vector3 dest;
        float time = 0;
        WaitForSeconds wait = new WaitForSeconds(0.01f);
        yield return wait;
        PlayerUnit player = PlayerUnit.player;
        flowSpeed = 0.5f;
        while (true)
        {
            if (inRange)
            {
                if(lastSwimTime + 0.12f < Time.time)
                {
                    dest = player.GetDest();
                    player.SwtAgent(false);
                    float delta = Time.time - time;
                    if (delta >= 0.1f)
                        delta = 0.1f;
                    player.transform.position += flowAngle * delta * flowSpeed;
                    player.SwtAgent(true);
                    player.SetDestination(dest);
                }
                time = Time.time;
            }
            yield return wait;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerUnit.player.SetIsInWater(true);
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUnit.player.SetIsInWater(false);
            inRange = false;
        }
    }
}
