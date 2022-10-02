using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IBattle
{
    public MonsterData monsterData;

   

    float hp;
    float maxhp;
    public float Hp { get => hp; 
        set
        {
            hp = Mathf.Clamp(value, 0.0f, maxhp);
            onHealthChangeEnemy?.Invoke(this);

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

    public System.Action<Enemy> onHealthChangeEnemy { get; set; }

    protected Animator animator;
    protected bool isDead=false;
    Collider myCollider;
    protected NavMeshAgent agent;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        maxHp = Random.Range(monsterData.minHP, monsterData.maxHP);
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            //Debug.Log($"{monsterData.name}이가 {damage}를 받았습니다");
            Hp -= damage;
            Debug.Log($"{hp}/{maxHp}");
            if (Hp < 0.1f)
            {
                isDead = true;
                animator.SetTrigger("Die");
                myCollider.enabled = false;
                agent.ResetPath();
                StartCoroutine(Del());
            }
        }


    }

    IEnumerator Del()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    public void TakeHeal(float heal)
    {
        
    }

    public void TakeMana(float mana)
    {
   
    }
}
