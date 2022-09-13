using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))] // 컴포넌트 자동추가 요청
public class GridInteract : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    // grid에 넣어주는 스크립트
    // grid에 마우스를 올려놨을때 main카메라에 있는 스크립트 InventoryController의 변수 SelectedItemGrid 지금 마우스를 올린 인벤토리 정보를 준다.
    
    InventoryController inventoryController;
    ItemGrid itemGrid;

   
    /// <summary>
    /// InventoryController타입은 1개밖에 없기 때문에 FindObjectOfType을 사용
    /// ItemGrid스크립트는 GridInteract스크립트와 같이 있기 때문에 GetComponent를 사용
    /// </summary>
    private void Awake()
    {
        inventoryController=FindObjectOfType<InventoryController>();
        itemGrid=GetComponent<ItemGrid>();
    }

    /// <summary>
    /// 인벤토리에 마우스를 올려놓으면 inventoryController.SelectedItemGrid에 현재 인벤토리 정보 할당
    /// </summary>
    /// <param name="eventData">사용하지 않음</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;
    }
    /// <summary>
    /// 인벤토리에 마우스를 빼면 inventoryController.SelectedItemGrid에 할당 해제
    /// </summary>
    /// <param name="eventData">사용하지 않음</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
    }
}
