using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    bool rotateState = true;
    Collider myCollider;
    Rigidbody myRigidbody;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        StartCoroutine(rotateItem());
    }


    IEnumerator rotateItem()
    {
        while(rotateState)
        {
            transform.Rotate(Vector3.right * 10.0f);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            rotateState = false;
            myCollider.enabled = false;
            myRigidbody.isKinematic = true;
            transform.rotation = Quaternion.Euler(new Vector3(-90,0,0));
        }
    }
}
