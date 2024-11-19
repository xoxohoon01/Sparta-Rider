using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
    
    // ItemControl이랑 같은지 확인용
    public GameObject generatorItem;
    public ItemSO generatorItemSo;

    public void Generate(GameObject obj)
    {
        ItemName _name = (ItemName)Random.Range(0, (int)ItemName.Count);
        Pool _pool = ItemManager.Instance.itemObjectPool.SpawnFromPool(_name);

        generatorItem = _pool.item;
        generatorItemSo = _pool.itemSO;

        // Item을 얻은 객체(obj)에게 Item 반환하기
        ItemController _itemControl = obj.GetComponent<ItemController>();
        _itemControl.GetItemPool(_pool);
    }
}