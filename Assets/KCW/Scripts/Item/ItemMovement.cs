using System.Collections;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Renderer ren;
    private Collider col;
    private float itemSizeZ;
    private float itemSizeY;

    private VehicleController vehicleController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ren = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        itemSizeZ = col.bounds.size.z;
        itemSizeY = col.bounds.size.y;
    }

    private void FixedUpdate()
    {
        if (itemSO.itemType == ItemType.Move)
        { 
            rb.velocity = moveDirection * itemSO.speed;
        }
    }

    public void SetVehicleController(VehicleController vController)
    {
        vehicleController = vController;
        vehicleController.totalRotate = 0f;
        vehicleController.itemSO = itemSO;
    }

    // 움직이는 객체만 Move로 이동
    public void CheckMoveItem(Vector3 forward, VehicleController vController)
    {
        SetVehicleController(vController);
        SetPosition();
        if (itemSO.itemType == ItemType.Move) Move(forward);
    }

    // 이동 방향 설정
    private void Move(Vector3 forward)
    {
        moveDirection = forward;
    }

    private void SetPosition()
    {
        // 아이템 위치 계산
        if (itemSO.itemType == ItemType.Move) transform.position += itemSizeZ * 1f * transform.forward;
        else transform.position += itemSizeZ * 0.5f * -transform.forward;
        transform.position += itemSizeY * 0.5f * Vector3.up + new Vector3(0, 0.1f, 0);
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || 
            collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            disableItem();

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
