using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerInventory : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public bool OnOff()
    {
        if(gameObject.activeSelf)
        {
            
            gameObject.SetActive(false);
            return false;
        }else
        {
            gameObject.SetActive(true);
            return true;
        }
    }

    public void OnInventory()
    {
        gameObject.SetActive(true);
    }
}
