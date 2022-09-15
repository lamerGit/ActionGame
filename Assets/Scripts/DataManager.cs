using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] ItemData[] items;

    static DataManager instance=null;
    public static DataManager Instance
    {
        get { return instance; }
    }

    public ItemData[] Items
    { 
        get 
        { return items; } 
    }


    private void Awake()
    {
        instance = this;
    }
}
