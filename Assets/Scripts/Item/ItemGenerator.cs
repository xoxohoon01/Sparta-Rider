using System;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    // ItemControl이랑 같은지 확인용
    public GameObject generatorItem;
    public ItemSO generatorItemSo;
    private int itemNameCount;
    private void Start()
    {
        itemNameCount = Enum.GetValues(typeof(ItemName)).Length;
    }

    public void Generate(GameObject obj)
    {
        ItemName _name = (ItemName)UnityEngine.Random.Range(0, itemNameCount);
        Pool _pool = ItemManager.Instance.itemObjectPool.SpawnFromPool(_name);

        generatorItem = _pool.item;
        generatorItemSo = _pool.itemSO;

        // Item을 얻은 객체(obj)에게 Item 반환하기
        ItemController _itemControl = obj.GetComponent<ItemController>();
        _itemControl.GetItemPool(_pool);
    }

    public Pool Generate()
    {
        ItemName _name = (ItemName)UnityEngine.Random.Range(0, itemNameCount);
        Pool _pool = ItemManager.Instance.itemObjectPool.SpawnFromPool(_name);

        generatorItem = _pool.item;
        generatorItemSo = _pool.itemSO;

        return _pool;
    }
}