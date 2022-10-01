using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetailInfo : MonoBehaviour
{
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    TextMeshProUGUI itemPower;

    private void Awake()
    {
        itemName = transform.Find("info_ItemName").GetComponent<TextMeshProUGUI>();
        itemPrice = transform.Find("info_Price").GetComponent<TextMeshProUGUI>();
        itemPower = transform.Find("info_Power").GetComponent <TextMeshProUGUI>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnOff(bool b)
    {
        if(b)
        {
            gameObject.SetActive(true);

        }else
        {
            gameObject.SetActive(false);
        }
    }

    public void InfoSet(InventoryItem inventoryItem)
    {
        itemName.text = inventoryItem.itemName;
        itemPrice.text = $"{inventoryItem.itemData.Price}원";
        if(inventoryItem.itemData.weaponType==WeaponType.Sword)
        {
            itemPower.text = $"공격력 : {inventoryItem.itemData.damage}";
        }else if(inventoryItem.itemData.weaponType == WeaponType.Shield || inventoryItem.itemData.weaponType == WeaponType.Armor)
        {
            itemPower.text = $"방어력 : {inventoryItem.itemData.defance}";
        }else
        {
            itemPower.text = "";
        }

    }
}
