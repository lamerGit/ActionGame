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
    /// ���� ���� ������ ������
    /// </summary>
    public GameObject TargetItem
    {
        get { return targetItem; }
        set {
            //�ڽŸ��� �ٸ��������� ���������� �ƿ����� ����
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
    /// �������� �ݴ� �Լ�
    /// �����Ÿ��� ���;� �ݴ´�.
    /// </summary>
    /// <param name="t">Player_Control��ũ��Ʈ���� ��ŷ�� Item���̾ ���� ���ӿ�����Ʈ�� �޴´�</param>
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
    /// �÷��̾� �̵��Լ� �׺�޽��� ���� �����δ�.
    /// </summary>
    /// <param name="v">Player_Control���� �������� �޾ƿ´�</param>
    public void MovePlayer(Vector3 v)
    {
        
        agent.SetDestination(v);
        isMoving = true;
        animator.SetBool("isMove", isMoving);
    }

    /// <summary>
    /// ��ǥ��ġ ��ó�� ���� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="v">��ǥ������ �޴´�.</param>
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
    /// �÷��̾� ȸ���Լ�
    /// </summary>
    /// <param name="v">�����ϴ� ����</param>
    public void TurnPlayer(Vector3 v)
    {
        if (v != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v), Time.deltaTime * turnSpeed);
        }
    }

    /// <summary>
    /// �����ϴ� ���� ǥ�����ִ� �Լ�
    /// </summary>
    /// <param name="t">��ǥ����</param>
    public void CFXSet(Vector3 t)
    {
        Vector3 cfxVector = t;
        cfxVector.y = 0.8f;
        Instantiate(targetCFX, cfxVector,targetCFX.transform.rotation);
    }

    /// <summary>
    /// �������� ������ �Լ�
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
