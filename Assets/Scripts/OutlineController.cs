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


        for (int i = 1; i < allChildren.Length; i++)
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
