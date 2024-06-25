using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_UpdownDebris : MonoBehaviour
{
    [SerializeField] private float delay = 1.0f;
    [SerializeField] private float distance = 2;
    [SerializeField] private float time = 4;
    

    private void OnEnable()
    {
        if(tween != null)
            tween.Kill();
        StartCoroutine(CoDelay());
    }

    private IEnumerator CoDelay()
    {
        yield return new WaitForSeconds(Random.Range(0, delay));
        Action();
    }

    private TweenerCore<Vector3, Vector3, VectorOptions> tween = null;
    private void Action(bool isUp = true)
    {
        if(isUp)
        {
            tween = transform.DOLocalMoveY(this.transform.position.y + distance, time).OnComplete(() =>
            {
                Action(false);
            }).SetEase(Ease.InOutSine);
        }
        else
        {
            tween = transform.DOLocalMoveY(this.transform.position.y - distance, time).OnComplete(() =>
            {
                Action(true);
            }).SetEase(Ease.InOutSine);
        }
        
    }
}
