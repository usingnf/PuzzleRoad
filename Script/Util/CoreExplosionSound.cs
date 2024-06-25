using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreExplosionSound : MonoBehaviour
{
    [SerializeField] private AudioSource audio1;
    [SerializeField] private AudioSource audio2;
    public float volume1;
    public float time1;
    public float spaceTime;
    public float volume2;
    public float time2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Stop();
            Play();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Stop();
        }
    }

    public void Play()
    {
        StartCoroutine(CoPlay());
    }

    private IEnumerator CoPlay()
    {
        SoundManager.Instance.StartSoundFadeLoop("SE_Core_Ex_Start", volume1, time1, 0.5f, 2.0f);
        yield return new WaitForSeconds(spaceTime);
        SoundManager.Instance.StartSoundFadeLoop("SE_Core_Ex_Loop", volume2, time2, 0.1f, 4.0f);
    }

    public void Stop()
    {
        if(audio1 != null)
            SoundManager.Instance.StopSound(audio1);
        if (audio2 != null)
            SoundManager.Instance.StopSound(audio2);
    }

}
