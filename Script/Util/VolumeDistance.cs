using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeDistance : MonoBehaviour
{
    public float defaultVolume = 1.0f;
    public AudioSource audioSource;
    public Transform target;
    public float maxDistance = 10.0f;

    private bool isFadeEnd = true;
    private void Start()
    {
        if (SoundManager.Exist())
        {
            SoundManager.Instance.AddSoundFunc(ChangeVolume);
        }
        StartCoroutine(CoCheckDistance());
        target = PlayerUnit.player.transform;
    }

    private IEnumerator CoCheckDistance()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while(true)
        {
            if(isFadeEnd)
                ChangeVolume(SoundManager.Instance.GetSoundVolume());
            yield return wait;
        }
    }
    private void OnEnable()
    {
        if (SoundManager.Exist())
        {
            audioSource.volume = defaultVolume * GetDistance() * SoundManager.Instance.GetSoundVolume();
        }
    }

    private void OnDestroy()
    {
        if (SoundManager.Exist())
        {
            SoundManager.Instance.RemoveSoundFunc(ChangeVolume);
        }
    }

    private void ChangeVolume(float volume)
    {
        audioSource.volume = defaultVolume * GetDistance() * volume;
    }

    public float GetFinalVolume()
    {
        return defaultVolume * GetDistance() * SoundManager.Instance.GetSoundVolume();
    }

    public float GetDistance()
    {
        if (target == null)
            target = PlayerUnit.player.transform;
        float dis = (this.transform.position - target.position).magnitude;
        if (dis >= maxDistance)
        {
            //this.gameObject.SetActive(false);
            audioSource.enabled = false;
            return 0.0f;
        }
        else
        {
            audioSource.enabled = true;
        }
        return Mathf.Pow(1 - (dis / maxDistance), 3);
    }

    public void Fade(float t)
    {
        isFadeEnd = false;
        audioSource.volume = 0;
        audioSource.DOFade(defaultVolume * 0.4f * SoundManager.Instance.GetSoundVolume(), t).OnComplete(()=> { isFadeEnd = true; });
    }
}
