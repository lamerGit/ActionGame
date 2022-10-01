
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessedHammer : MonoBehaviour
{
    int startCnt = 0;
    int maxCnt = 0;
    List<Vector3> point;

    float timer = 0.001f;

    public float skillDamage = 10.0f;
    private void Start()
    {
        point = Spiral(transform.position, 10.0f, 500, 5);
        startCnt = point.Count - 1;
    }
    //public void SetSpiral(Vector3 v)
    //{
    //    point = Spiral(v, 20.0f, 500, 5);
    //    startCnt = point.Count - 1;
    //}

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        transform.Rotate(10.0f, 0, 0,Space.World);
        transform.position=point[startCnt];
        if(timer>0.0f)
        {
            timer-=Time.deltaTime;
        }else
        {
            timer = 0.001f;
            if(startCnt>maxCnt)
            {
                startCnt--;
            }else
            {
                Destroy(gameObject);
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            IBattle enemy=other.GetComponent<IBattle>();
            enemy.TakeDamage(skillDamage);
        }
    }



    List<Vector3> Spiral(Vector3 centerPoint, float radius,int pointCount,int reviseCount)
    {
        List<Vector3> list = new List<Vector3>();

        for(int i=0; i<pointCount; i++)
        {
            float angle = (i * 2.0f * Mathf.PI / (pointCount / reviseCount));
            float scale = radius * (1 - (float)i / pointCount);

            float x = centerPoint.x + scale * Mathf.Cos(angle);
            float y = centerPoint.z + scale * Mathf.Sin(angle);

            list.Add(new Vector3(x,centerPoint.y,y));
        }




        return list;
    }
}
