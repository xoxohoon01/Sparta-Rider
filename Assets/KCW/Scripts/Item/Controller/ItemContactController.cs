using System.Collections;
using UnityEngine;

public class ItemContactController : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    private Renderer ren;
    private Collider col;

    private VehicleController vehicleController;

    private Coroutine CoBanana;
    private Coroutine CoTomato;
    private Coroutine CoCoffee;
    private Coroutine CoCake;
    private Coroutine CoWatermelon;
    private Coroutine CoMushroom;

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
        col.enabled = true;
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
            float _initialSpeed = vehicleController.itemAccelerationMultiplier;
            switch (itemSO.itemName)
            {
                case ItemName.Banana: CollideBanana(); break;
                case ItemName.Tomato: CollideTomato(); break;
                case ItemName.Coffee: CollideCoffee(_initialSpeed); break;
                case ItemName.Cake: CollideCake(_initialSpeed); break;
                case ItemName.Watermelon: CollideWatermelon(); break;
                case ItemName.Mushroom: CollideMushroom(); break;
            }
        }
    }

    // 바나나 밟을 때
    private void CollideBanana()
    {
        vehicleController.isBanana = true;
        vehicleController.itemAccelerationMultiplier = 0f;
        if (CoBanana != null) StopCoroutine(CoBanana);
        CoBanana = StartCoroutine(CoCollideBanana());
    }

    private IEnumerator CoCollideBanana()
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.isBanana = false;
        vehicleController.itemAccelerationMultiplier = 1f;
        enableItem();
        gameObject.SetActive(false);
    }

    // 토마토 맞으면 화면 붉은색으로 변경 
    private void CollideTomato()
    {
        GameObject _effect = ItemManager.Instance.tomatoEffect;
        _effect.SetActive(true);
        if (CoTomato != null) StopCoroutine(CoTomato);
        CoTomato = StartCoroutine(CoCollideTomato(_effect));
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
        if(CoCoffee != null) StopCoroutine(CoCoffee);
        CoCoffee = StartCoroutine(CoCollideCoffee(initialSpeed));
    }

    private IEnumerator CoCollideCoffee(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.itemAccelerationMultiplier = initialSpeed;
        gameObject.SetActive(false);
    }

    private void CollideCake(float initialSpeed)
    {
        vehicleController.itemAccelerationMultiplier *= 0.5f;
        if(CoCake != null) StopCoroutine(CoCake);
        CoCake = StartCoroutine(CoCollideCake(initialSpeed));
    }

    private IEnumerator CoCollideCake(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.itemAccelerationMultiplier = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }

    // 수박 맞으면 정해진 시간동안 멈춤
    private void CollideWatermelon()
    {
        vehicleController.itemAccelerationMultiplier = 0f;
        if(CoWatermelon != null) StopCoroutine(CoWatermelon);
        CoWatermelon = StartCoroutine(CoCollideWatermelon());
    }

    private IEnumerator CoCollideWatermelon()
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.itemAccelerationMultiplier = 1f;
        enableItem();
        gameObject.SetActive(false);
    }

    // 일정 시간동안 반전
    private void CollideMushroom()
    {
        vehicleController.isMushroom = true;
        if (CoMushroom != null) StopCoroutine(CoMushroom);
        CoMushroom = StartCoroutine(CoCollideMushroom());
    }

    private IEnumerator CoCollideMushroom()
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleController.isMushroom = false;
        enableItem();
        gameObject.SetActive(false);
    }
}
