using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFiend : Enemy
{
    public GameObject bullet;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(PlayerSearch());
    }


    IEnumerator PlayerSearch()
    {
        while (!isDead)
        {
            Collider[] hitPlayer = Physics.OverlapSphere(transform.position, 15.0f, LayerMask.GetMask("Player"));
            if (hitPlayer.Length > 0)
            {
                //Debug.Log("플레이어가 범위 안에 있다");
                transform.LookAt(hitPlayer[0].transform.position);
                animator.SetTrigger("Attack");
            }

            yield return new WaitForSeconds(2.0f);
        }
    }

    public void SpikeFiendAttack()
    {
        SpikeFiend_Bullet b = Instantiate(bullet, transform.position + transform.forward+transform.up, transform.rotation).GetComponent<SpikeFiend_Bullet>();
        b.bulletDamage = monsterData.damage;
    }
}
