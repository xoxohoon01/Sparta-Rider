using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    
    // GameManager에 넣을 예정
    public ItemObjectPool objectPool;
    
    // ItemControl이랑 같은지 확인용
    public GameObject generatorItem;
    public ItemSO generatorItemSo;

    private void Awake()
    {
        objectPool = GetComponent<ItemObjectPool>();
    }

    public void Generate(GameObject obj)
    {
        ItemType _type = (ItemType)Random.Range(0, (int)ItemType.Count);
        Pool _pool = objectPool.SpawnFromPool(_type);

        generatorItem = _pool.item;
        generatorItemSo = _pool.itemSO;

        // Item을 얻은 객체(obj)에게 Item 반환하기
        ItemController _itemControl = obj.GetComponent<ItemController>();
        _itemControl.GetItemPool(_pool);
    }
}