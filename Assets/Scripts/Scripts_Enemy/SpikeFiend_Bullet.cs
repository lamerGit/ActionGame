using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFiend_Bullet : MonoBehaviour
{
    Rigidbody rigid;

    public float bulletDamage=0.0f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * 10.0f;
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattle battle = other.GetComponent<IBattle>();
            if (battle != null)
            {
                battle.TakeDamage(bulletDamage);
                Destroy(gameObject);
            }
        }

    }
}
