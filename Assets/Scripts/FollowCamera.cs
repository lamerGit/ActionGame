using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform target;
    public float speed = 3.0f;
    public Vector3 offset;

    private void Start()
    {
        target = FindObjectOfType<Player>()?.transform;
        
    }

    // ��� ������Ʈ �Լ����� ����� ����
    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.fixedDeltaTime);
        }

    }
}
