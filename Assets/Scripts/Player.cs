using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    Vector3 lookDir;
    Vector3 targetVector;
    Vector3 goalVector;
    NavMeshAgent agent;
    

    public float turnSpeed = 30.0f;
    public float moveSpeed = 10.0f;

    public GameObject targetCFX;

    float distanceRange = 1.5f;

    Animator animator;
    bool isMoving = false;

    bool isLeftClick = false;

    public bool IsLeftClick
    {
        get { return isLeftClick; }
        set { isLeftClick = value; }
    }


    GameObject targetItem=null;
    ItemGrid playerInventory;
    InventoryController inventoryController;

    float pickUpRange = 3.0f;


    /// <summary>
    /// 현재 내가 선택한 아이템
    /// </summary>
    public GameObject TargetItem
    {
        get { return targetItem; }
        set {
            //자신말고 다른아이템을 선택했을때 아웃라인 해제
            if (targetItem != null && targetItem!=value)
            {
                targetItem.GetComponent<Item>().OutlineOff();
            }
            targetItem = value; }
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
        agent.acceleration = moveSpeed * moveSpeed;
        playerInventory=FindObjectOfType<FindPlayerInventory>().GetComponent<ItemGrid>();
        inventoryController = FindObjectOfType<InventoryController>();
    }

    /// <summary>
    /// 아이템을 줍는 함수
    /// 일정거리에 들어와야 줍는다.
    /// </summary>
    /// <param name="t">Player_Control스크립트에서 피킹된 Item레이어를 가진 게임오브젝트를 받는다</param>
    private void PickUpItem(GameObject t)
    {
        float d = (t.transform.position - transform.position).sqrMagnitude;
        ItemData itemdata = t.GetComponent<Item>().data;

        if (d < pickUpRange * pickUpRange)
        {
            if (inventoryController.PickUpItem(itemdata.itemID, playerInventory))
            {

                Destroy(t);


            }
        }
    }
    /// <summary>
    /// 플레이어 이동함수 네브메쉬를 통해 움직인다.
    /// </summary>
    /// <param name="v">Player_Control에서 움직임을 받아온다</param>
    public void MovePlayer(Vector3 v)
    {
        
        agent.SetDestination(v);
        isMoving = true;
        animator.SetBool("isMove", isMoving);
    }

    /// <summary>
    /// 목표위치 근처로 가면 실행되는 함수
    /// </summary>
    /// <param name="v">목표지점을 받는다.</param>
    public void DistaceAcess(Vector3 v)
    {
        float distance = (v - transform.position).sqrMagnitude;

        if (distance < distanceRange * distanceRange)
        {
            if(TargetItem!=null)
            {
                PickUpItem(TargetItem);
                TargetItem = null;
            }

            isMoving = false;
            animator.SetBool("isMove", isMoving);
        }
    }

    /// <summary>
    /// 플레이어 회전함수
    /// </summary>
    /// <param name="v">봐야하는 방향</param>
    public void TurnPlayer(Vector3 v)
    {
        if (v != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v), Time.deltaTime * turnSpeed);
        }
    }

    /// <summary>
    /// 가야하는 곳을 표시해주는 함수
    /// </summary>
    /// <param name="t">목표지점</param>
    public void CFXSet(Vector3 t)
    {
        Vector3 cfxVector = t;
        cfxVector.y = 0.8f;
        Instantiate(targetCFX, cfxVector,targetCFX.transform.rotation);
    }

    /// <summary>
    /// 아이템을 버리는 함수
    /// </summary>
    public void DropItem()
    {
        if (inventoryController.SelectedItem != null)
        {
            ItemFactory.MakeItem(inventoryController.SelectedItem.itemData.itemID,transform.position,true);
            Destroy(inventoryController.SelectedItem.gameObject);
        }
    }

}
