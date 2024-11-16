using UnityEngine;
using static UnityEditor.Progress;

public class ItemControl : MonoBehaviour
{
    public GameObject item;
    public ItemSO itemSO;

    public void GetItemPool(Pool pool)
    {
        item = pool.item;
        itemSO = pool.itemSO;
    }

    public void UseItem()
    {
        switch (itemSO.type)
        {
            case ItemType.Banana: break;
            case ItemType.Tomato: break;
            case ItemType.Coffee: break;
            case ItemType.Cake: break;
            case ItemType.Watermelon: break;
        }
    }
}