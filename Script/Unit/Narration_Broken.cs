using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narration_Broken : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] [ColorUsage(true, true)] private Color onColor;
    [SerializeField] [ColorUsage(true, true)] private Color offColor;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject spark;

    private bool isActive = false;
    public void Action(bool b)
    {
        isActive = b;
        if(b)
        {
            spark.SetActive(true);
            mat.DOColor(onColor, "_EmissionColor", Random.Range(0.5f, 2.0f)).OnComplete(() =>
            {
                mat.DOColor(offColor, "_EmissionColor", Random.Range(0.5f, 2.0f)).OnComplete(() =>
                {
                    if (isActive)
                    {
                        Action(true);
                    }
                }).SetEase(Ease.Linear);
            }).SetEase(Ease.Linear);
        }
        else
        {
            spark.SetActive(false);
        }
    }

    public void StartSound()
    {
        audioSource.volume = SoundManager.Instance.GetSoundVolume() * 0.5f;
        audioSource.Play();
        SoundLoop();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void SoundLoop(bool b = true)
    {
        float v = SoundManager.Instance.GetSoundVolume();
        if (b)
            v = v * 0.08f;
        else
            v = v * 0.04f;
        audioSource.DOFade(v, 0.5f).OnComplete(() =>
        {
            if(this.gameObject.activeSelf)
                SoundLoop(!b);
        });
    }
}
