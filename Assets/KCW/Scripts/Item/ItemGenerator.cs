using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    
    public ItemObjectPool objectPool;
    public GameObject item;

    private void Awake()
    {
        objectPool = GetComponent<ItemObjectPool>();
    }

    public void Generate()
    {
        ItemType _type = (ItemType)Random.Range(0, (int)ItemType.Count);
        GameObject _item = objectPool.SpawnFromPool(_type);
        // Item을 Player에게 반환하기!!!
        item = _item;
        Debug.Log("Item SetActive");
    }
}
