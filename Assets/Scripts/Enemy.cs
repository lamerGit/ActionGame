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
            onHealthChangeEnemy?.Invoke(this);

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

    public System.Action<Enemy> onHealthChangeEnemy { get; set; }

    private void Start()
    {
        maxHp = Random.Range(monsterData.minHP, monsterData.maxHP);
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"{monsterData.name}이가 {damage}를 받았습니다");
        Hp-=damage;
        Debug.Log($"{hp}/{maxHp}");
    }

    public void TakeHeal(float heal)
    {
        
    }

    public void TakeMana(float mana)
    {
   
    }
}
