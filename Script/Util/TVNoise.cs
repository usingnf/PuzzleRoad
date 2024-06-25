using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVNoise : MonoBehaviour
{
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Material mat;
    //[SerializeField] [Range(0, 0.5f)] private float t = 0.1f;
    [SerializeField] [Range(0, 1.0f)] private float start_step = 1.0f;
    [SerializeField] [Range(0, 1.0f)] private float end_step = 0.0f;
    [SerializeField] [Range(0, 1.0f)] private float last_step = 1.0f;
    [SerializeField] private float time_step = 1.0f;
    [SerializeField] private float delay_step = 1.0f;
    [SerializeField] private GameObject target;
    [SerializeField] private bool isLoop = false;
    [SerializeField] private bool isTargetSwt = false;
    private WaitForSeconds wait = new WaitForSeconds(0.05f);

    [SerializeField] private bool isSound = false;
    private AudioSource audioSource;

    private void OnEnable()
    {
        mat = render.material;
        StartCoroutine(CoWhile());
        mat.SetFloat("_Step", start_step);
        mat.DOFloat(end_step, "_Step", time_step).OnComplete(() =>
        {
            StartCoroutine(CoWait());
        });

        if (isSound)
            audioSource = SoundManager.Instance.StartSoundFadeLoop("SE_TVNoise_Loop", 0.5f, 0.1f, 0.1f, delay_step + time_step * 2);
    }

    private IEnumerator CoWait()
    {
        yield return new WaitForSeconds(delay_step);
        if (target != null)
            target.SetActive(isTargetSwt);
        mat.DOFloat(last_step, "_Step", time_step).OnComplete(() =>
        {
            if(!isLoop)
                this.gameObject.SetActive(false);
        });
    }

    private IEnumerator CoWhile()
    {
        while(true)
        {
            mat.SetFloat("_Pos", Random.Range(0.0f, 1.0f));
            yield return wait;
        }
    }

    private void OnDisable()
    {
        if(audioSource != null)
        {
            SoundManager.Instance.StopSound(audioSource);
            audioSource = null;
        }
    }


}
