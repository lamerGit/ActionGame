using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetCFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(del());
    }

    IEnumerator del()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

   
}
