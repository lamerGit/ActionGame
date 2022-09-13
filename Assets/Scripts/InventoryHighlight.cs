using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    /// <summary>
    /// ���̶���Ʈ�� ����Ű�� �Լ�
    /// </summary>
    /// <param name="b">���̶���Ʈ�� �����ֲ��� true �ƴϸ� false�� ������ �ȴ�.</param>
    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }

    /// <summary>
    /// �������� ũ�⿡ ���� ���̶���Ʈ�� ũ�⸦ �����ϴ� �Լ�
    /// </summary>
    /// <param name="targetItem">���̶���Ʈ�� ������ ������</param>
    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y= targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;
    }

    /// <summary>
    /// ���̶���Ʈ�� ��ġ�� �����ִ� �Լ�
    /// </summary>
    /// <param name="targetGrid">���� ���콺�� ����Ű�� �ִ� �׸���</param>
    /// <param name="targetItem">���� ������</param>
    public void SetPosition(ItemGrid targetGrid,InventoryItem targetItem)
    {
        SetParent(targetGrid);

        Vector2 pos = new();

        //����
        //pos = targetGrid.CalculatePositionOnGrid(targetItem.onGridPositionX, targetItem.onGridPositionY);
        
        //������
        pos = targetGrid.CalculatePositionOnGrid(targetItem.onGridPositionX, targetItem.onGridPositionY);


        highlighter.anchoredPosition = pos;
    }

    /// <summary>
    /// ���̶���Ʈ�� �׸����� �ڽ����� ������ �Լ�
    /// </summary>
    /// <param name="targetGrid">���� ����Ű�� �׸���</param>
    public void SetParent(ItemGrid targetGrid)
    {
        if(targetGrid==null)
        {
            return;
        }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    /// <summary>
    /// �������� ������ �ڸ��� �����ִ� �Լ�
    /// </summary>
    /// <param name="targetGrid">���� �׸���</param>
    /// <param name="targetItem">�ӽú���</param>
    /// <param name="posX">������ ��ġX</param>
    /// <param name="posY">������ ��ġY</param>
    public void SetPosition(ItemGrid targetGrid,InventoryItem targetItem,int posX,int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(posX,posY);

        highlighter.anchoredPosition = pos;


    }


}
