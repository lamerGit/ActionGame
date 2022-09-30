using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InventoryGold : MonoBehaviour
{
    TextMeshProUGUI goldText;

    private void Awake()
    {
        goldText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void GoldChange(int gold)
    {
        goldText.text = gold.ToString();
    }

}
