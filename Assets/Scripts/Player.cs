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
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
        agent.acceleration = moveSpeed * moveSpeed;
        playerInventory=FindObjectOfType<FindPlayerInventory>().GetComponent<ItemGrid>();
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void FixedUpdate()
    {
        
        DistaceCFX();
        if(IsLeftClick)
        {
            TurnPlayer();
            MovePlayer();
            if(targetItem!=null)
            {
                float d = (targetItem.transform.position - transform.position).sqrMagnitude;
                ItemData itemdata = targetItem.GetComponent<Item>().data;

                if (d < pickUpRange * pickUpRange)
                {
                    if (inventoryController.PickUpItem(itemdata.itemID, playerInventory))
                    {

                        Destroy(targetItem);


                    }
                }

            }
        }
    }

    private void MovePlayer()
    {
        goalVector = targetVector;
        agent.SetDestination(goalVector);
        isMoving = true;
        animator.SetBool("isMove", isMoving);
    }

    private void DistaceCFX()
    {
        float distance = (goalVector - transform.position).sqrMagnitude;
        
        if (distance < distanceRange*distanceRange)
        {
            isMoving = false;
            animator.SetBool("isMove", isMoving);
        }
    }

    private void TurnPlayer()
    {
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * turnSpeed);
        }
    }

    public void LookSet(Vector3 v)
    {
        lookDir = v;
       
    }

    public void TargetSet(Vector3 t)
    {

        targetVector = t;

    }

    public void CFXSet()
    {
        Vector3 cfxVector = targetVector;
        cfxVector.y = 0.8f;
        Instantiate(targetCFX, cfxVector,targetCFX.transform.rotation);
    }

    public void TargetItemSet(GameObject t)
    {
        targetItem = t;
    }

    public void DropItem()
    {
        if (inventoryController.SelectedItem != null)
        {
            ItemFactory.MakeItem(inventoryController.SelectedItem.itemData.itemID,transform.position,true);
            Destroy(inventoryController.SelectedItem.gameObject);
        }
    }

}
