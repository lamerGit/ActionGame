using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    //[HideInInspector]
    private ItemGrid selectedItemGrid; //���� ���õ� �κ��丮�� �Ҵ��ϴ� ����

    /// <summary>
    /// ���� �Ҵ�� �κ��丮�� �Ѱ��ִ� ������Ƽ 
    /// set�� �ɶ� inventoryHighlight(���������� ���콺�� �ø��ų� �����ϸ� �ֺ��� �Ͼ�� ���ִ� ������Ʈ)�� �ڽ����� ����
    /// inventoryHighlight�� �ڽ����� ������ �ٸ� �κ��丮�� ������ �Ⱥ����� �ʴ´�.
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



    InventoryItem selectedItem; //���� ���õ� ������ ����
    InventoryItem overlapItem; // �������� �ٲ��ٶ� ��� �־��� ����
    RectTransform rectTransform; //�������� ĵ������ġ�� �޾ƿ� ����
    RectTransform detailRectTrensform; // ������ ������â�� ��ġ�� �̸�ã�Ƶ� ����


    //[SerializeField] List<ItemData> items; //����� ������ ������ !! �����͸Ŵ����� ����ؼ� ���� ������
    [SerializeField] GameObject itemPrefab; //������ �������� ������
    [SerializeField] Transform canvasTransform; // �κ��丮�� ������ ĵ������ �ִ� ����
    [SerializeField] DetailInfo detailInfo; //�������� ������ ������ ��ũ��Ʈ

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
        //�κ��丮�� off�ɶ� �����ϰ� �ִ� �κ��丮�Ҵ�����
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
        //����ִ� �������� �������� q�� �������� �����ۻ���
        if (selectedItem == null)
        {
            CreateRandomItem();
        }
    }

    private void OnCreateEmptyItem(InputAction.CallbackContext obj)
    {
        //�κ��丮�� ��ĭ���ٰ� ������ �ֱ�
        InsertRandomItem();
    }

    private void OnItemRotate(InputAction.CallbackContext obj)
    {

        //������ 90�� ������ �̹� ������ �ִٸ� �ٽõ��ƿ�

        RetateItem();
    }

    private void OnLeftClick(InputAction.CallbackContext obj)
    {
        //�κ��丮�� ������������ ����
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
        //������ �������� ���ٸ� ���̶���Ʈ ������������
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
    /// detailInfoâ�� �����̴��Լ�
    /// </summary>
    private void DetailDrag()
    {
        if (detailInfo.gameObject.activeSelf)// detailâ�� �����Ҷ���
        {
            Vector2 mousePos = Mouse.current.position.ReadValue(); //���콺�����ӹޱ�
            Vector2 pivotVector = new Vector2(-2.0f, -3.0f); // �⺻ �Ǻ���ġ

            
            RectTransform rect = (RectTransform)detailRectTrensform.transform; // detailâ�� ��ġ ����
            if ((mousePos.x + rect.sizeDelta.x * 3.5f) > Screen.width) //��ũ�������� ��������
            {
                pivotVector.x = 3.0f;
            }
            if ((mousePos.y + rect.sizeDelta.y * 5.5f) > Screen.height) // ��ũ������ ��������
            {
                pivotVector.y = 3.0f;
            }
            


            detailRectTrensform.pivot = pivotVector; //�����Ǻ�����
            detailRectTrensform.transform.position = mousePos; // ���콺����ٴϰ� ����


        }
    }

    /// <summary>
    /// �������� ������ �Լ�,
    /// ����ִ� �������� ���ٸ� return
    /// �׷��� ������ InventoryItem��ũ��Ʈ �ȿ��ִ� Rotate�Լ� ����
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
    /// �κ��丮 ������� �������� �ִ� �Լ� ������� ������ �����Ѵ�.
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
    /// �������� �κ��丮�� �ִ� �Լ�,
    /// Nullable Type���� ���� FindSpaceforObject�Լ��� �κ��丮 �ȿ� �� �� �ִ��� Ȯ���ϰ�
    /// ���� null�̸� ���� �ƴϸ� �κ��丮�� �־��ش�.
    /// </summary>
    /// <param name="itemToInsert">�� ������ ����</param>
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
    /// ���������� ���콺�� �÷����ų� �������� ���������� �׺κ��� ���̶���Ʈ���ִ� �Լ�
    /// </summary>
    private void HandleHighlight()
    {
        Vector2Int positiononGrid = GetTileGridPosition();
        if(oldPosition==positiononGrid) //������ ��ġ�� ������ġ�� ������ ����
        {
            return;
        }

        oldPosition = positiononGrid; //������ ��ġ�� �õ������ǿ� �־���
                                      


        if (selectedItem == null) //����ִ� �������� ������
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
        }else //����ִ� ������ ������
        {
            detailInfo.OnOff(false);
            //�������� ������ �ڸ��� ���̶���Ʈ�� �����ش�
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
    /// ������ �������� ����� �ִ� �Լ�,
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
    /// ������ �������� ���콺�� �Ѿ� �ٴϰ� �ϴ� �Լ�
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
    /// ���콺��ġ�� ���� ���° Ÿ���� �����ߴ� �� Ȯ���ϰ� ���� ����ִ� �������� ���ٸ� �׳ɵ��
    /// �������� �ִٸ� PlaceItem�� ���� �������� ��ä���ش�.
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
    /// ���� ���콺��ġ�� �κ��丮�� ��� ĭ�� �ִ��� ��ȯ���ִ� �Լ�
    /// </summary>
    /// <returns>�κ��丮�� ��ġ�� ��ȯ</returns>
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
    /// �������� �κ��丮�� �־��ִ� �Լ�,
    /// ���� �κ��丮�� ���� �ִ� ���������� Ȯ���ϰ�
    /// ���� �ִ� �������̸� �־��ְ� �ٸ��������� �ִٸ� ��ü�Ѵ�. 
    /// </summary>
    /// <param name="tileGridPosition">�������� �ϴ� �κ��丮�� ��ġ</param>
    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y,ref overlapItem);
        if(complete)
        {
            detailInfo.OnOff(true); //������â Ȱ��ȭ
            detailInfo.InfoSet(selectedItem); // ������â ���� ����
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
    /// �κ��丮�� �ִ� �������� selectedItem�� �־��ִ� �Լ�
    /// </summary>
    /// <param name="tileGridPosition">Ÿ���� ��ġ</param>
    private void PickUpItem(Vector2Int tileGridPosition)
    {
        

        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }

    }
}
