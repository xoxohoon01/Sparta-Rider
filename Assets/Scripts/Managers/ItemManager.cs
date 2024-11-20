using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    public ItemObjectPool itemObjectPool;
    public GameObject tomatoEffect;

    private void Start()
    {
        itemObjectPool = GetComponent<ItemObjectPool>();
    }
}
