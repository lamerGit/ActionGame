using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    ///������ �κ��丮 ���� ��ũ��Ʈ ���⼭ ����� InventroyController���� ����Ѵ�.

    public const float tileSizeWidth = 64; //Ÿ���� 1���� ����
    public const float tileSizeHeight = 64; // Ÿ���� 1���� ����

    InventoryItem[,] inventoryItemSlot; // ������ ������ ������ �迭


    RectTransform rectTransform; // ��ũ���� �κ��丮 ��ġ�� Ȯ���� ����

    [SerializeField] int gridSizeWidth=20; //�κ��丮 ��üũ�⸦ ���� ���� ����
    [SerializeField] int gridSizeHeight=10; // �κ��丮 ��üũ�⸦ ���� ���� ����


    /// <summary>
    /// RectTransform������Ʈ�� ã�� Init�Լ��� ���� gridSizeWidth*gridSizeHeight �������� �κ��丮 ����
    /// </summary>

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);

    }

    /// <summary>
    /// �κ��丮�� �������ִ� �Լ�,
    /// grid������Ʈ�� �ִ� �κ��丮 �̹���Ÿ���� Tiled�̴�
    /// ũ�Ⱑ Ŀ���� �̹����� �ݺ��Ǵ� �����̱� ������
    /// Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight)�� ����� ������ ����ϰ�
    /// rectTransform.sizeDelta=size�� ����� �����ش�.
    /// </summary>
    /// <param name="width">�κ��丮�� ����</param>
    /// <param name="height">�κ��丮�� ����</param>
    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;


    }

    Vector2 positionOnTheGrid = new Vector2(); //���콺�������� ������ ����
    Vector2Int tileGridPosition = new Vector2Int(); //Ÿ���� ��ġ�� �������� ����

    /// <summary>
    /// ���콺��ġ�� �޾Ƽ� ����Ѵ��� ���� ����Ű�� �ִ� Ÿ���� �������� �˷��ִ� �Լ�,
    /// </summary>
    /// <param name="mousePosition">���콺 ��ġ </param>
    /// <returns>���縶�콺�� �÷����� Ÿ���� ��ġ�� ��ȯ</returns>
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y-mousePosition.y;


        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    /// <summary>
    /// �κ��丮�� �������� ��ȯ���ִ� �Լ�
    /// </summary>
    /// <param name="x">�κ��丮 x��</param>
    /// <param name="y">�κ��丮 y��</param>
    /// <returns>�κ��丮 ������ ��ȯ</returns>
    public InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x,y];
    }
    /// <summary>
    /// �������� �κ��丮�� �����ֱ� ���� �������� üũ�� �ϴ� �Լ�,
    /// BoundryCheck�� �κ��丮�� �Ѿ���� üũ
    /// OverlapCheck�� �������� ��ġ���� üũ
    /// ������Ǹ� CleanGridReference�� ���� overlapItem�� �����ϰ� �ִ� ������ ���� ����
    /// PlaceItem(�����ε� �Լ�)�� ���� ������ �����ֱ�
    /// </summary>
    /// <param name="inventoryItem">������ ������</param>
    /// <param name="posX">�κ��丮 x��</param>
    /// <param name="posY">�κ��丮 y��</param>
    /// <param name="overlapItem">���� �κ��丮�� �������� �𸣴� ������</param>
    /// <returns>BoundryCheck���� false�� false��ȯ OverlapItem�� false�� false��ȯ ���� ����ϸ� true��ȯ</returns>
    public bool PlaceItem(InventoryItem inventoryItem,int posX,int posY,ref InventoryItem overlapItem)
    {
        if (BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false)
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    /// <summary>
    /// �������� �κ��丮�� �����ִ� �Լ�,
    /// �������� �ڽ��� �ڽ����� �����ش�.
    /// inventoryItemSlot���ٰ� ��ġ�� �°� inventoryItem���� ü���ش�.
    /// �������� ��ġ������ �������ش�(�ڽ��� inventoryItemSlot�� x,y�� ������� ����)
    /// ȭ������� ��Ÿ�� ��ġ ��� CalculatePositionOnGrid�Լ� ���
    /// ȸ�������̸� �߰����
    /// ��ġ ����
    /// </summary>
    /// <param name="inventoryItem">������ ����</param>
    /// <param name="posX">�������� ���� x��</param>
    /// <param name="posY">�������� ���� y��</param>
    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);


        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = new Vector2();
        position = CalculatePositionOnGrid(posX, posY);

        if(inventoryItem.rotated)
        {
            position.y -= inventoryItem.HEIGHT * tileSizeHeight;
        }

        //rectTransform.localPosition= position;
        rectTransform.anchoredPosition = position;
    }

    /// <summary>
    /// �κ��丮�� ���� �����鼭 �ű⿡ �������� �ִ��� Ȯ���ϴ� �Լ�,
    /// </summary>
    /// <param name="itemToInsert">������ ������</param>
    /// <returns>�κ��丮�� ���� �����鼭 �������� ������ �� �κ��丮�� x,y���� ��ȯ �ȵǸ� null��ȯ</returns>
    public Vector2Int? FindSpaceforObject(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT+1;
        int width = gridSizeWidth - itemToInsert.WIDTH+1;
        for (int y=0; y<height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(CheckAvailableSpace(x, y, itemToInsert.WIDTH, itemToInsert.HEIGHT))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
        
    }
    /// <summary>
    /// Ÿ���� ȭ�� ��ġ�� ����ϴ� �Լ�,
    /// posX*tileSizeWidth�� �ϸ� ȭ���� ���̴� ���� Ÿ���� x��ġ
    /// posY*tileSizeHeight�� �ϸ� ȭ���� ���̴� ���� Ÿ���� y��ġ
    /// </summary>
    /// <param name="posX">���� Ÿ���� x��</param>
    /// <param name="posY">���� Ÿ���� y��</param>
    /// <returns>ȭ�鿡 ���̴� Ÿ���� ��ġ�� ��ȯ</returns>
    public Vector2 CalculatePositionOnGrid(int posX, int posY)
    {
        Vector2 position = new();
        position.x = posX * tileSizeWidth;
        position.y = -(posY * tileSizeHeight);
        


        //position.x = posX * tileSizeWidth + tileSizeWidth / 2;
        //position.y = -(posY * tileSizeHeight + tileSizeHeight / 2);


        return position;
    }

    /// <summary>
    /// �������� �η��� �ϴ� ���� �������� �ִ��� Ȯ���ϴ� �Լ�,
    /// �ش� inventoryItemSlot�� �������� �����ϰ� overlapItem�� ��� ������ ������ ������ �ش�.
    /// </summary>
    /// <param name="posX">���õ� �κ��丮�� x��</param>
    /// <param name="posY">���õ� �κ��丮�� y��</param>
    /// <param name="width">�������� ����</param>
    /// <param name="height">�������� ����</param>
    /// <param name="overlapItem">��ġ���� Ȯ���� ������</param>
    /// <returns>overlapItem�� null�� �ƴϰ� �η��� �ϴ� �����۰� ���������� false �׷��� ������ true ��ȯ</returns>
    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if (overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {

                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// �������� �ִ��� ���� Ȯ���ϴ� �Լ�,
    /// </summary>
    /// <param name="posX">�� �������� ���� �ִ� �ִ�x��</param>
    /// <param name="posY">�� �������� ���� �ִ� �ִ�y��</param>
    /// <param name="width">�������� x�� ����</param>
    /// <param name="height">�������� y�� ����</param>
    /// <returns>�������� ������ false ������ true��ȯ</returns>
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }
    /// <summary>
    /// �������� ���� �Լ�,
    /// toReturn�� inventoryItemSlot[x,y]���� �ִ´�
    /// inventoryItemSlot[x,y]�� ���� ������
    /// CleanGridReference�� �̿��� inventoryItemSlot[x,y]�� �ִ� �������� ũ�⸸ŭ inventoryItemSlot�� ����ش�.
    /// </summary>
    /// <param name="x">�κ��丮�� x��</param>
    /// <param name="y">�κ��丮�� y��</param>
    /// <returns>inventoryItemSlot[x,y]���� ������ null�� ������ toReturn ��ȯ</returns>
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }
        CleanGridReference(toReturn);
        

       
        return toReturn;
    }

    /// <summary>
    /// �κ��丮���� �������� ���� �Լ�,
    /// �������� ũ�⸸ŭ inventoryItemSlot�� null�� ���ش�
    /// </summary>
    /// <param name="item">ũ�⸦ �޾ƿ� ������</param>
    private void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }

    }
    /// <summary>
    /// �κ��丮 �ִ� �ּ� ũ�⸦ �Ѿ���� üũ�ϴ� �Լ�,
    /// </summary>
    /// <param name="posX">üũ�� x��</param>
    /// <param name="posY">üũ�� y��</param>
    /// <returns>�ִ� �ּ�ũ�⸦ �Ѿ�� false �ƴϸ� true</returns>
    bool PositionCheck(int posX, int posY)
    {
        if(posX<0 || posY<0)
        {
            return false;
        }

        if(posX>=gridSizeWidth || posY>=gridSizeHeight )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// �������� �κ��丮�� �Ѿ�� üũ�ϴ� �Լ�,
    /// 1.���� �κ��丮 x,y���� �Ѿ�� PositionCheck�� üũ
    /// 2.1���� ��������� x,y���� �������� ����-1,����-1�� ���ϰ� �ٽ� PositionCheck�� üũ
    /// </summary>
    /// <param name="posX">���õ� �κ��丮�� x��</param>
    /// <param name="posY">���õ� �κ��丮�� y��</param>
    /// <param name="width">���õ� �������� ���ΰ�</param>
    /// <param name="height">���õ� �������� ���̰�</param>
    /// <returns>���� ������ 1,2�� ������ϸ� false �ƴϸ� true</returns>
    public bool BoundryCheck(int posX,int posY,int width,int height)
    {
        if (PositionCheck(posX,posY)==false)
        {
            return false;
        }

        posX += width-1;
        posY += height-1;

        if(PositionCheck(posX,posY)==false)
        {
            return false;
        }

        return true;
    }
}
