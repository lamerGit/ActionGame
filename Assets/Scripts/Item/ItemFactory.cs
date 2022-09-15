using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    static int itemCount = 0;


    public static GameObject MakeItem(ItemIDCode code)
    {
        GameObject obj = new GameObject();
        Item item=obj.AddComponent<Item>();

        item.data = DataManager.Instance.Items[(int)code];
        string itemName = item.data.itemName;
        obj.name = $"{itemName}_{itemCount}";
        obj.layer = LayerMask.NameToLayer("Item");
        SphereCollider col=obj.AddComponent<SphereCollider>();
        col.radius = 0.5f;
        col.isTrigger = true;
        itemCount++;


        return obj;
    }

    public static GameObject MakeItem(ItemIDCode code,Vector3 position,bool randomNoise=false)
    {
        GameObject obj = MakeItem(code);
        if(randomNoise)
        {
            Vector2 noise = Random.insideUnitCircle * 1.5f;
            position.x += noise.x;
            position.z += noise.y;
        }
        obj.transform.position = position;

        return obj;
    }

    public static void MakeItems(ItemIDCode code, Vector3 position,uint count)
    {
        for(int i=0; i<count; i++)
        {
            MakeItem(code, position, true);
        }
    }

    public static GameObject MakeItem(uint id)
    {
        return MakeItem((ItemIDCode)id);
    }

    public static GameObject MakeItem(uint id, Vector3 position, bool randomNoise=false)
    {
        return MakeItem((ItemIDCode)id, position, randomNoise);
    }

    public static void MakeItems(uint id,Vector3 postion,uint count)
    {
        MakeItems((ItemIDCode)id, postion, count);
    }
}
