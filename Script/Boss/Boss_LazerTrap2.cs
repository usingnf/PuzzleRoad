using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_LazerTrap2 : MonoBehaviour
{
    public Boss_LazerTrapGroup2 group;
    [SerializeField] private Boss_Lazer2 lazer;
    [SerializeField] private Transform head;
    [SerializeField] private GameObject outline;
    [SerializeField] private Transform inline;
    [SerializeField] private Transform arrow;

    private AudioSource audioSource = null;


    public void AttackStart(LazerAngle angle, float time, bool isLeft)
    {
        this.transform.rotation = Quaternion.Euler(0, (int)(angle+1) * 90, 0);
        if (angle == LazerAngle.Left)
        {
            if(isLeft)
                this.transform.localPosition = new Vector3(9, 0, 11);
            else
                this.transform.localPosition = new Vector3(-9, 0, 11);
        }
        else if (angle == LazerAngle.Up)
        {
            if(isLeft)
                this.transform.localPosition = new Vector3(11, 0, -9);
            else
                this.transform.localPosition = new Vector3(11, 0, 9);

        }
        else if (angle == LazerAngle.Right)
        {
            if (isLeft)
                this.transform.localPosition = new Vector3(-9, 0, -11);
            else
                this.transform.localPosition = new Vector3(9, 0, -11);

        }
        else if (angle == LazerAngle.Down)
        {
            if (isLeft)
                this.transform.localPosition = new Vector3(-11, 0, 9);
            else
                this.transform.localPosition = new Vector3(-11, 0, -9);
        }

        if (isLeft)
        {
            outline.transform.localRotation = Quaternion.Euler(90, 90, 0);
            head.localPosition = head.localPosition + new Vector3(0, 0, 20);
        }
        else
        {
            outline.transform.localRotation = Quaternion.Euler(90, 270, 0);
            head.localPosition = head.localPosition + new Vector3(0, 0, -20);
        }

        lazer.isStop = true;
        
        outline.SetActive(true);
        inline.localScale = new Vector3(0, inline.localScale.y, inline.localScale.z);
        inline.DOScaleX(1, time).OnComplete(() =>
        {
            outline.SetActive(false);
        }).SetEase(Ease.Linear);

        //StartCoroutine(CoSound(time - 1.4f)); ;
        if(isLeft)
        {
            head.DOLocalMoveZ(1, time).OnComplete(() =>
            {
                lazer.isStop = false;
                if (this.gameObject.activeInHierarchy)
                    audioSource =  SoundManager.Instance.StartSoundFadeLoop("SE_Lazer_Loop3", 1.0f, 0, 0, 100);
                head.DOLocalMoveZ(-20, 5.0f).OnComplete(() =>
                {
                    lazer.isStop = true;
                    SoundManager.Instance.StopSound(audioSource, 0.1f);
                    head.DOLocalMoveY(10, 1.0f).OnComplete(() =>
                    {
                        Destroy(this.gameObject, 0.1f);
                    });
                }).SetEase(Ease.Linear);
            }).SetEase(Ease.OutCirc);
        }
        else
        {
            head.DOLocalMoveZ(-1, time).OnComplete(() =>
            {
                lazer.isStop = false;
                if (this.gameObject.activeInHierarchy)
                    audioSource = SoundManager.Instance.StartSoundFadeLoop("SE_Lazer_Loop3", 1.0f, 0, 0, 100);
                head.DOLocalMoveZ(20, 5.0f).OnComplete(() =>
                {
                    lazer.isStop = true;
                    SoundManager.Instance.StopSound(audioSource, 0.1f);
                    head.DOLocalMoveY(10, 1.0f).OnComplete(() =>
                    {
                        Destroy(this.gameObject, 0.1f);
                    });
                }).SetEase(Ease.Linear);
            }).SetEase(Ease.OutCirc);
        }
    }

    private IEnumerator CoSound(float t)
    {
        yield return new WaitForSeconds(t);
        SoundManager.Instance.StartSoundFadeLoop("SE_Lazer_Charge", 0.5f, 0.5f, 0, 1.5f);
    }

    public void Kill()
    {
        group.Kill();
    }

    private void OnDisable()
    {
        if(audioSource != null)
            SoundManager.Instance.StopSound(audioSource);
    }
}
