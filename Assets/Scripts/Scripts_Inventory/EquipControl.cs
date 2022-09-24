using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipControl : MonoBehaviour
{
    EquipSlot[] equipSlot;

    private void Awake()
    {
        equipSlot = new EquipSlot[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            equipSlot[i] = transform.GetChild(i).GetComponent<EquipSlot>();
        }
    }
    private void Start()
    {
        
        gameObject.SetActive(false);
    }

    public bool OnOff()
    {
        if (gameObject.activeSelf)
        {

            gameObject.SetActive(false);
            return false;
        }
        else
        {
            gameObject.SetActive(true);
            return true;
        }
    }

    public void OnInventory()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 장비슬롯중에 방패가 장착된 슬롯을 찾아주는 함수
    /// </summary>
    public EquipSlot FindEquipShield()
    {
        for(int i = 0; i < equipSlot.Length; i++)
        {
            if (equipSlot[i].SlotItem != null)
            {
                if (equipSlot[i].SlotItem.itemData.weaponType == WeaponType.Shield)
                {
                    return equipSlot[i];
                }
            }
        }

        return null;
    }

}
