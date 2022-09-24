using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Skill : MonoBehaviour
{
    public SkillData[] skillDatas;


    int attackLevel = 1;
    int prayerLevel = 0; //기도
    int vigorLevel = 0; //원기
    int mightLevel = 0; //위세
    int holyFireLevel = 0; // 신성한 불꽃
    int holyBoltLevel = 0; //신성한 빛줄기
    int blessedHammerLevel = 0; //축복받은 망치

    int skillPoint = 0;

    public int AttackLevel { get => attackLevel; }
    public int PrayerLevel { get => prayerLevel;  }
    public int VigorLevel { get => vigorLevel; }
    public int MightLevel { get => mightLevel; }
    public int HolyFireLevel { get => holyFireLevel;  }
    public int HolyBoltLevel { get => holyBoltLevel;  }
    public int BlessedHammerLevel { get => blessedHammerLevel;  }
    public int SkillPoint { get => skillPoint; }

    public void AttackLevelUp()
    {
        attackLevel++;
    }

    public void PreayLevelUp()
    {
        prayerLevel++;
    }
    public void VigorLevelUp()
    {
        vigorLevel++;
    }
    public void MightLevelUp()
    {
        mightLevel++;
    }
    public void HolyFireLevelUp()
    {
        holyFireLevel++;
    }

    public void HolyBoltLevelUp()
    {
        holyBoltLevel++;
    }
    public void BlessedHammerLevelUp()
    {
        blessedHammerLevel++;
    }
    public void LevelUp()
    {
        skillPoint++;
    }
}
