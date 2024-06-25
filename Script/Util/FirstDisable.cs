using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDisable : MonoBehaviour
{
    public bool isDisable = false;
    IEnumerator Start()
    {
        yield return null;
        if(isDisable)
            this.gameObject.SetActive(false);
    }

}
