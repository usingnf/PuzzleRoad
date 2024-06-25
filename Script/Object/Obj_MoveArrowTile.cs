using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_MoveArrowTile : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    [SerializeField] private SpriteRenderer render;

    private Tweener tween = null;
    public void Arrow(ArrowAngle angle)
    {
        SoundManager.Instance.StartSound("SE_Tile_Do", 1.0f);
        if (angle == ArrowAngle.Up)
            arrow.localRotation = Quaternion.Euler(90, 180, 0);
        else if (angle == ArrowAngle.Down)
            arrow.localRotation = Quaternion.Euler(90, 0, 0);
        else if (angle == ArrowAngle.Left)
            arrow.localRotation = Quaternion.Euler(90, 90, 0);
        else if (angle == ArrowAngle.Right)
            arrow.localRotation = Quaternion.Euler(90, 270, 0);

        if (tween != null)
            tween.Kill();
        tween = render.DOFade(1, 0.33f).OnComplete(() =>
        {
            tween = render.DOFade(0, 0.33f).OnComplete(() =>
            {

            });
        });
    }

}
