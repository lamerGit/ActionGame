using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallen : Enemy
{
    Player targetPlayer;
    bool isMove=false;
    float attackRange = 1.5f;
    float targetOffRange = 20.0f;

    float attackCoolTimeMax = 1.5f;
    float attackCoolTime = 0.5f;
  
    protected override void Start()
    {
        base.Start();
 
    }


    private void FixedUpdate()
    {
        if (!isDead)
        {

            if (attackCoolTime > 0.0f)
            {
                attackCoolTime -= Time.deltaTime;
            }

            if (targetPlayer != null)
            {
                float distance = (targetPlayer.transform.position - transform.position).sqrMagnitude;
                if (distance < attackRange * attackRange)
                {

                    isMove = false;
                    animator.SetBool("isMove", isMove);
                    agent.ResetPath();
                    transform.LookAt(targetPlayer.transform.position);


                    if (attackCoolTime < 0.1f)
                    {
                        attackCoolTime = attackCoolTimeMax;
                        animator.SetTrigger("Attack");
                    }
                }
                else if (distance > targetOffRange * targetOffRange)
                {
                    targetPlayer = null;
                    isMove = false;
                    animator.SetBool("isMove", isMove);
                    agent.ResetPath();
                }
                else
                {
                    agent.SetDestination(targetPlayer.transform.position);
                    isMove = true;
                    animator.SetBool("isMove", isMove);
                }

            }
            else
            {
                PlayerSearch();

            }
        }

    }

    void PlayerSearch()
    {

        Collider[] hitPlayer = Physics.OverlapSphere(transform.position, 15.0f, LayerMask.GetMask("Player"));
        if (hitPlayer.Length > 0)
        {
            targetPlayer = hitPlayer[0].gameObject.GetComponent<Player>();
            
        }


    }


    public void AttackPlayer()
    {
        if (targetPlayer != null)
        {
            IBattle battle = targetPlayer.GetComponent<IBattle>();
            battle.TakeDamage(monsterData.damage);
            targetPlayer = null;

        }
    }

}
