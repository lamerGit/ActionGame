using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IBattle 
{
    float Hp
    {
        get; set; 
    }
    float maxHp
    {
        get; set;
    }

    Action<Enemy> onHealthChangeEnemy { get; set; }

    void TakeDamage(float damage);
   
}
