using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ItemType type;
        public GameObject prefab;
        public int size;
    }

    // 리스트는 자동적으로 시리얼라이즈 되고 딕셔너리는 안됨
    public List<Pool> pools = new List<Pool>();
    public Dictionary<ItemType, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        PoolDictionary = new Dictionary<ItemType, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject _obj = Instantiate(pool.prefab);
                _obj.SetActive(false);
                queue.Enqueue(_obj);
            }

            PoolDictionary.Add(pool.type, queue);
        }
    }

    public GameObject SpawnFromPool(ItemType type)
    {
        if (!PoolDictionary.ContainsKey(type))
        {
            return null;
        }

        GameObject _obj = PoolDictionary[type].Dequeue();
        PoolDictionary[type].Enqueue(_obj);

        return _obj;
    }
}
