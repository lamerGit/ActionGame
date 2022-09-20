using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerInput inputActions;
    bool itemNameCheck = false;

    static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public bool ItemNameCheck
    {
        get { return itemNameCheck; }
    }

    private void Awake()
    {
        inputActions = new();

        if(instance==null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.GameManager.ItemNameOnOff.performed += OnItemNameOnOff;
    }

    private void OnDisable()
    {
        inputActions.GameManager.ItemNameOnOff.performed -= OnItemNameOnOff;
        inputActions.Disable();
    }

    private void OnItemNameOnOff(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Debug.Log("alt");
        itemNameCheck = !itemNameCheck;
        Item[] all = FindObjectsOfType<Item>();

        if (itemNameCheck)
        {
            for (int i = 0; i < all.Length; i++)
            {
                all[i].NameOn();
            }
        }else
        {
            for (int i = 0; i < all.Length; i++)
            {
                all[i].NameOff();
            }
        }
        
        


    }
}
