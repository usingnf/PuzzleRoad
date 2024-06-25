using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_GridTrap : MonoBehaviour
{
    public Boss_GridTrapGroup group;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Transform fillTrans;
    [SerializeField] private GameObject outLine;
    [SerializeField] private Collider coll;
    [SerializeField] private float damageDelay;

    public int x;
    public int y;

    private bool inRange = false;
    // Start is called before the first frame update

    public void StartShoot(float t, bool isSound = true)
    {
        fillTrans.gameObject.SetActive(true);
        outLine.SetActive(true);
        fillTrans.localScale = new Vector3(0,1,0);
        fillTrans.DOScale(new Vector3(1, 1, 1), t).OnComplete(() =>
        {
            Shoot(isSound);
        }).SetEase(Ease.Linear);
    }

    public void Shoot(bool isSound = true)
    {
        particle.Play();
        if(gameObject.activeInHierarchy)
            StartCoroutine(CoDetect());
        fillTrans.gameObject.SetActive(false);
        outLine.SetActive(false);
        if(isSound)
        {
            if (this.gameObject.activeInHierarchy)
            {
                SoundManager.Instance.DelayStartSound("SE_Lazer_Attack", 0.4f, 0.05f);
                SoundManager.Instance.DelayStartSound("SE_Explosion_1", 0.7f, 0.05f);
            }
        }
    }

    private IEnumerator CoDetect()
    {
        yield return new WaitForSeconds(damageDelay);
        if (inRange && !PlayerUnit.player.IsShield)
            group.DetectPlayer();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            group.playerX = x;
            group.playerY = y;
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
