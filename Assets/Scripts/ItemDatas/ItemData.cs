using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// 아이템 데이터용 스크립트 유니티 프로젝트에서 만들수 있게 스크립트함
    /// </summary>
    // 아이템은 기본 1의 높이밑 넓이를 가지고 있어야한다
    public int width = 1;  
    public int height = 1;

    public Sprite itemIcon; //아이템의 스프라이트를 가지고 있을 변수

    public string itemName;
}
