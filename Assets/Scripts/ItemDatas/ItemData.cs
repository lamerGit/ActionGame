using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// ������ �����Ϳ� ��ũ��Ʈ ����Ƽ ������Ʈ���� ����� �ְ� ��ũ��Ʈ��
    /// </summary>
    // �������� �⺻ 1�� ���̹� ���̸� ������ �־���Ѵ�
    public int width = 1;  
    public int height = 1;

    public Sprite itemIcon; //�������� ��������Ʈ�� ������ ���� ����

    public string itemName;
}
