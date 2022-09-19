using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;
    Transform[] allChildren;
    private void Start()
    {
        Instantiate(data.itemPrefab,transform.position,transform.rotation,transform);
        allChildren = GetComponentsInChildren<Transform>();
     
    }

    public void OutlineOn()
    {
  

        for(int i = 1; i < allChildren.Length; i++)
        {
            allChildren[i].gameObject.layer = LayerMask.NameToLayer("Outline");
            //Debug.Log(child.name);
        }

    }

    public void OutlineOff()
    {

        for (int i = 1; i < allChildren.Length; i++)
        {
            allChildren[i].gameObject.layer = LayerMask.NameToLayer("Default");
            //Debug.Log(child.name);
        }
    }
}
