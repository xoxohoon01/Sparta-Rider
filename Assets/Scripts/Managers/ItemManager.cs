using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : DestroySingleton<ItemManager>
{
    public ItemObjectPool itemObjectPool;
    public GameObject tomatoEffect;
    public GameObject mushroomEffect;
    protected override void Awake()
    {
        base.Awake();
        itemObjectPool = GetComponent<ItemObjectPool>();
    }
}
