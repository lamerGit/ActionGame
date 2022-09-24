using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemData data;
    Collider myCollider;

    private void Start()
    {
        Instantiate(data.itemPrefab,transform.position,transform.rotation,transform);

        myCollider = GetComponent<Collider>();
        myCollider.enabled = false;
        StartCoroutine(ColliderOn());

    }

    public void NameOn()
    {
        transform.GetChild(0).transform.Find("ItemName").gameObject.SetActive(true);
    }

    public void NameOff()
    {
        transform.GetChild(0).transform.Find("ItemName").gameObject.SetActive(false);
    }


    IEnumerator ColliderOn()
    {
        yield return new WaitForSeconds(0.5f);
        myCollider.enabled = true;
    }

}
