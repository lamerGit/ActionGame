using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item Data",menuName ="Scriptable Object/Item Data",order =0)]
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

    public GameObject itemPrefab; // ���� �������� ������

    public uint itemID = 0;

    public EquipType equipType = EquipType.None;

    public GameObject equipItem; // ������ ������ ������

    public Vector3 leftHandRotation; //�޼������� ȸ��
    public Vector3 rightHandRotation; //������ ������ ȸ��

    public WeaponType weaponType = WeaponType.None;
}
