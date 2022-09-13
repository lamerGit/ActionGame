using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))] // ������Ʈ �ڵ��߰� ��û
public class GridInteract : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    // grid�� �־��ִ� ��ũ��Ʈ
    // grid�� ���콺�� �÷������� mainī�޶� �ִ� ��ũ��Ʈ InventoryController�� ���� SelectedItemGrid ���� ���콺�� �ø� �κ��丮 ������ �ش�.
    
    InventoryController inventoryController;
    ItemGrid itemGrid;

   
    /// <summary>
    /// InventoryControllerŸ���� 1���ۿ� ���� ������ FindObjectOfType�� ���
    /// ItemGrid��ũ��Ʈ�� GridInteract��ũ��Ʈ�� ���� �ֱ� ������ GetComponent�� ���
    /// </summary>
    private void Awake()
    {
        inventoryController=FindObjectOfType<InventoryController>();
        itemGrid=GetComponent<ItemGrid>();
    }

    /// <summary>
    /// �κ��丮�� ���콺�� �÷������� inventoryController.SelectedItemGrid�� ���� �κ��丮 ���� �Ҵ�
    /// </summary>
    /// <param name="eventData">������� ����</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;
    }
    /// <summary>
    /// �κ��丮�� ���콺�� ���� inventoryController.SelectedItemGrid�� �Ҵ� ����
    /// </summary>
    /// <param name="eventData">������� ����</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }
}
