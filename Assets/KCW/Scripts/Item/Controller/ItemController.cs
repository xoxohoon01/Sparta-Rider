using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemController : MonoBehaviour
{
    private ItemMovement itemMovement;
    private VehicleController vehicleController;
    private float CarSizeZ;

    public GameObject item;
    public ItemSO itemSO;

    private void Awake()
    {
        vehicleController = GetComponent<VehicleController>();

        // 차의 z 축 크기 계산
        Renderer[] _renderers = GetComponentsInChildren<Renderer>();
        if (_renderers.Length > 0)
        {
            Bounds _combinedBounds = _renderers[0].bounds;

            foreach (Renderer renderer in _renderers)
            {
                _combinedBounds.Encapsulate(renderer.bounds);
            }

            CarSizeZ = _combinedBounds.size.z;
        }
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
            if (itemSO.name == ItemName.Coffee)
            {
                // 코루틴에서 itemSO 사용
                StartCoroutine(CoBooster(vehicleController.acceleration));
                vehicleController.acceleration *= 2f;
            }
            else
            {
                SetItem();
                itemMovement.Move(transform.forward);
            }
            item = null;
            itemSO = null;
        }
    }

    private void SetItem()
    {
        item.SetActive(true);
        if (itemSO.speed > 0)
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

    private IEnumerator CoBooster(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.acceleration = initialSpeed;
    }
}