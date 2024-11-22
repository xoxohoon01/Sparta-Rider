using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemController : MonoBehaviour
{
    private ItemMovement itemMovement;
    private float CarSizeZ;

    public GameObject item;
    public ItemSO itemSO;

    private VehicleController vehicleController;
    private Collider vehicleCollider;

    private void Awake()
    {
        vehicleController = GetComponent<VehicleController>();
        vehicleCollider = GetComponent<Collider>();

        CarSizeZ = vehicleCollider.bounds.size.z;
    }

    // Box 객체가 가지고 있는 ItemGenerator를 통해 Pool 가져오기
    public void GetItemPool(Pool pool)
    {
        if (!item && !itemSO)
        {
            item = pool.item;
            itemSO = pool.itemSO;
            itemMovement = item.GetComponent<ItemMovement>();
        }
    }

    public void OnItem()
    {
        if (item)
        {
            if(itemSO.itemType != ItemType.None) SetItem();
            itemMovement.CheckMoveItem(transform.forward, vehicleController);
            item = null;
            itemSO = null;
        }
    }

    // ItemType이 Move, Idle일 때만 실행 (None이면 실행 안됨)
    private void SetItem()
    {
        item.SetActive(true);
        if (itemSO.itemType == ItemType.Move)
        {
            // 자동차 앞에 위치
            item.transform.position = transform.position + CarSizeZ * 0.6f * transform.forward;
        }
        else
        {
            // 자동차 뒤에 위치
            item.transform.position = transform.position + CarSizeZ * 0.6f * -transform.forward;
        }
    }
}