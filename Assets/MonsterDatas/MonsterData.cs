using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Monster Data", menuName = "Scriptable Object/Monster Data", order = 2)]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public MonsterType monsterType;

    public float minHP;
    public float maxHP;

    public float damage = 0;
    
}
