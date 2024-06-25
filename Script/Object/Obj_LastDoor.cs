using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_LastDoor : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private Animator ani;
    private bool isOpen = false;
    public bool isSound = false;
    public void Open()
    {
        if (isOpen)
            return;
        isOpen = true;
        ani.SetTrigger("Open");
        obstacle.SetActive(false);
        SoundManager.Instance.StartSound("SE_Elevator_Open", 0.8f);
    }

    public void Close()
    {
        if (!isOpen)
            return;
        isOpen = false;
        ani.SetTrigger("Close");
        obstacle.SetActive(true);
        if(isSound)
            StartCoroutine(CoClose());
    }

    private IEnumerator CoClose()
    {
        yield return new WaitForSeconds(0.83f);
        SoundManager.Instance.StartSound("SE_Elevator_Close", 0.8f);
    }
}
