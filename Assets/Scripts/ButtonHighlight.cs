using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class ButtonHighlight : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler
{
    TextMeshProUGUI myText;

    void Awake()
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnDisable()
    {
        myText.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myText.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myText.color = Color.white;
    }

}
