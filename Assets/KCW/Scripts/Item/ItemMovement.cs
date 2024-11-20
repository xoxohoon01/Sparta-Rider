using System.Collections;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Renderer ren;
    private Collider col;
    private float itemSizeX;
    private float itemSizeY;
    private bool isBanana;

    private GameObject collisionCar;
    private Rigidbody carRigidbody;
    private VehicleStatus vehicleStatus;

    float rotateY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ren = GetComponent<Renderer>();
        itemSizeX = ren.bounds.size.z;
        itemSizeY = ren.bounds.size.y;
        col = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (itemSO.itemType == ItemType.Move)
        {
            rb.velocity = moveDirection * itemSO.speed;
        }

        // 바나나 밟으면 회전
        if (isBanana)
        {
            rotateY += 1080f / itemSO.durationTime * Time.fixedDeltaTime;
            carRigidbody.MoveRotation(Quaternion.Euler(0f, rotateY, 0f));
        }
    }

    // 움직이는 객체만 Move로 이동
    public void CheckMoveItem(Vector3 forward, VehicleStatus vehicle)
    {
        rotateY = 0f;
        vehicleStatus = vehicle;
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
        if (itemSO.itemType == ItemType.Move) transform.position += itemSizeX * 0.5f * transform.forward;
        else transform.position += itemSizeX * 0.5f * -transform.forward;
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || 
            collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            disableItem();
            collisionCar = collision.gameObject;
            carRigidbody = collisionCar.GetComponent<Rigidbody>();
            switch (itemSO.itemName)
            {
                case ItemName.Banana: CollideBanana(collisionCar.GetComponent<VehicleController>().carSpeed); break;
                case ItemName.Tomato: CollideTomato(); break;
                case ItemName.Coffee: CollideCoffee(collisionCar.GetComponent<VehicleController>().carSpeed); break;
                case ItemName.Cake: CollideCake(collisionCar.GetComponent<VehicleController>().carSpeed); break;
                case ItemName.Watermelon: CollideWatermelon(collisionCar.GetComponent<VehicleController>().carSpeed); break;
            }
        }
    }

    // 바나나 밟을 때
    private void CollideBanana(float initialSpeed)
    {
        isBanana = true;
        collisionCar.GetComponent<VehicleController>().carSpeed = 0f;
        StartCoroutine(CoCollideBanana(initialSpeed));
    }

    private IEnumerator CoCollideBanana(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        isBanana = false;
        collisionCar.GetComponent<VehicleController>().carSpeed = initialSpeed;
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
        collisionCar.GetComponent<VehicleController>().accelerationMultiplier *= 2f;
        StartCoroutine(CoCollideCoffee(initialSpeed));
    }

    private IEnumerator CoCollideCoffee(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        collisionCar.GetComponent<VehicleController>().accelerationMultiplier = initialSpeed;
    }

    private void CollideCake(float initialSpeed)
    {
        collisionCar.GetComponent<VehicleController>().accelerationMultiplier *= 0.5f;
        StartCoroutine(CoCollideCake(initialSpeed));
    }

    private IEnumerator CoCollideCake(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        collisionCar.GetComponent<VehicleController>().accelerationMultiplier = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }

    // 수박 맞으면 정해진 시간동안 멈춤
    private void CollideWatermelon(float initialSpeed)
    {
        collisionCar.GetComponent<VehicleController>().accelerationMultiplier = 0f;
    }

    private IEnumerator CoCollideWatermelon(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        collisionCar.GetComponent<VehicleController>().accelerationMultiplier = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }
}
