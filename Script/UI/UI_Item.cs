using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Image img;

    [SerializeField] private SO_Item item;
    [SerializeField] private int count;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Inventory.Instance.SetInfoItem(item, count);
        Inventory.Instance.Swt_Info(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Inventory.Instance.Swt_Info(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Inventory.Instance.SetInfoPos(eventData.position + new Vector2(0, -60));
    }

    public void OnPointerEnter()
    {
        Inventory.Instance.SetInfoItem(item, count);
        Inventory.Instance.Swt_Info(true);
    }

    public void OnPointerExit()
    {
        Inventory.Instance.Swt_Info(false);
    }

    public void OnPointerMove()
    {
        Inventory.Instance.SetInfoPos(Input.mousePosition);
    }

    public void SetData(SO_Item item, int count = 0)
    {
        this.item = item;
        img.sprite = item.itemSprite;
        this.count = count;
    }

    public void SetData(int count = 0)
    {
        img.sprite = item.itemSprite;
        this.count = count;
    }
}
