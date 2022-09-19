using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    ///아이템 인벤토리 메인 스크립트 여기서 만들고 InventroyController에서 사용한다.

    public const float tileSizeWidth = 64; //타일의 1개의 가로
    public const float tileSizeHeight = 64; // 타일의 1개의 높이

    InventoryItem[,] inventoryItemSlot; // 아이템 정보를 저장할 배열


    RectTransform rectTransform; // 스크린상 인벤토리 위치를 확인할 변수

    [SerializeField] int gridSizeWidth=20; //인벤토리 전체크기를 정할 가로 변수
    [SerializeField] int gridSizeHeight=10; // 인벤토리 전체크기를 정할 세로 변수


    /// <summary>
    /// RectTransform컴포넌트를 찾고 Init함수를 통해 gridSizeWidth*gridSizeHeight 사이즈의 인벤토리 생성
    /// </summary>

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);

    }

    /// <summary>
    /// 인벤토리를 생성해주는 함수,
    /// grid오브젝트에 있는 인벤토리 이미지타입은 Tiled이다
    /// 크기가 커져도 이미지가 반복되는 형태이기 때문에
    /// Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight)로 사이즈를 딱맞춰 계산하고
    /// rectTransform.sizeDelta=size로 사이즈를 맞춰준다.
    /// </summary>
    /// <param name="width">인벤토리의 가로</param>
    /// <param name="height">인벤토리의 세로</param>
    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;


    }

    Vector2 positionOnTheGrid = new Vector2(); //마우스포지션을 저장할 변수
    Vector2Int tileGridPosition = new Vector2Int(); //타일의 위치를 전달해줄 변수

    /// <summary>
    /// 마우스위치를 받아서 계산한다음 현제 가르키고 있는 타일이 무엇인지 알려주는 함수,
    /// </summary>
    /// <param name="mousePosition">마우스 위치 </param>
    /// <returns>현재마우스를 올려놓은 타일의 위치를 반환</returns>
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y-mousePosition.y;


        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    /// <summary>
    /// 인벤토리의 아이템을 반환해주는 함수
    /// </summary>
    /// <param name="x">인벤토리 x값</param>
    /// <param name="y">인벤토리 y값</param>
    /// <returns>인벤토리 아이템 반환</returns>
    public InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x,y];
    }
    /// <summary>
    /// 아이템을 인벤토리에 붙혀주기 전에 여러가지 체크를 하는 함수,
    /// BoundryCheck로 인벤토리를 넘어가는지 체크
    /// OverlapCheck로 아이템이 겹치는지 체크
    /// 다통과되면 CleanGridReference로 현재 overlapItem이 차지하고 있던 아이템 정보 삭제
    /// PlaceItem(오버로딩 함수)로 최종 아이템 붙혀주기
    /// </summary>
    /// <param name="inventoryItem">붙혀줄 아이템</param>
    /// <param name="posX">인벤토리 x값</param>
    /// <param name="posY">인벤토리 y값</param>
    /// <param name="overlapItem">현재 인벤토리에 있을지도 모르는 아이템</param>
    /// <returns>BoundryCheck값이 false면 false반환 OverlapItem이 false면 false반환 전부 통과하면 true반환</returns>
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
    /// 아이템을 인벤토리에 붙혀주는 함수,
    /// 아이템을 자신의 자식으로 붙혀준다.
    /// inventoryItemSlot에다가 위치에 맞게 inventoryItem으로 체워준다.
    /// 아이템의 위치정보를 갱신해준다(자신이 inventoryItemSlot의 x,y가 어디인지 저장)
    /// 화면상으로 나타날 위치 계산 CalculatePositionOnGrid함수 사용
    /// 회전상태이면 추가계산
    /// 위치 설정
    /// </summary>
    /// <param name="inventoryItem">아이템 정보</param>
    /// <param name="posX">아이템을 놓을 x값</param>
    /// <param name="posY">아이템을 놓을 y값</param>
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
    /// 인벤토리에 들어갈수 있으면서 거기에 아이템이 있는지 확인하는 함수,
    /// </summary>
    /// <param name="itemToInsert">들어가려는 아이템</param>
    /// <returns>인벤토리에 들어갈수 있으면서 아이템이 없으면 들어갈 인벤토리의 x,y값을 반환 안되면 null반환</returns>
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
    /// 타일의 화면 위치를 계산하는 함수,
    /// posX*tileSizeWidth를 하면 화면의 보이는 현재 타일의 x위치
    /// posY*tileSizeHeight를 하면 화면의 보이는 현재 타일의 y위치
    /// </summary>
    /// <param name="posX">현재 타일의 x값</param>
    /// <param name="posY">현재 타일의 y값</param>
    /// <returns>화면에 보이는 타일의 위치를 반환</returns>
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
    /// 아이템을 두려고 하는 곳에 아이템이 있는지 확인하는 함수,
    /// 해당 inventoryItemSlot에 아이템이 존재하고 overlapItem이 비어 있으면 아이템 정보를 준다.
    /// </summary>
    /// <param name="posX">선택된 인벤토리의 x값</param>
    /// <param name="posY">선택된 인벤토리의 y값</param>
    /// <param name="width">아이템의 가로</param>
    /// <param name="height">아이템의 세로</param>
    /// <param name="overlapItem">겹치는지 확인할 아이템</param>
    /// <returns>overlapItem이 null이 아니고 두려고 하는 아이템과 같지않으면 false 그렇지 않으면 true 반환</returns>
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
    /// 아이템이 있는지 없는 확인하는 함수,
    /// </summary>
    /// <param name="posX">이 아이템이 들어갈수 있는 최대x값</param>
    /// <param name="posY">이 아이템이 들어갈수 있는 최대y값</param>
    /// <param name="width">아이템의 x값 길이</param>
    /// <param name="height">아이템의 y값 길이</param>
    /// <returns>아이템이 있으면 false 없으면 true반환</returns>
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
    /// 아이템을 집는 함수,
    /// toReturn에 inventoryItemSlot[x,y]값을 넣는다
    /// inventoryItemSlot[x,y]에 값이 있으면
    /// CleanGridReference을 이용행 inventoryItemSlot[x,y]에 있던 아이템의 크기만큼 inventoryItemSlot을 비워준다.
    /// </summary>
    /// <param name="x">인벤토리의 x값</param>
    /// <param name="y">인벤토리의 y값</param>
    /// <returns>inventoryItemSlot[x,y]값이 없으면 null을 있으면 toReturn 반환</returns>
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
    /// 인벤토리에서 아이템을 비우는 함수,
    /// 아이템의 크기만큼 inventoryItemSlot을 null로 해준다
    /// </summary>
    /// <param name="item">크기를 받아올 아이템</param>
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
    /// 인벤토리 최대 최소 크기를 넘어가는지 체크하는 함수,
    /// </summary>
    /// <param name="posX">체크할 x값</param>
    /// <param name="posY">체크할 y값</param>
    /// <returns>최대 최소크기를 넘어가면 false 아니면 true</returns>
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
    /// 아이템이 인벤토리를 넘어가는 체크하는 함수,
    /// 1.먼저 인벤토리 x,y값이 넘어가는 PositionCheck로 체크
    /// 2.1번을 통과했으면 x,y값에 아이템의 가로-1,높이-1을 더하고 다시 PositionCheck로 체크
    /// </summary>
    /// <param name="posX">선택된 인벤토리의 x값</param>
    /// <param name="posY">선택된 인벤토리의 y값</param>
    /// <param name="width">선택된 아이템의 가로값</param>
    /// <param name="height">선택된 아이템의 높이값</param>
    /// <returns>위에 설명한 1,2을 못통과하면 false 아니면 true</returns>
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
