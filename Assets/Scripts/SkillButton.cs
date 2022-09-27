using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    MainSkillButton mainSkillButton;
    Button myButton;

    public SkillType skillType;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        mainSkillButton = transform.parent.GetComponentInParent<MainSkillButton>();
        myButton.onClick.AddListener(SubButtonOff);
    }

    void SubButtonOff()
    {
        mainSkillButton.SetMainButton(myButton.image.sprite,skillType);
        mainSkillButton.SubButtonOff();
    }


}
