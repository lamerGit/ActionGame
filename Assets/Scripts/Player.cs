using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour ,IBattle
{
 
    NavMeshAgent agent;

    public AnimatorController[] animeType;

    public float turnSpeed = 30.0f;
    public float moveSpeed = 10.0f;

    public GameObject targetCFX;
    public GameObject holyBoltEffect;
    public GameObject holyBolt;
    public GameObject blessdHammer;
    public GameObject holyFire;

    GameObject payerObj;
    GameObject vigorObj;
    GameObject mightObj;
    GameObject holyFireObj;

    float distanceRange = 1.5f;

    Animator animator;
    bool isMoving = false;

    bool isLeftClick = false;
    bool isAttack = false;
    bool targetOn = false;
    bool isRightClick = false;
    bool auraSkill=false;

    float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
            agent.speed = moveSpeed;
            agent.acceleration = moveSpeed * moveSpeed;
        }
    }

    public bool AuraSkill
    {
        get { return auraSkill; }
    }

    public bool IsAttack
    {
        get { return isAttack; }
    }

    public bool TargetOn
    {
        get { return targetOn; }
        set { targetOn = value; }
    }

    public bool IsLeftClick
    {
        get { return isLeftClick; }
        set { isLeftClick = value; }
    }

    public bool IsRightClick
    {
        get { return isRightClick; }
        set { isRightClick = value; }
    }


    GameObject target=null;
    ItemGrid playerInventory;
    InventoryController inventoryController;
    IBattle targetInterface;

    float pickUpRange = 3.0f;

    SkillType leftSkill = SkillType.Attack;
    SkillType rightSkill = SkillType.Attack;

    public SkillType LeftSkill
    {
        set
        {
            leftSkill = value;
        }
    }

    public SkillType RightSkill
    {
        get { return rightSkill; }
        set
        {
            if(rightSkill==SkillType.Prayer)
            {
                StopCoroutine(SkillPrayer());
                payerObj.SetActive(false);
                auraSkill = false;
            }
            else if (rightSkill == SkillType.Vigor)
            {
                SkillVigor(false);
                vigorObj.SetActive(false);
                auraSkill = false;
            }
            else if (rightSkill == SkillType.Might)
            {
                SkillMight(false);
                mightObj.SetActive(false);
                auraSkill = false;
            }
            else if (rightSkill == SkillType.Holyfire)
            {
                StopCoroutine(SkillHolyFire());
                holyFireObj.SetActive(false);
                auraSkill = false;
            }

            rightSkill = value;
            if(rightSkill==SkillType.Prayer)
            {
                StartCoroutine(SkillPrayer());
                payerObj.SetActive(true);
                auraSkill = true;
            }
            else if(rightSkill==SkillType.Vigor)
            {
                SkillVigor(true);
                vigorObj.SetActive(true);
                auraSkill = true;
            }
            else if(rightSkill==SkillType.Might)
            {
                SkillMight(true);
                mightObj.SetActive(true);
                auraSkill = true;
            }
            else if(rightSkill==SkillType.Holyfire)
            {
                StartCoroutine(SkillHolyFire());
                holyFireObj.SetActive(true);
                auraSkill = true;
            }
        }
    }

    SkillType castSkil = SkillType.None;

    Player_Skill player_Skill;

    int Level = 1;

    InventoryItem leftHand;
    InventoryItem armor;
    InventoryItem rightHand;

    public InventoryItem LeftHand
    {
        get { return leftHand; }
        set
        {
            if(leftHand!=null)
            {
                GameObject temp = findPlayerLeftHand.gameObject.transform.GetChild(0).gameObject;
                Destroy(temp);
            }
            
            leftHand = value;
            if(leftHand==null)
            {
                GameObject temp = findPlayerLeftHand.gameObject.transform.GetChild(0).gameObject;
                Destroy(temp);

                animator.runtimeAnimatorController = animeType[(int)Motion.NoWeapon];
            }else
            {
                GameObject temp= Instantiate(leftHand.itemData.equipItem, findPlayerLeftHand.gameObject.transform);
                temp.transform.localPosition = new(0, 0, 0);
                temp.transform.localRotation = Quaternion.Euler(leftHand.itemData.leftHandRotation);

                if(leftHand.itemData.weaponType==WeaponType.Shield)
                {
                    animator.runtimeAnimatorController = animeType[(int)Motion.SwordAndShield];
                }else if(leftHand.itemData.weaponType==WeaponType.Sword)
                {
                    animator.runtimeAnimatorController = animeType[(int)Motion.TwinSword];
                }
                
            }
            
        }
    }

    public InventoryItem RightHand
    {
        get { return rightHand; }
        set
        {
            if(rightHand!=null)
            {
                GameObject temp = findPlayerRightHand.gameObject.transform.GetChild(0).gameObject;
                Destroy(temp);
            }
           
            rightHand = value;
            if(rightHand==null)
            {
                GameObject temp = findPlayerRightHand.gameObject.transform.GetChild(0).gameObject;
                Destroy(temp);
            }else
            {
                GameObject temp = Instantiate(rightHand.itemData.equipItem, findPlayerRightHand.gameObject.transform);
                temp.transform.localPosition = new(0, 0, 0);
                temp.transform.localRotation = Quaternion.Euler(rightHand.itemData.rightHandRotation);

                
            }
        }
    }

    float power = 20.0f;
    float denfence = 1.0f;

    FindPlayerLeftHand findPlayerLeftHand;
    FindPlayerRightHand findPlayerRightHand;
    Rigidbody rigid;

    PlayerGlobeController[] playerHpMp;

    /// <summary>
    /// 현재 내가 선택한 타겟
    /// </summary>
    public GameObject Target
    {
        get { return target; }
        set
        {
           
            target = value;
            

        }
    }

    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
        agent.acceleration = moveSpeed * moveSpeed;
        playerInventory=FindObjectOfType<FindPlayerInventory>().GetComponent<ItemGrid>();
        inventoryController = FindObjectOfType<InventoryController>();
        player_Skill = GetComponent<Player_Skill>();
        findPlayerLeftHand = FindObjectOfType<FindPlayerLeftHand>();
        findPlayerRightHand = FindObjectOfType<FindPlayerRightHand>();
        payerObj = transform.Find("PrayerEffect").gameObject;
        vigorObj = transform.Find("VigorEffect").gameObject;
        mightObj = transform.Find("MightEffect").gameObject;
        holyFireObj = transform.Find("HolyFireEffect").gameObject;
        payerObj.SetActive(false);
        vigorObj.SetActive(false);
        mightObj.SetActive(false);
        holyFireObj.SetActive(false);

        playerHpMp = FindObjectsOfType<PlayerGlobeController>();
        //FindObjectsOfType은 순서대로 찾아준다는 보장이 없기때문에 아래코드로 정렬해준다. 0은 hp 1은 mp이다
        Array.Sort(playerHpMp, (a, b) =>
        {
            return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
        });

        onHealthChangePlayer += (ratio) =>
        {
            playerHpMp[0].GloveChange(ratio);
        };

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
        if (!isAttack)
        {
            agent.SetDestination(v);

            isMoving = true;
            animator.SetBool("isMove", isMoving);


        }
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
            if(Target!=null)
            {
                if(Target.layer==LayerMask.NameToLayer("Item"))
                {
                    PickUpItem(Target);
                    Target = null;
                    return;
                }
                if(Target.layer==LayerMask.NameToLayer("ItemName"))
                {
                    PickUpItem(Target.transform.root.gameObject);
                    Target = null;
                    return;
                }
                
                    
                
            }
            if (!IsLeftClick)
            {
                isMoving = false;
                animator.SetBool("isMove", isMoving);
            }
        }
    }

    public void BattleAndMoveLeft(Vector3 v)
    {
        float distance = (v - transform.position).sqrMagnitude;

        if (Target != null)
        {

            if (Target.layer == LayerMask.NameToLayer("Enemy") && IsLeftClick)
            {
                TargetOn = true;

                float skillRange = player_Skill.skillDatas[(int)leftSkill].range;
                if (distance < skillRange * skillRange)
                {
                    //Debug.Log("스킬발동");
                    if (leftSkill == SkillType.Attack)
                    {
                        transform.LookAt(v);
                        rigid.velocity = Vector3.zero;
                        SkillAttack();
                    }
                    else if (leftSkill == SkillType.HolyBolt)
                    {
                        transform.LookAt(v);
                        SkillHolyBolt();
                    }
                    else if (leftSkill == SkillType.BlessedHammer)
                    {
                        SkillBlessedHammer();
                    }
                    if (!IsLeftClick)
                    {
                        Target = null;
                        TargetOn = false;
                       

                    }
                    return;
                }
                else
                {
                   
                    MovePlayer(v);
                }
               
            }
        }

        MovePlayer(v);

    }

    public void BattleAndMoveRight(Vector3 v)
    {
        //isMoving = false;
        //animator.SetBool("isMove", isMoving);
        float distance = (v - transform.position).sqrMagnitude;

        if (Target != null)
        {

            if (Target.layer == LayerMask.NameToLayer("Enemy") && IsRightClick)
            {
                TargetOn = true;

                float skillRange = player_Skill.skillDatas[(int)rightSkill].range;
                if (distance < skillRange * skillRange)
                {
                    //Debug.Log("스킬발동");
                    if (rightSkill == SkillType.Attack)
                    {
                        transform.LookAt(v);
                        rigid.velocity = Vector3.zero;
                        SkillAttack();

                    }
                    else if (rightSkill == SkillType.HolyBolt)
                    {
                        
                        transform.LookAt(v);
                        
                        SkillHolyBolt();


                    }
                    else if (rightSkill == SkillType.BlessedHammer)
                    {
                        SkillBlessedHammer();
                    }
                    if (!IsRightClick)
                    {
                        Target = null;
                        TargetOn = false;
                        

                    }
                    return;
                }
                else
                {
                    if (rightSkill == SkillType.Attack)
                    {
                        MovePlayer(v);
                    }
                }

            }
        }else
        {
            if (rightSkill == SkillType.Attack)
            {
                MovePlayer(v);
            }else if (rightSkill == SkillType.HolyBolt)
            {
                if (!isAttack)
                {
                    transform.LookAt(v);
                }
                SkillHolyBolt();
            }
            else if (rightSkill == SkillType.BlessedHammer)
            {
                SkillBlessedHammer();
            }
            
        }
    }

    private void SkillAttack()
    {
        isMoving = false;
        animator.SetBool("isMove", isMoving);
        isAttack = true;
        targetInterface = Target.GetComponent<IBattle>();
        animator.SetBool("Attack", isAttack);
        agent.ResetPath();
    }

    private void SkillHolyBolt()
    {
        castSkil = SkillType.HolyBolt;
        isMoving = false;
        animator.SetBool("isMove", isMoving);
        isAttack = true;
        animator.SetBool("Skill", isAttack);
        agent.ResetPath();
    }

    private void SkillBlessedHammer()
    {
        castSkil = SkillType.BlessedHammer;
        isMoving = false;
        animator.SetBool("isMove", isMoving);
        isAttack = true;
        animator.SetBool("Skill", isAttack);
        agent.ResetPath();
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

    public void AttackOff()
    {
        isAttack = false;
        animator.SetBool("Attack", isAttack);
        if(targetInterface != null)
        {
            targetInterface.TakeDamage(power);
            targetInterface = null;
        }
    }
    public void SkillOff()
    {
        isAttack=false;
        animator.SetBool("Skill", isAttack);
        
        if(castSkil==SkillType.HolyBolt)
        {
            Instantiate(holyBolt, transform.position + transform.forward+transform.up, transform.rotation);
        }

        if(castSkil==SkillType.BlessedHammer)
        {
            Instantiate(blessdHammer,transform.position+Vector3.forward+Vector3.up+Vector3.left,Quaternion.identity);
            //temp.GetComponent<BlessedHammer>().SetSpiral(transform.position+Vector3.forward+Vector3.up);
        }
        
    }

    public void StartSkillEffect()
    {
       
        if(castSkil==SkillType.HolyBolt)
        {
            Instantiate(holyBoltEffect, transform);
        }
    }

    public void TwinAttackCombo1()
    {

        isAttack = false;
        animator.SetBool("Attack", isAttack);
        animator.SetInteger("TwinAttack", 1);
        if (targetInterface != null)
        {
            targetInterface.TakeDamage(power);
            targetInterface = null;
        }
    }

    public void TwinAttackCombo2()
    {
        isAttack = false;
        animator.SetBool("Attack", isAttack);
        animator.SetInteger("TwinAttack", 0);
        if (targetInterface != null)
        {
            targetInterface.TakeDamage(power);
            targetInterface = null;
        }
    }

    private float hp = 200;
    private float maxhp = 200;
    public float Hp
    {
        get => hp; 
        set
        {
            hp = Mathf.Clamp( value,0,maxhp);

            Debug.Log($"{hp}/{maxhp}");
            onHealthChangePlayer?.Invoke(hp/maxhp);
        }
    }
    public float maxHp
    {
        get => maxhp; 
        set
        {
            maxhp = value;
        }
    }
    public Action<float> onHealthChangePlayer { get; set; }

    public void TakeDamage(float damage)
    {
        
    }

    IEnumerator SkillPrayer()
    {
        while(rightSkill==SkillType.Prayer)
        {
            Hp += 10.0f;
            yield return new WaitForSeconds(1.0f);
        }
    }

    void SkillVigor(bool onOff)
    {
        if (onOff)
        {
            MoveSpeed += 10.0f;
        }else
        {
            MoveSpeed -= 10.0f;
        }
    }

    void SkillMight(bool onOff)
    {
        if(onOff)
        {
            power += 20.0f;
        }else
        {
            power -= 20.0f;
        }
    }

    IEnumerator SkillHolyFire()
    {
        while(rightSkill==SkillType.Holyfire)
        {
            Collider[] hitEnemy = Physics.OverlapSphere(transform.position, 15.0f, LayerMask.GetMask("Enemy"));
            if(hitEnemy.Length > 0)
            {
                foreach(Collider enemy in hitEnemy)
                {
                    Instantiate(holyFire,enemy.transform.position,Quaternion.identity);
                    IBattle battle = enemy.gameObject.GetComponent<IBattle>();
                    battle.TakeDamage(20.0f);
                }
            }

            yield return new WaitForSeconds(2.0f);
        }
    }
}
