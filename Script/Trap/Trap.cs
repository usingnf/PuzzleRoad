using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trap : MonoBehaviour
{
    public TrapGroup group;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Transform fillTrans;
    [SerializeField] private Collider coll;
    [SerializeField] private float damageDelay;

    [SerializeField] private int repeatIndex;
    [SerializeField] private int startIndex;

    public int currentIndex;
    private bool inRange = false;

    public void ResetTime()
    {
        currentIndex = startIndex;
    }

    public bool CheckShoot()
    {
        float t = (float)(currentIndex % (repeatIndex)) / (float)repeatIndex;
        fillTrans.DOScale(new Vector3(t, 1, t), 0.1f);
        if (currentIndex % repeatIndex == 0)
        {
            Shoot();
            return true;
        }
        return false;
    }

    public virtual void Shoot()
    {
        particle.Play();
        StartCoroutine(CoDetect());
    }

    private IEnumerator CoDetect()
    {
        yield return new WaitForSeconds(damageDelay);
        if(inRange && !PlayerUnit.player.IsShield)
            group.DetectPlayer();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void OnDisable()
    {
        inRange = false;
    }
}
