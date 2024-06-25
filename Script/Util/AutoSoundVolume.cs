using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSoundVolume : MonoBehaviour
{
    public float defaultVolume = 1.0f;
    public AudioSource audioSource;
    private void Start()
    {
        if (SoundManager.Exist())
        {
            SoundManager.Instance.AddSoundFunc(ChangeVolume);
        }
    }
    private void OnEnable()
    {
        if (SoundManager.Exist())
        {
            audioSource.volume = defaultVolume * SoundManager.Instance.GetSoundVolume();
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
        audioSource.volume = defaultVolume * volume;
    }

    public float GetFinalVolume()
    {
        return defaultVolume * SoundManager.Instance.GetSoundVolume();
    }

    public void SetVolume(float volume, float fadeTime = 0.0f)
    {
        defaultVolume = volume;
        if (fadeTime > 0.0f)
            audioSource.DOFade(defaultVolume * SoundManager.Instance.GetSoundVolume(), fadeTime);
        else
            audioSource.volume = defaultVolume * SoundManager.Instance.GetSoundVolume();
    }

    public void SwtSound(bool b)
    {
        audioSource.enabled = b;
    }
}
