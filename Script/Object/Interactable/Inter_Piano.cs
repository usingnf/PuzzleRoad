using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_Piano : InteractableObject
{
    private bool isPiano = false;
    private AudioSource audioSource = null;
    public override void Interact(Unit u)
    {
        if (!isCanInteract)
            return;
        if (isPiano)
        {
            isPiano = false;
            SoundManager.Instance.StopMusicInterupt();
            if(coPiano != null)
                StopCoroutine(coPiano);
        }
        else
        {
            isPiano = true;
            SoundManager.Instance.StartMusicInterupt("Music_Piano_Heartfelt", 1.0f);
            if (coPiano != null)
                StopCoroutine(coPiano);
            coPiano = CoPiano(SoundManager.Instance.GetAudioMusic().clip.length);
            StartCoroutine(coPiano);
        }
    }

    private IEnumerator coPiano = null;
    private IEnumerator CoPiano(float time)
    {
        yield return new WaitForSeconds(time);
        isPiano = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Swt_Interact(false);
            if (other.TryGetComponent<PlayerUnit>(out PlayerUnit player))
            {
                player.RemoveInteract(this);
            }
            if(isPiano)
            {
                isPiano = false;
                SoundManager.Instance.StopMusicInterupt();
            }
        }
    }
}
