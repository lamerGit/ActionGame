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
    public int reauiredLevel; //�ʿ䷹��
    public float damagePerLevel; //������ �������� ������

}
