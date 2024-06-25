using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inter_Gate : InteractableObject
{
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;
    [SerializeField] private NavMeshObstacle obstacle;

    public bool isProgress = false;

    private Tweener tween1;
    private Tweener tween2;
    public override void Interact(Unit u)
    {
        if (isProgress)
            return;
        base.Interact(u);
        Open();
    }

    public void Open()
    {
        isCanInteract = false;
        Swt_Interact(false);
        isProgress = true;
        tween1 = up.transform.DOLocalMoveY(5.0f, 5.0f);
        tween2 = down.transform.DOLocalMoveY(-5.0f, 5.0f);
        tween2.OnComplete(() =>
        {
            down.SetActive(false);
            obstacle.enabled = false;
        });
        tween1.Play();
        tween2.Play();
        //down.transform.DOLocalMoveY(-5.0f, 5.0f).OnComplete(() =>
        //{

        //});
        StartCoroutine(CoSound());
    }

    private IEnumerator CoSound()
    {
        AudioSource audio = SoundManager.Instance.StartSound("SE_Gate_Open", 0.2f);
        yield return new WaitForSeconds(2.8f);
        //SoundManager.Instance.StopSound(audio);
        SoundManager.Instance.StartSound("SE_Gate_End", 0.2f);
        ///yield return new WaitForSeconds(3.0f);
    }

    public void CloseForce()
    {
        if (tween1 != null)
            tween1.Kill();
        if(tween2 != null)
            tween2.Kill();
        isProgress = false;
        up.transform.localPosition = Vector3.zero;
        down.transform.localPosition = Vector3.zero;
        down.SetActive(true);
        obstacle.enabled = true;
    }
}
