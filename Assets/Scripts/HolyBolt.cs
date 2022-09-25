using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyBolt : MonoBehaviour
{
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward*10.0f;
        StartCoroutine(Del());
    }

    IEnumerator Del()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            IBattle enemy=other.GetComponent<IBattle>();
            enemy.TakeDamage(20.0f);
        }
    }
}
