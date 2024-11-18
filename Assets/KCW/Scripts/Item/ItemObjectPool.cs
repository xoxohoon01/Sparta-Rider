using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pool
{
    public GameObject item;
    public ItemSO itemSO;
    public int size;
}

public class ItemObjectPool : MonoBehaviour
{
    public List<Pool> pools = new List<Pool>();
    public Dictionary<ItemType, Queue<Pool>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<ItemType, Queue<Pool>>();

        foreach (var pool in pools)
        {
            Queue<Pool> queue = new Queue<Pool>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject _obj = Instantiate(pool.item);
                _obj.SetActive(false);
                pool.item = _obj;
                queue.Enqueue(pool);
            }

            poolDictionary.Add(pool.itemSO.type, queue);
        }
    }

    public Pool SpawnFromPool(ItemType type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            return null;
        }

        Pool _pool = poolDictionary[type].Dequeue();
        poolDictionary[type].Enqueue(_pool);

        return _pool;
    }
}
