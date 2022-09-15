using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    Material material;


    private void Awake()
    {
        Renderer temp=GetComponent<Renderer>();
        material=temp.material;
    }

    private void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            gameObject.layer = LayerMask.NameToLayer("Outline");
        }
    }
}
