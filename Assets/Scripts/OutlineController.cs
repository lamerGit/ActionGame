using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    Transform[] allChildren;
    private void Start()
    {
        allChildren = GetComponentsInChildren<Transform>();
    }
    public void OutlineOn()
    {


        for (int i = 1; i < allChildren.Length-1; i++)
        {
            allChildren[i].gameObject.layer = LayerMask.NameToLayer("Outline");
            //Debug.Log(child.name);
        }
        allChildren[allChildren.Length - 1].gameObject.SetActive(true);

    }

    public void OutlineOff()
    {

        for (int i = 1; i < allChildren.Length-1; i++)
        {
            allChildren[i].gameObject.layer = LayerMask.NameToLayer("Default");
            //Debug.Log(child.name);
        }
        if (!GameManager.Instance.ItemNameCheck)
        {
            allChildren[allChildren.Length - 1].gameObject.SetActive(false);
        }
    }
}
