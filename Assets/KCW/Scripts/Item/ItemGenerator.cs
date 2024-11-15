using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private ItemObjectPool objectPool;
    public float spawnTime;
    private float lastSpawnTime;

    private void Awake()
    {
        objectPool = GetComponent<ItemObjectPool>();
    }

    void Update()
    {
        lastSpawnTime += Time.deltaTime;
        if (lastSpawnTime >= spawnTime)
        {
            lastSpawnTime = 0f;

            ItemType _type = (ItemType)Random.Range(0, (int)ItemType.Count);
            GameObject _obj = objectPool.SpawnFromPool(_type);
            _obj.transform.position = transform.position;
            _obj.SetActive(true);
        }
    }
}
