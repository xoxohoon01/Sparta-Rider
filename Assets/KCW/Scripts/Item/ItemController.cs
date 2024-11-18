using System;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemController : MonoBehaviour
{
    private Vector3 defaultPosY;
    public GameObject item;

    private void Awake()
    {
        defaultPosY = new Vector3(0f, 5f, 0f);
    }

    public void GetItemPool(Pool pool)
    {
        if (!item)
        {
            item = pool.item;
        }
    }

    public void OnItem()
    {
        if (item)
        {
            SetItem();
            item = null;
        }
    }

    private void SetItem()
    {
        item.SetActive(true);
        item.transform.position = transform.position + defaultPosY;
    }
}