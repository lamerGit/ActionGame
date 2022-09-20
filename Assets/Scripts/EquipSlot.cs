using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    Image highlighter;
    Image slotImage;
    InventoryController inventoryController;

    InventoryItem slotItem=null;

    EquipSlot equipSlot=null;

    RectTransform rectTransform;

    Color redColor = new Color(1, 0, 0,0.3f);
    Color whiteColor = new Color(1, 1, 1, 0.3f);
    public InventoryItem SlotItem
    {
        get
        {
            return slotItem;
        }
        set
        {
            slotItem = value;
            if(slotItem==null)
            {
                slotImage.gameObject.SetActive(true);
            }else
            {
                slotImage.gameObject.SetActive(false);
            }
        }
    }



    [SerializeField]
    EquipType[] equipType;
    private void Awake()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        rectTransform = GetComponent<RectTransform>();
        highlighter = transform.GetChild(0).gameObject.GetComponent<Image>();
        slotImage=transform.GetChild(1).gameObject.GetComponent<Image>();
        highlighter.gameObject.SetActive(false);
        equipSlot = GetComponent<EquipSlot>();
    }

    public bool PlaceItem(InventoryItem inventoryItem)
    {
        bool result = false;
        if (CheckEquipType(inventoryItem))
        {
            RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
            rectTransform.SetParent(this.rectTransform);

            Vector2 pos = new Vector2(0, 0);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = pivot;
            rectTransform.anchorMin = pivot;
            rectTransform.anchorMax = pivot;
            rectTransform.anchoredPosition = pos;
            SlotItem = inventoryItem;
            result = true;
        }
        return result;
    }

    bool CheckEquipType(InventoryItem item)
    {
        bool result = false;
        for(int i=0; i<equipType.Length; i++)
        {
            if (equipType[i] ==item.EQUIPTYPE)
            {
                result = true;
            }
        }

        return result;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("วาด็");
        inventoryController.SelectedEquipSlot = equipSlot;
        if(inventoryController.SelectedItem!=null)
        {
            if(CheckEquipType(inventoryController.SelectedItem))
            {
                highlighter.color = whiteColor;
            }else
            {
                highlighter.color = redColor;
            }

            if (inventoryController.SelectedItem.EQUIPTYPE != EquipType.None)
            {
                highlighter.gameObject.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       inventoryController.SelectedEquipSlot=null;
        highlighter.gameObject.SetActive(false);
    }

}
