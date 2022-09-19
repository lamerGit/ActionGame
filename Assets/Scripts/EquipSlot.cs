using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    GameObject highlighter;
    InventoryController inventoryController;

    InventoryItem slotItem=null;

    EquipSlot equipSlot=null;

    RectTransform rectTransform;
    public InventoryItem SlotItem
    {
        get
        {
            return slotItem;
        }
        set
        {
            slotItem = value;
        }
    }



    [SerializeField]
    EquipType equipType;
    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        rectTransform = GetComponent<RectTransform>();
        highlighter = transform.GetChild(0).gameObject;
        highlighter.SetActive(false);
        equipSlot = GetComponent<EquipSlot>();
    }

    public void PlaceItem(InventoryItem inventoryItem)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        Vector2 pos = new Vector2(0, 0);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = pivot;
        rectTransform.anchorMin = pivot;
        rectTransform.anchorMax = pivot;
        rectTransform.anchoredPosition= pos;
        slotItem = inventoryItem;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("วาด็");
        inventoryController.SelectedEquipSlot = equipSlot;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       inventoryController.SelectedEquipSlot=null;
    }

}
