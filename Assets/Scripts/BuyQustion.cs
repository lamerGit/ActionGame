using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyQustion : MonoBehaviour
{
    Button yesButton;
    Button noButton;

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
        //noButton.onClick.AddListener(Close);
        
    }

}
