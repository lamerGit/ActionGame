using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   
}
