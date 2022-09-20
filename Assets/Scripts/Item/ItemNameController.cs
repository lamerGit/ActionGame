using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemNameController : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.ItemNameCheck)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        transform.forward = Camera.main.transform.forward;
    }


}
