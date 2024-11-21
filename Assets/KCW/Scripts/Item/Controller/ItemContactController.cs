using System.Collections;
using UnityEngine;

public class ItemContactController : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    private Renderer ren;
    private Collider col;

    private VehicleController vehicleController;

    private void Awake()
    {
        ren = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    public void SetVehicleController(VehicleController vController)
    {
        vehicleController = vController;
        vehicleController.totalRotate = 0f;
        vehicleController.itemSO = itemSO;
    }

    private void disableItem()
    {
        ren.enabled = false;
        col.enabled = false;
    }

    private void enableItem()
    {
        ren.enabled = true;
        ren.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(gameObject.transform.parent != null)
            {
                ItemSpawnPoint _point = GetComponentInParent<ItemSpawnPoint>();
                _point.ResetItem();
            }

            disableItem();
            SetVehicleController(collision.gameObject.GetComponent<VehicleController>());
            switch (itemSO.itemName)
            {
                case ItemName.Banana: CollideBanana(vehicleController.itemAccelerationMultiplier); break;
                case ItemName.Tomato: CollideTomato(); break;
                case ItemName.Coffee: CollideCoffee(vehicleController.itemAccelerationMultiplier); break;
                case ItemName.Cake: CollideCake(vehicleController.itemAccelerationMultiplier); break;
                case ItemName.Watermelon: CollideWatermelon(vehicleController.itemAccelerationMultiplier); break;
            }
        }
    }

    // 바나나 밟을 때
    private void CollideBanana(float initialSpeed)
    {
        vehicleController.isBanana = true;
        vehicleController.itemAccelerationMultiplier = 0f;
        StartCoroutine(CoCollideBanana(initialSpeed));
    }

    private IEnumerator CoCollideBanana(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.isBanana = false;
        vehicleController.itemAccelerationMultiplier = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }

    // 토마토 맞으면 화면 붉은색으로 변경 
    private void CollideTomato()
    {
        GameObject _effect = ItemManager.Instance.tomatoEffect;
        _effect.SetActive(true);
        StartCoroutine(CoCollideTomato(_effect));
    }

    private IEnumerator CoCollideTomato(GameObject image)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        enableItem();
        image.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // 커피 사용하면 속도 2배
    private void CollideCoffee(float initialSpeed)
    {
        vehicleController.itemAccelerationMultiplier *= 2f;
        StartCoroutine(CoCollideCoffee(initialSpeed));
    }

    private IEnumerator CoCollideCoffee(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.itemAccelerationMultiplier = initialSpeed;
    }

    private void CollideCake(float initialSpeed)
    {
        vehicleController.itemAccelerationMultiplier *= 0.5f;
        StartCoroutine(CoCollideCake(initialSpeed));
    }

    private IEnumerator CoCollideCake(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.itemAccelerationMultiplier = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }

    // 수박 맞으면 정해진 시간동안 멈춤
    private void CollideWatermelon(float initialSpeed)
    {
        vehicleController.itemAccelerationMultiplier = 0f;
    }

    private IEnumerator CoCollideWatermelon(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.itemAccelerationMultiplier = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }
}
