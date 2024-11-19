using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    public ItemObjectPool itemObjectPool;

    private void Awake()
    {
        base.Awake();
        itemObjectPool = GetComponent<ItemObjectPool>();
    }
}
