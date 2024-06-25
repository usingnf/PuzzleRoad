using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter_SwtCore : InteractableObject
{
    
    public enum SwtCoreType
    {
        Black = 0, //Black
        Yellow = 1,
        Red = 2,
        Green = 3,
        Blue = 4,
        White = 5,
        Explode = 6,
    }

    public SwtCoreType state;
    public bool isOn = false;

    [SerializeField] private GameObject[] line;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Color[] lineColors;

    private void Start()
    {
        SetSwt(SwtCoreType.Black);
    }

    public override void Interact(Unit u)
    {
        base.Interact(u);
        if (!isCanInteract)
            return;
        
    }

    private Tweener tweener = null;
    public void SetSwt(SwtCoreType state)
    {
        //foreach (GameObject line in line)
        //{
        //    line.SetActive(false);
        //}
        
        
        if(state == SwtCoreType.Black)
        {
            isOn = false;
            line[0].SetActive(true);
            line[1].SetActive(false);
        }
        else
        {
            SoundManager.Instance.StartSound("SE_Core_Energy", 0.5f);
            if (!isOn)
            {
                SoundManager.Instance.StartSound("SE_Core_PowerOn", 0.5f);
            }
            else
            {
                SoundManager.Instance.StartSound("SE_Core_PowerChange", 0.5f);
            }
            isOn = true;
            line[0].SetActive(false);
            line[1].SetActive(true);
        }
        this.state = state;

        Color start = lineRenderer.startColor;
        Color end = lineRenderer.endColor;
        Color finish = lineColors[(int)state];
        if (tweener != null)
            tweener.Kill();
        tweener = lineRenderer.DOColor(new Color2(start, end), new Color2(finish, end), 0.5f);
        tweener.OnComplete(() =>
        {
            tweener = lineRenderer.DOColor(new Color2(finish, end), new Color2(finish, finish), 0.5f);
            tweener.Play();
        });
        tweener.Play();
        //lineRenderer.DOColor(new Color2(start, lineColors[(int)state]), new Color2(end, end), 1.0f).OnComplete(() => 
        //{
        //    lineRenderer.DOColor(new Color2(lineColors[(int)state], lineColors[(int)state]), new Color2(end, lineColors[(int)state]), 1.0f).OnComplete(() =>
        //    {

        //    });
        //});
    }
}
