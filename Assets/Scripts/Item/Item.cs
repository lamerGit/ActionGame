using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemData data;

    private void Start()
    {
        Instantiate(data.itemPrefab,transform.position,transform.rotation,transform);

    }

    public void NameOn()
    {
        transform.GetChild(0).transform.Find("ItemName").gameObject.SetActive(true);
    }

    public void NameOff()
    {
        transform.GetChild(0).transform.Find("ItemName").gameObject.SetActive(false);
    }


}
