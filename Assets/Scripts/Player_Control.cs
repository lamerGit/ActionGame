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
    Vector3 targetDir;

    private void Awake()
    {
        inputActions = new PlayerInput();

    }

    private void Start()
    {
        player = GetComponent<Player>();
    }


    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.LeftClick.performed += OnLeftClick;
        inputActions.Player.LeftClick.canceled += OnLeftClick;
        
    }

    private void OnDisable()
    {
        inputActions.Player.LeftClick.canceled -= OnLeftClick;
        inputActions.Player.LeftClick.performed -= OnLeftClick;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Disable();
    }

    private void OnLeftClick(InputAction.CallbackContext obj)
    {
        if(obj.performed && !IsPointerOverUIObject())
        {
            
            player.CFXSet();
            player.IsLeftClick = true;
            player.DropItem();

            
        }else
        {
            player.IsLeftClick = false;

        }
        
    }

    private void OnLook(InputAction.CallbackContext obj)
    {
        Vector2 sceenPos = obj.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(sceenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))
        {
            targetDir = hit.point;
            lookDir = hit.point - player.transform.position;
            lookDir.y = 0.0f;
            lookDir = lookDir.normalized;


            player.TargetSet(targetDir);
            player.LookSet(lookDir);



        }
        if (Physics.Raycast(ray, out RaycastHit hitItem, 1000.0f, LayerMask.GetMask("Item")))
        {
            player.TargetItemSet(hitItem.transform.gameObject);


        }

    }

    /// <summary>
    /// 마우스클릭시 UI인지 object인지 확인해줄 변수
    /// </summary>
    /// <returns>true이면 UI있음 false면 UI없음</returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;

    }
}
