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



    InventoryItem selectedItem; //���� ���õ� ������ ����
    InventoryItem overlapItem; // �������� �ٲ��ٶ� ��� �־��� ����
    RectTransform rectTransform; //�������� ĵ������ġ�� �޾ƿ� ����
    RectTransform detailRectTrensform; // ������ ������â�� ��ġ�� �̸�ã�Ƶ� ����


    [SerializeField] List<ItemData> items; //����� ������ ������
    [SerializeField] GameObject itemPrefab; //������ �������� ������
    [SerializeField] Transform canvasTransform; // �κ��丮�� ������ ĵ������ �ִ� ����
    [SerializeField] DetailInfo detailInfo; //�������� ������ ������ ��ũ��Ʈ

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;
    InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight=GetComponent<InventoryHighlight>();
        detailRectTrensform = detailInfo.gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        ItemIconDrag();
        DetailDrag();
        //����ִ� �������� �������� q�� �������� �����ۻ���
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (selectedItem == null)
            {
                CreateRandomItem();
            }
        }

        //�κ��丮�� ��ĭ���ٰ� ������ �ֱ�
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            InsertRandomItem();
        }

        //������ 90�� ������ �̹� ������ �ִٸ� �ٽõ��ƿ�
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RetateItem();
        }

        //������ �������� ���ٸ� ���̶���Ʈ ������������
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            detailInfo.OnOff(false);


            return;
        }

        HandleHighlight();

        //���콺 ���ʹ�ư �������� �ൿ
        //if (Input.GetMouseButtonDown(0))
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LeftMouseButtonPress();

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
    /// 
    /// </summary>
    private void InsertRandomItem()
    {
        if(selectedItemGrid==null)
        {
            return;
        }

        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
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

        int selectedItemID=UnityEngine.Random.Range(0,items.Count);
        inventoryItem.Set(items[selectedItemID]);


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
        Vector2Int tileGridPosition = GetTileGridPosition();
        //Debug.Log($"{tileGridPosition.x},{tileGridPosition.y}");
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
            Vector2Int positiononGrid = GetTileGridPosition();
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positiononGrid.x, positiononGrid.y);
            detailInfo.OnOff(false);

        }
        else
        {
            
            PlaceItem(tileGridPosition);
            
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
