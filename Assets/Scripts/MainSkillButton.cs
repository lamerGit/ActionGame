using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainSkillButton : MonoBehaviour
{
    Button myButton;
    GameObject subButton;

    public MainSkillButton anotherMainSkillButton;

    SkillType buttonSkillType = SkillType.Attack;

    public SkillType ButtonSkillType
    {
        get { return buttonSkillType; }
        set { buttonSkillType = value;
            OnChangeSkill?.Invoke(buttonSkillType);
        
        }
    }

    public Action<SkillType> OnChangeSkill;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        subButton = transform.Find("SubButton").gameObject;
        subButton.SetActive(false);
        myButton.onClick.AddListener(subButtonOnOff);

    }

    void subButtonOnOff()
    {
        if (subButton.activeSelf)
        {
            subButton.SetActive(false);
        }
        else
        {
            subButton.SetActive(true);
            anotherMainSkillButton.SubButtonOff();
        }
    }

    public void SubButtonOn()
    {
        subButton.SetActive(true);
    }

    public void SubButtonOff()
    {
        subButton.SetActive(false);
    }

    public void SetMainButton(Sprite sprite,SkillType skill)
    {
        ButtonSkillType = skill;
        myButton.image.sprite = sprite;
    }

}
