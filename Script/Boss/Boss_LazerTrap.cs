using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LazerAngle
{
    Left = 0,
    Up = 1,
    Right = 2,
    Down = 3,
}
public class Boss_LazerTrap : MonoBehaviour
{
    public Boss_LazerTrapGroup group;
    [SerializeField] private Transform head;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform targetStartPos;
    [SerializeField] private Boss_Lazer lazer;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject obj_charge;
    [SerializeField] private Transform arrow;
    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject detector;
    [SerializeField] private GameObject sound;


    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.T))
    //    {
    //        Debug.Log("T");
    //        AttackStart(temp, temp2, 2.0f);
    //    }
    //}
    public void AttackStart(float pos, LazerAngle angle, float time, bool isSound)
    {
        this.transform.rotation = Quaternion.Euler(0, (int)angle * 90, 0);
        if (pos < -9)
            pos = -9;
        else if (pos > 9)
            pos = 9;
        if (angle == LazerAngle.Left)
        {
            this.transform.localPosition = new Vector3(pos, 0, 0);
            head.position = startPos.position + new Vector3(0, 0, 5);
        }
        else if (angle == LazerAngle.Up)
        {
            this.transform.localPosition = new Vector3(0, 0, pos);
            head.position = startPos.position + new Vector3(5, 0, 0);
        }
        else if (angle == LazerAngle.Right)
        {
            this.transform.localPosition = new Vector3(pos, 0, 0);
            head.position = startPos.position + new Vector3(0, 0, -5);
        }
        else if (angle == LazerAngle.Down)
        {
            this.transform.localPosition = new Vector3(0, 0, pos);
            head.position = startPos.position + new Vector3(-5, 0, 0);
        }
        lazer.isStop = true;
        target.position = targetStartPos.position;
        outline.SetActive(true);
        arrow.localPosition = new Vector3(0, 0.1f, 9);
        arrow.gameObject.SetActive(true);
        arrow.DOLocalMoveZ(-9, time).OnComplete(()=>
        {
            arrow.gameObject.SetActive(false);
        }).SetEase(Ease.Linear);
        head.DOMove(startPos.position, time).OnComplete(() =>
        {
            lazer.isStop = false;
            detector.SetActive(true);
            lazer.isLook = true;
            //sound.SetActive(true);
            if(this.gameObject.activeInHierarchy)
                SoundManager.Instance.StartSound("SE_Lazer_Shot", 1.0f);
            target.DOLocalMoveZ(-11, 0.5f).OnComplete(() =>
            {
                lazer.isStop = true;
                detector.SetActive(false);
                outline.SetActive(false);
                obj_charge.SetActive(false);
                //sound.SetActive(false);
                target.DOMove(targetStartPos.position, 0.5f).OnComplete(() =>
                {
                    lazer.isLook = false;
                    head.DOLocalMoveY(head.localPosition.y + 5.0f, 1.0f).OnComplete(() =>
                    {
                        //this.gameObject.SetActive(false);
                        Destroy(this.gameObject, 0.1f);
                    });
                }).SetEase(Ease.Linear);
            }).SetEase(Ease.Linear);
        });
    }

    public void Kill()
    {
        group.Kill();
    }
}
