using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player_Control : MonoBehaviour
{
    PlayerInput inputActions;
    Player player;
    Vector3 lookDir;
    Vector3 targetPos;
    Vector2 mousePos;

    //bool isLeftClick = false;

    GameObject rayTarget = null;
    MonsterHpBarController monsterHpBar;

    MainSkillButton[] skillButtons; // 스킬버튼 찾기 0이 왼쪽 1이 오른쪽
    private void Awake()
    {
        inputActions = new PlayerInput();
        monsterHpBar = FindObjectOfType<MonsterHpBarController>();
        skillButtons=FindObjectsOfType<MainSkillButton>();
        //FindObjectsOfType은 순서대로 찾아준다는 보장이 없기때문에 아래코드로 정렬해준다.
        Array.Sort(skillButtons, (a, b) =>
        {
            return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
        });

        skillButtons[0].OnChangeSkill += (skill) =>
        {
            player.LeftSkill = skill;
        };

        skillButtons[1].OnChangeSkill += (skill) =>
        {
            player.RightSkill = skill;
        };
    }

    private void Start()
    {
        player = GetComponent<Player>();
        
    }

    private void FixedUpdate()
    {

        //if(Keyboard.current.digit1Key.wasPressedThisFrame)
        //{
        //    player.RightSkill = SkillType.Prayer;
        //}

        //if (Keyboard.current.digit2Key.wasPressedThisFrame)
        //{
        //    player.RightSkill = SkillType.Vigor;
        //}

        //if (Keyboard.current.digit3Key.wasPressedThisFrame)
        //{
        //    player.RightSkill = SkillType.Might;
        //}

        //if (Keyboard.current.digit4Key.wasPressedThisFrame)
        //{
        //    player.RightSkill = SkillType.Holyfire;
        //}

        mousePos = Mouse.current.position.ReadValue();

        if (!player.TargetOn)
        {
            RayTargetOutline();
        }else
        {
            RayTargetItemNameOff();
        }
        RayTarget();
        if (player.IsLeftClick)
        {
            mouseRay();

            //player.MovePlayer(targetPos);
            player.BattleAndMoveLeft(targetPos);
            if (!player.IsAttack)
            {
                player.TurnPlayer(lookDir);
            }

        }
        if (player.IsRightClick)
        {

            mouseRay();
            player.BattleAndMoveRight(targetPos);

        }
        player.DistaceAcess(targetPos);
        

    }


    private void OnEnable()
    {
        inputActions.Enable();
        //inputActions.Player.Look.performed += OnLook;
        inputActions.Player.LeftClick.performed += OnLeftClick;
        inputActions.Player.LeftClick.canceled += OnLeftClick;
        inputActions.Player.RightClick.performed += OnRightClick;
        inputActions.Player.RightClick.canceled+=OnRightClick;
        
    }

    private void OnDisable()
    {
        inputActions.Player.RightClick.canceled -= OnRightClick;
        inputActions.Player.RightClick.performed -= OnRightClick;
        inputActions.Player.LeftClick.canceled -= OnLeftClick;
        inputActions.Player.LeftClick.performed -= OnLeftClick;
        //inputActions.Player.Look.performed -= OnLook;
        inputActions.Disable();
    }

    private void OnRightClick(InputAction.CallbackContext obj)
    {
        if(obj.performed && !IsPointerOverUIObject())
        {
            if (!player.AuraSkill)
            {
                mouseRay();
                player.IsRightClick = true;
            }
        }else
        {
            player.IsRightClick=false;
            player.TargetOn = false;
        }
    }

    private void OnLeftClick(InputAction.CallbackContext obj)
    {
        if(obj.performed && !IsPointerOverUIObject())
        {

            mouseRay();
            player.CFXSet(targetPos);
            player.DropItem();
            //isLeftClick = true;
            player.IsLeftClick = true;

            SkillButtonOff();
            
        }else
        {
            player.IsLeftClick = false;
            player.TargetOn = false;
            //isLeftClick = false;
     
        }
        
    }

    /// <summary>
    /// 레이를 통해 마우스가 땅을 클릭하는지 확인하는 함수
    /// </summary>
    private void mouseRay()
    {
        Vector2 sceenPos = mousePos;
        Ray ray = Camera.main.ScreenPointToRay(sceenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")) && !player.TargetOn)
        {
            targetPos = hit.point;

            lookDir = hit.point - player.transform.position;
            lookDir.y = 0.0f;
            lookDir = lookDir.normalized;
            player.Target = rayTarget;
            if(rayTarget!=null)
            {
                targetPos = rayTarget.transform.root.position;
            }
        }
        

    }

    /// <summary>
    /// 레이를 통해 아이템을 선택하는 함수
    /// </summary>
    private void RayTarget()
    {

        Vector2 sceenPos = mousePos;
        Ray ray = Camera.main.ScreenPointToRay(sceenPos);


        if (Physics.Raycast(ray, out RaycastHit hitEnemy, 1000.0f, LayerMask.GetMask("Enemy")))
        {
            //player.PickUpItem(hitItem.transform.gameObject);
            rayTarget = hitEnemy.transform.gameObject;
            rayTarget.GetComponent<OutlineController>().OutlineOn();
            Enemy rayEnemy = rayTarget.GetComponent<Enemy>();
            monsterHpBar.Setinfo(rayEnemy);
            rayEnemy.onHealthChangeEnemy += (ratio) =>
            {
                monsterHpBar.Setinfo(ratio);
            };
           
            
            //monsterHpBar.Setinfo(rayTarget.GetComponent<Enemy>());
            //player.TargetItem = hitItem.transform.gameObject;
        }
        else if (Physics.Raycast(ray, out RaycastHit hitItemName, 1000.0f, LayerMask.GetMask("ItemName")))
        {
            rayTarget = hitItemName.transform.gameObject;
            rayTarget.GetComponent<ItemNameController>().NameOn();
        }
        else if (Physics.Raycast(ray, out RaycastHit hitItem, 1000.0f, LayerMask.GetMask("Item")) && !player.TargetOn)
        {

            rayTarget = hitItem.transform.gameObject;
            rayTarget.GetComponent<OutlineController>().OutlineOn();
        }
        else
        {
            rayTarget = null;
        }

       

    }

    private void RayTargetOutline()
    {
        if (rayTarget != null)
        {

            OutlineController outlineController = rayTarget.GetComponent<OutlineController>();
            ItemNameController itemNameController = rayTarget.GetComponent<ItemNameController>();

            if (outlineController != null)
            {

                outlineController.OutlineOff();

            }

            if (itemNameController != null)
            {
                itemNameController.NameOff();
            }

        }
        monsterHpBar.TargetOff();
    }

    void RayTargetItemNameOff()
    {
        if(rayTarget!=null)
        {
            ItemNameController itemNameController = rayTarget.GetComponent<ItemNameController>();
            if (itemNameController != null)
            {
                itemNameController.NameOff();
            }
        }
    }

    private void OnLook(InputAction.CallbackContext obj)
    {
       

    }

    /// <summary>
    /// 마우스클릭시 UI인지 object인지 확인해줄 변수
    /// </summary>
    /// <returns>true이면 UI있음 false면 UI없음</returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = mousePos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;

    }

    void SkillButtonOff()
    {
        skillButtons[0].SubButtonOff();
        skillButtons[1].SubButtonOff();
    }

}
