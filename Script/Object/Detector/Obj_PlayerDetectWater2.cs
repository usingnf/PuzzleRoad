using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_PlayerDetectWater2 : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AutoSoundVolume auto;
    private bool isFirstEnter = false;
    private TweenerCore<float, float, FloatOptions> tween = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.enabled = true;
            audioSource.volume = 0;
            if (tween != null)
                tween.Kill();
            tween = audioSource.DOFade(auto.GetFinalVolume(), 1.0f);
            if (!isFirstEnter)
            {
                isFirstEnter = true;
                if (DialogManager.Exist())
                    DialogManager.Instance.ShowDialog("Dialog_Senario007_FirstEnter");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tween != null)
                tween.Kill();
            tween = audioSource.DOFade(0, 1.0f).OnComplete(()=> { audioSource.enabled = false; });
            
        }
    }
}
