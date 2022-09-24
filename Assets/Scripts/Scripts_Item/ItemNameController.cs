using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemNameController : MonoBehaviour
{
    TextMeshPro textMeshPro;
    OutlineController outlineController;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        outlineController=transform.root.GetComponent<OutlineController>();
    }

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

    public void NameOn()
    {
        textMeshPro.color = Color.green;
        outlineController.OutlineOn();
    }

    public void NameOff()
    {
        textMeshPro.color = Color.white;
        outlineController.OutlineOff();
    }


}
