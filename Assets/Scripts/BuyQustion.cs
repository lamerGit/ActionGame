using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuyQustion : MonoBehaviour
{
    Button yesButton;
    Button noButton;

    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;


    public Button YesButton
    {
        get { return yesButton; }
    }

    public Button NoButton
    {
        get { return noButton; }
    }

    private void Awake()
    {
        yesButton = transform.Find("Yes").GetComponent<Button>();
        noButton = transform.Find("No").GetComponent<Button>();

        itemName = transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        itemPrice = transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>();

        //noButton.onClick.AddListener(Close);

    }

    public void infoSet(ItemData itemData)
    {
        itemName.text = itemData.itemName;
        itemPrice.text = $"{itemData.Price}¿ø";
    }

}
