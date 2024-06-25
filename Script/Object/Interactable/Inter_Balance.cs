using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inter_Balance : InteractableObject
{
    [SerializeField] private int remainCount = 3;
    [SerializeField] private UI_Balance balance;
    [SerializeField] private TextMeshPro text_count;
    public bool isZero = false;

    private void OnEnable()
    {
        text_count.text = remainCount.ToString();
    }
    public override void Interact(Unit u)
    {
        if (!isCanInteract)
            return;
        if (remainCount <= 0)
            return;
        balance.gameObject.SetActive(!balance.gameObject.activeSelf);
        if(!isZero && balance.gameObject.activeSelf)
            remainCount += -1;
        text_count.text = remainCount.ToString();
        if (balance.gameObject.activeSelf)
            SoundManager.Instance.StartSound("UI_Button_Click3", 1.0f);
    }

    public void SetCount(int count)
    {
        remainCount = count;
        text_count.text = remainCount.ToString();
    }
}
