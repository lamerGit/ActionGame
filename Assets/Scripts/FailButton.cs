using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FailButton : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Close);
        gameObject.SetActive(false);
    }


    void Close()
    {
        gameObject.SetActive(false);
    }


}
