using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IBattle
{
    public MonsterData monsterData;

    float hp;
    float maxhp;
    public float Hp { get => hp; 
        set
        {
            hp = Mathf.Clamp(value, 0.0f, maxhp);


        }
    }
    public float maxHp
    {
        get => maxhp; 
        set
        {
            maxhp = value;
        
        }
    }

    private void Start()
    {
        maxHp = Random.Range(monsterData.minHP, monsterData.maxHP);
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"{monsterData.name}�̰� {damage}�� �޾ҽ��ϴ�");
        Hp-=damage;
        Debug.Log($"{hp}/{maxHp}");
    }
}
