using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Boss_RockTrap : MonoBehaviour
{
    public Boss_RockTrapGroup group;
    [SerializeField] private Transform rock;
    [SerializeField] private ParticleSystem range;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private GameObject debri;
    [SerializeField] private Rigidbody[] debris;
    [SerializeField] private NavMeshObstacle obtacle;
    private bool inRange = false;
    // Start is called before the first frame update

    public void Falling(float t)
    {
        range.Play();
        rock.localPosition = new Vector3(0, 20, 0);

        rock.DOLocalMoveY(0, t).OnComplete(() =>
        {
            effect.gameObject.SetActive(true);
            effect.Play();
            rock.gameObject.SetActive(false);
            debri.SetActive(true);
            foreach(Rigidbody rigid in debris)
            {
                rigid.AddForce(new Vector3 (Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10, ForceMode.Impulse);
            }
            CameraManager.Instance.Shake(3.0f, 1, 50);
            Destroy(this.gameObject, 3.0f);
            obtacle.enabled = true;
            range.gameObject.SetActive(false);
            if(inRange)
                group.Kill();
            SoundManager.Instance.StartSound("SE_Rock_Explosion", 1.0f);
            SoundManager.Instance.StartSound("SE_Rock_Start", 1.0f);
        }).SetEase(Ease.Linear);
        SoundManager.Instance.StartSound("SE_Rock_Fall", 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
}
