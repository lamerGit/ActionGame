using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MonsterHpBarController : MonoBehaviour
{
    Image hpbar;
    TextMeshProUGUI monsterName;
    TextMeshProUGUI monsterType;
    Enemy targetEnemy;

    private void Awake()
    {
        hpbar = transform.GetChild(0).GetComponent<Image>();
        monsterName=transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        monsterType=transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        

    }

    private void Start()
    {
        gameObject.SetActive(false);
    }


    public void Setinfo(Enemy enemy)
    {
        targetEnemy = enemy;
        hpbar.fillAmount = enemy.Hp / enemy.maxHp;
        monsterName.text = enemy.monsterData.monsterName;
        if(enemy.monsterData.monsterType==MonsterType.beast)
        {
            monsterType.text = "야수";
        }else if(enemy.monsterData.monsterType == MonsterType.undead)
        {
            monsterType.text = "언데드";
        }
        else if(enemy.monsterData.monsterType == MonsterType.demon)
        {
            monsterType.text = "악마";
        }
        gameObject.SetActive(true);
    }

    public void TargetOff()
    {
        if (targetEnemy != null)
        {
            targetEnemy.gameObject.GetComponent<OutlineController>().OutlineOff();
            targetEnemy.onHealthChangeEnemy = null;
        }

        gameObject.SetActive(false);
    }

}
