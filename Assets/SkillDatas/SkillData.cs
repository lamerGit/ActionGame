using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Scriptable Object/Skill Data", order = 1)]
public class SkillData : ScriptableObject
{
    public float damage;
    public float range;
    public bool targetType;
    public float mana;
    public int reauiredLevel; //필요레벨
    public float damagePerLevel; //레벨당 더해지는 데미지

}
