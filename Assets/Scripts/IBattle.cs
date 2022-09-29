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

    void TakeDamage(float damage);
    void TakeHeal(float heal);

    void TakeMana(float mana);
   
}
