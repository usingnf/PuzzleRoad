using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Inspector")]
    [SerializeField] private Field field;
    [SerializeField] private GameObject[] seeThroughObjects;
    [SerializeField] private GameObject roof;

    //[Header("Status")]
    //[SerializeField] private bool isVisible = false;
    //[SerializeField] private bool isOpen = false;



    private void EnterPlayer()
    {

    }

    private void ExitPlayer()
    {

    }

    public void SetVisible(bool b)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitPlayer();
        }
    }

}
