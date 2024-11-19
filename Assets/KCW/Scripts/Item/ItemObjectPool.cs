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
    private Dictionary<ItemName, Queue<Pool>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<ItemName, Queue<Pool>>();

        foreach (var pool in pools)
        {
            Queue<Pool> queue = new Queue<Pool>();
            for (int i = 0; i < pool.size; i++)
            {
                Pool _pool = new Pool();
                GameObject _obj = Instantiate(pool.item);
                _obj.SetActive(false);
                _obj.name = _obj.name.Replace("(Clone)", i.ToString());
                _pool.item = _obj;
                _pool.itemSO = pool.itemSO;
                queue.Enqueue(_pool);
            }

            poolDictionary.Add(pool.itemSO.name, queue);
        }
    }

    public Pool SpawnFromPool(ItemName name)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            return null;
        }

        Pool _pool = poolDictionary[name].Dequeue();
        poolDictionary[name].Enqueue(_pool);

        return _pool;
    }
}
