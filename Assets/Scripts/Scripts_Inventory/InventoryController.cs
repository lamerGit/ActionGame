using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    //[HideInInspector]
    private ItemGrid selectedItemGrid; //현재 선택된 인벤토리를 할당하는 변수

    /// <summary>
    /// 현재 할당된 인벤토리를 넘겨주는 프로퍼티 
    /// set이 될때 inventoryHighlight(아이템위에 마우스를 올리거나 선택하면 주변을 하얗게 해주는 오브젝트)를 자식으로 만듬
    /// inventoryHighlight를 자식으로 만들어야 다른 인벤토리에 가려서 안보이지 않는다.
    /// </summary>
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);

        }
    }

    public EquipSlot selectedEquipSlot;

    public EquipSlot SelectedEquipSlot
    {
        get => selectedEquipSlot;
        set
        {
            selectedEquipSlot = value;
        }
    }



    InventoryItem selectedItem; //현재 선택된 아이템 변수
    InventoryItem overlapItem; // 아이템을 바꿔줄때 잠시 넣어줄 변수
    RectTransform rectTransform; //아이템의 캔버스위치를 받아올 변수
    RectTransform detailRectTrensform; // 아이템 디테일창을 위치를 미리찾아둘 변수


    //[SerializeField] List<ItemData> items; //사용할 아이템 데이터 !! 데이터매니저를 사용해서 이제 사용안함
    [SerializeField] GameObject itemPrefab; //생성할 아이템의 프리펩
    [SerializeField] Transform canvasTransform; // 인벤토리를 보여줄 캔버스를 넣는 변수
    [SerializeField] DetailInfo detailInfo; //아이템의 정보를 보여줄 스크립트

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;
    InventoryHighlight inventoryHighlight;

    PlayerInput inputActions;

    FindPlayerInventory playerInventory;
    EquipControl playerEquipControl;


    public InventoryItem SelectedItem
    {
        get { return selectedItem; }
        set { selectedItem = value; }
    }
    private void Awake()
    {
        inputActions=new PlayerInput();
        inventoryHighlight=GetComponent<InventoryHighlight>();
        detailRectTrensform = detailInfo.gameObject.GetComponent<RectTransform>();
        playerInventory=FindObjectOfType<FindPlayerInventory>();
        playerEquipControl=FindObjectOfType<EquipControl>();
    }

    private void OnEnable()
    {
        inputActions.InventoryUI.Enable();
        inputActions.InventoryUI.InventoryOnOff.performed += OnInventoryOnOff;
        inputActions.InventoryUI.CreateItem.performed += OnCreateItem;
        inputActions.InventoryUI.CreateEmptyItem.performed += OnCreateEmptyItem;
        inputActions.InventoryUI.ItemRotate.performed += OnItemRotate;
        inputActions.InventoryUI.LeftClick.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputActions.InventoryUI.LeftClick.performed -= OnLeftClick;
        inputActions.InventoryUI.ItemRotate.performed -= OnItemRotate;
        inputActions.InventoryUI.CreateEmptyItem.performed -= OnCreateEmptyItem;
        inputActions.InventoryUI.CreateItem.performed -= OnCreateItem;
        inputActions.InventoryUI.InventoryOnOff.performed -= OnInventoryOnOff;
        inputActions.InventoryUI.Disable();
    }

    private void OnInventoryOnOff(InputAction.CallbackContext obj)
    {
        //인벤토리가 off될때 선택하고 있는 인벤토리할당해제
        if(!playerInventory.OnOff())
        {
            selectedItemGrid = null;
        }
        if (!playerEquipControl.OnOff())
        {

        }

        
    }

    private void OnCreateItem(InputAction.CallbackContext obj)
    {
        //들고있는 아이템이 없을때만 q를 눌렀을때 아이템생성
        if (selectedItem == null)
        {
            CreateRandomItem();
        }
    }

    private void OnCreateEmptyItem(InputAction.CallbackContext obj)
    {
        //인벤토리의 빈칸에다가 아이템 넣기
        InsertRandomItem();
    }

    private void OnItemRotate(InputAction.CallbackContext obj)
    {

        //아이템 90도 돌리기 이미 돌려져 있다면 다시돌아옴

        RetateItem();
    }

    private void OnLeftClick(InputAction.CallbackContext obj)
    {
        //인벤토리가 켜져있을때만 실행
        if (selectedItemGrid != null || selectedEquipSlot!=null)
        {
            LeftMouseButtonPress();
        }
    }

    private void Update()
    {
        ItemIconDrag();
        DetailDrag();

        if (selectedEquipSlot != null)
        {
            if (selectedEquipSlot.SlotItem != null && selectedItem==null)
            {
                detailInfo.OnOff(true);
                detailInfo.InfoSet(selectedEquipSlot.SlotItem);
            }


        }
        //선택한 아이템이 없다면 하이라이트 보여주지않음
        if (selectedItemGrid == null && selectedEquipSlot==null)
        {
            inventoryHighlight.Show(false);
            detailInfo.OnOff(false);


            return;
        }

        if (selectedItemGrid != null)
        {
            HandleHighlight();
        }
        

        

     

        


    }

    /// <summary>
    /// detailInfo창을 움직이는함수
    /// </summary>
    private void DetailDrag()
    {
        if (detailInfo.gameObject.activeSelf)// detail창이 존재할때만
        {
            Vector2 mousePos = Mouse.current.position.ReadValue(); //마우스움직임받기
            Vector2 pivotVector = new Vector2(-2.0f, -3.0f); // 기본 피봇위치

            
            RectTransform rect = (RectTransform)detailRectTrensform.transform; // detail창의 위치 저장
            if ((mousePos.x + rect.sizeDelta.x * 3.5f) > Screen.width) //스크린옆으로 나갔을때
            {
                pivotVector.x = 3.0f;
            }
            if ((mousePos.y + rect.sizeDelta.y * 5.5f) > Screen.height) // 스크린위로 나갔을때
            {
                pivotVector.y = 3.0f;
            }
            


            detailRectTrensform.pivot = pivotVector; //최종피봇설정
            detailRectTrensform.transform.position = mousePos; // 마우스따라다니게 설정


        }
    }

    /// <summary>
    /// 아이템을 돌리는 함수,
    /// 들고있는 아이템이 없다면 return
    /// 그렇지 않으면 InventoryItem스크립트 안에있는 Rotate함수 실행
    /// </summary>
    private void RetateItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Rotate();
        Vector2Int positiononGrid = GetTileGridPosition();
        inventoryHighlight.SetSize(selectedItem);
        inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positiononGrid.x, positiononGrid.y);

    }

    /// <summary>
    /// 인벤토리 빈공간에 아이템을 넣는 함수 빈공간이 없으면 삭제한다.
    /// </summary>
    private void InsertRandomItem()
    {
        if(selectedItemGrid==null)
        {
            return;
        }
        if(selectedItem!=null)
        {
            return;
        }

        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceforObject(itemToInsert);
        if(posOnGrid==null)
        {
            Destroy(selectedItem.gameObject);
            return;
        }
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    /// <summary>
    /// 아이템을 인벤토리에 넣는 함수,
    /// Nullable Type으로 만든 FindSpaceforObject함수로 인벤토리 안에 들어갈 수 있는지 확인하고
    /// 값이 null이면 리턴 아니면 인벤토리에 넣어준다.
    /// </summary>
    /// <param name="itemToInsert">들어갈 아이템 정보</param>
    private void InsertItem(InventoryItem itemToInsert)
    {
        


        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceforObject(itemToInsert);

        if(posOnGrid==null)
        {
            return;
        }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void InsertItem(InventoryItem itemToInsert,ItemGrid Inventory)
    {
        Vector2Int? posOnGrid = Inventory.FindSpaceforObject(itemToInsert);

        if (posOnGrid == null)
        {
            return;
        }

        Inventory.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    /// <summary>
    /// 아이템위에 마우스를 올려놓거나 아이템을 선택했을때 그부분을 하이라이트해주는 함수
    /// </summary>
    private void HandleHighlight()
    {
        Vector2Int positiononGrid = GetTileGridPosition();
        if(oldPosition==positiononGrid) //선택한 위치와 예전위치가 같으면 리턴
        {
            return;
        }

        oldPosition = positiononGrid; //선택한 위치를 올드포지션에 넣어줌
                                      


        if (selectedItem == null) //들고있는 아이템이 없을때
        {
            itemToHighlight = selectedItemGrid.GetItem(positiononGrid.x, positiononGrid.y);
            

            if(itemToHighlight!=null)
            {
                inventoryHighlight.Show(true);
                detailInfo.OnOff(true);
                detailInfo.InfoSet(itemToHighlight);

                inventoryHighlight.SetSize(itemToHighlight);
                //inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }else
            {
                detailInfo.OnOff(false);
                inventoryHighlight.Show(false);
            }           
        }else //들고있는 아이템 있을때
        {
            detailInfo.OnOff(false);
            //아이템이 들어가야할 자리에 하이라이트를 보여준다
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(positiononGrid.x,
                                                                   positiononGrid.y,
                                                                   selectedItem.WIDTH,
                                                                   selectedItem.HEIGHT));

            inventoryHighlight.SetSize(selectedItem);
            //inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid,selectedItem,positiononGrid.x,positiononGrid.y);
        }
    }

    /// <summary>
    /// 랜덤한 아이템을 만들어 주는 함수,
    /// </summary>
    private void CreateRandomItem()
    {
        InventoryItem inventoryItem=Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;



        rectTransform=inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        //int selectedItemID=UnityEngine.Random.Range(0,items.Count);
        int selectedItemID=UnityEngine.Random.Range(0,DataManager.Instance.Items.Length);
        //inventoryItem.Set(items[selectedItemID]);
        inventoryItem.Set(DataManager.Instance.Items[selectedItemID]);


    }

    public bool PickUpItem(uint code,ItemGrid inventory)
    {
        bool result = false;
        if (selectedItem == null)
        {
            InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
            selectedItem = inventoryItem;

            rectTransform = inventoryItem.GetComponent<RectTransform>();
            rectTransform.SetParent(canvasTransform);
            rectTransform.SetAsLastSibling();

            inventoryItem.Set(DataManager.Instance.Items[code]);


            InventoryItem itemToInsert = selectedItem;
            Vector2Int? posOnGrid = inventory.FindSpaceforObject(itemToInsert);
            if (posOnGrid != null)
            {
                selectedItem = null;
                InsertItem(itemToInsert,inventory);
                //Destroy(selectedItem.gameObject);
                //return;
            }else
            {
                playerInventory.OnInventory();
            }
          
            result = true;
        }
        return result;

    }

    /// <summary>
    /// 아이템 아이콘이 마우스를 쫓아 다니게 하는 함수
    /// </summary>
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            if (!selectedItem.rotated)
            {
                //rectTransform.position = Input.mousePosition;
                rectTransform.position = Mouse.current.position.ReadValue();
            }else
            {
                rectTransform.position = Mouse.current.position.ReadValue();
                //rectTransform.position=new Vector3(Input.mousePosition.x,Input.mousePosition.y-(ItemGrid.tileSizeHeight*selectedItem.HEIGHT),0);
                rectTransform.position = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y - (ItemGrid.tileSizeHeight * selectedItem.HEIGHT), 0);
            }

        }
    }

    /// <summary>
    /// 마우스위치를 통해 몇번째 타일을 선택했는 지 확인하고 현재 들고있는 아이템이 없다면 그냥들고
    /// 아이템이 있다면 PlaceItem을 통해 아이템을 교채해준다.
    /// </summary>
    private void LeftMouseButtonPress()
    {

        
        //Debug.Log($"{tileGridPosition.x},{tileGridPosition.y}");
        if (selectedItem == null && selectedItemGrid != null)
        {
            Vector2Int tileGridPosition = GetTileGridPosition();
            PickUpItem(tileGridPosition);
            Vector2Int positiononGrid = GetTileGridPosition();
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positiononGrid.x, positiononGrid.y);
            detailInfo.OnOff(false);

        }
        else if (selectedItem != null && selectedItemGrid != null)
        {
            Vector2Int tileGridPosition = GetTileGridPosition();
            PlaceItem(tileGridPosition);

        }

        if (selectedEquipSlot!=null)
        {
            if(selectedEquipSlot.SlotItem==null && selectedItem!=null)
            {
                selectedEquipSlot.PlaceItem(selectedItem);
                selectedItem = null;
            }else if(selectedEquipSlot.SlotItem!=null && selectedItem==null)
            {
                Vector2 pivot = new Vector2(0, 1.0f);
                selectedItem = selectedEquipSlot.SlotItem;
                rectTransform=selectedItem.GetComponent<RectTransform>();
                rectTransform.SetParent(playerInventory.transform);
                rectTransform.pivot = pivot;
                rectTransform.anchorMax= pivot;
                rectTransform.anchorMin = pivot;
                rectTransform.SetAsLastSibling();
                selectedEquipSlot.SlotItem = null;
            }
            
        }
    }

    /// <summary>
    /// 현재 마우스위치가 인벤토리의 어느 칸에 있는지 반환해주는 함수
    /// </summary>
    /// <returns>인벤토리의 위치값 반환</returns>
    private Vector2Int GetTileGridPosition()
    {
        //Vector2 position = Input.mousePosition;
        Vector2 position = Mouse.current.position.ReadValue();

        //if(selectedItem!= null)
        //{
        //    position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileSizeWidth/2;
        //    position.y +=(selectedItem.itemData.height - 1) * ItemGrid.tileSizeHeight/2;
        //}

        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(position);
        return tileGridPosition;
    }

    /// <summary>
    /// 아이템을 인벤토리에 넣어주는 함수,
    /// 먼저 인벤토리에 들어갈수 있는 아이템인지 확인하고
    /// 들어갈수 있는 아이템이면 넣어주고 다른아이템이 있다면 교체한다. 
    /// </summary>
    /// <param name="tileGridPosition">넣을려고 하는 인벤토리의 위치</param>
    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y,ref overlapItem);
        if(complete)
        {
            detailInfo.OnOff(true); //디테일창 활성화
            detailInfo.InfoSet(selectedItem); // 디테일창 정보 세팅
            selectedItem = null;
            if(overlapItem!=null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform=selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
        
        
       
    }

    /// <summary>
    /// 인벤토리에 있는 아이템을 selectedItem에 넣어주는 함수
    /// </summary>
    /// <param name="tileGridPosition">타일의 위치</param>
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        

        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }

    }
}
