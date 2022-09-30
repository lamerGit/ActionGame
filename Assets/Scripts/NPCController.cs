using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject myInventory;
    OutlineController outlineController;
    GameObject myName;
    public ItemGrid NPCItemGrid;

    InventoryController inventoryController;

    private void Awake()
    {
        NPCItemGrid = myInventory.GetComponent<ItemGrid>();
    }

    private void Start()
    {
        outlineController=GetComponent<OutlineController>();
        myName = transform.Find("Name").gameObject;
        myName.transform.forward= Camera.main.transform.forward;
        
        inventoryController=FindObjectOfType<InventoryController>();
        StartCoroutine(StartCreateItem());
        myName.SetActive(false);
        
    }
    public void InventoryOn()
    {
        myInventory.gameObject.SetActive(true);

    }

    public void InventoryOff()
    {
        myInventory.gameObject.SetActive(false);
    }

    public void On()
    {
        outlineController.OutlineOn();
        myName.SetActive(true);
    }

    public void Off()
    {
        outlineController.OutlineOff();
        myName.SetActive(false);
    }


    IEnumerator StartCreateItem()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < 3; i++)
        {
            inventoryController.InsertRandomItem(NPCItemGrid,ItemIDCode.Armor);
        }
        for (int i = 0; i < 3; i++)
        {
            inventoryController.InsertRandomItem(NPCItemGrid, ItemIDCode.Diamond);
        }
        for (int i=0; i<5; i++)
        {
            inventoryController.InsertRandomItem(NPCItemGrid, ItemIDCode.Sword);
        }
        for (int i = 0; i < 5; i++)
        {
            inventoryController.InsertRandomItem(NPCItemGrid, ItemIDCode.Shield);
        }

        for (int i = 0; i < 10; i++)
        {
            inventoryController.InsertRandomItem(NPCItemGrid, ItemIDCode.Potion);
        }

        for (int i = 0; i < 10; i++)
        {
            inventoryController.InsertRandomItem(NPCItemGrid, ItemIDCode.Potion_Mana);
        }
        myInventory.SetActive(false);
    }


}
