using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundData
{
    public string soundName;
    public float volume;
}
public class SoundSupport : MonoBehaviour
{
    [SerializeField] private string soundName;
    [SerializeField] [Range(0, 1)] private float volume = 1.0f;
    [SerializeField] private float delayTime = 0;
    [SerializeField] private float trimTime = 0;
    [SerializeField] private float stopTime = 0;
    [SerializeField] private bool checkState = false;
    [SerializeField] private bool isSound = true;

    public void Play()
    {
        if (!isSound)
            return;
        if (string.IsNullOrEmpty(soundName))
            return;
        if (checkState && GameManager.Exist() && GameManager.GetState() != GameState.Start)
            return;
        if (delayTime > 0)
            StartCoroutine(CoPlay());
        else
        {
            if(SoundManager.Exist())
            {
                AudioSource audio = SoundManager.Instance.StartSound(soundName, volume, trimTime);
                if (stopTime > 0)
                    StartCoroutine(CoStop(audio));
            }
        }
    }

    public void PlayString(string str)
    {
        if (!isSound)
            return;
        if (string.IsNullOrEmpty(str))
            return;
        if (checkState && GameManager.Exist() && GameManager.GetState() != GameState.Start)
            return;
        if (delayTime > 0)
            StartCoroutine(CoPlay());
        else
        {
            AudioSource audio = SoundManager.Instance.StartSound(str, volume, trimTime);
            if (stopTime > 0)
                StartCoroutine(CoStop(audio));
        }
    }

    private IEnumerator CoPlay()
    {
        yield return new WaitForSeconds(delayTime);
        AudioSource audio = SoundManager.Instance.StartSound(soundName, volume, trimTime);
        if(stopTime > 0)
            StartCoroutine(CoStop(audio));
    }

    private IEnumerator CoStop(AudioSource audio)
    {
        yield return new WaitForSeconds(stopTime);
        SoundManager.Instance.StopSound(audio);
    }

    public void SetOnOff(bool b)
    {
        isSound = b;
    }
}
