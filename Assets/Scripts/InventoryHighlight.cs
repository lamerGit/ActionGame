using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    /// <summary>
    /// 하이라이트를 껏다키는 함수
    /// </summary>
    /// <param name="b">하이라이트를 보여주꺼면 true 아니면 false를 넣으면 된다.</param>
    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }

    /// <summary>
    /// 아이템의 크기에 따라 하이라이트의 크기를 조절하는 함수
    /// </summary>
    /// <param name="targetItem">하이라이트를 보여줄 아이템</param>
    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y= targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;
    }

    /// <summary>
    /// 하이라이트의 위치를 정해주는 함수
    /// </summary>
    /// <param name="targetGrid">현재 마우스로 가르키고 있는 그리드</param>
    /// <param name="targetItem">현재 아이템</param>
    public void SetPosition(ItemGrid targetGrid,InventoryItem targetItem)
    {
        SetParent(targetGrid);

        Vector2 pos = new();

        //원본
        //pos = targetGrid.CalculatePositionOnGrid(targetItem.onGridPositionX, targetItem.onGridPositionY);
        
        //수정본
        pos = targetGrid.CalculatePositionOnGrid(targetItem.onGridPositionX, targetItem.onGridPositionY);


        highlighter.anchoredPosition = pos;
    }

    /// <summary>
    /// 하이라이트를 그리드의 자식으로 붙히는 함수
    /// </summary>
    /// <param name="targetGrid">현재 가르키는 그리드</param>
    public void SetParent(ItemGrid targetGrid)
    {
        if(targetGrid==null)
        {
            return;
        }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    /// <summary>
    /// 아이템이 들어가야할 자리는 보여주는 함수
    /// </summary>
    /// <param name="targetGrid">넣을 그리드</param>
    /// <param name="targetItem">임시변수</param>
    /// <param name="posX">들어가야할 위치X</param>
    /// <param name="posY">들어가야할 위치Y</param>
    public void SetPosition(ItemGrid targetGrid,InventoryItem targetItem,int posX,int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(posX,posY);

        highlighter.anchoredPosition = pos;


    }


}
