using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obj_FallingTile : MonoBehaviour
{
    [SerializeField] private SenarioTrue stage;
    [SerializeField] private Transform tile;
    [SerializeField] private BoxCollider coll;

    public void FallingTile()
    {
        //tile.gameObject.SetActive(true);
        //tile.localPosition = Vector3.zero;
        tile.DOLocalRotate(new Vector3(Random.Range(-180.0f, 180.0f), 0, Random.Range(-180.0f, 180.0f)), 2.0f);
        tile.DOLocalMoveY(0.5f + Random.Range(-0.1f, 0.1f), 0.1f).OnComplete(() =>
        {
            coll.enabled = true;

            tile.DOLocalMoveY(-8.0f + Random.Range(-2.0f, 2.0f), 1.5f).OnComplete(() =>
            {
                tile.gameObject.SetActive(false);
            }).SetEase(Ease.InQuad);
        }).SetEase(Ease.OutQuad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stage.FallingPlayer(PlayerUnit.player);
        }
    }
    

    public void DecreaseDifficult()
    {
        float size = coll.size.x;
        size = size * 0.95f;
        if (size < 0.50f)
            size = 0.50f;

        coll.size = new Vector3(size, coll.size.y, size);
    }
}
