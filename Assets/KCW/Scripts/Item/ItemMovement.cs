using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    private Vector3 moveDirection = Vector3.zero;
    private Renderer ren;
    private Collider col;
    private float itemSizeX;
    private float itemSizeY;
    private bool isBanana;

    private GameObject collisionCar;
    private VehicleStatus vehicleStatus;

    private void Awake()
    {
        ren = GetComponent<Renderer>();
        itemSizeX = ren.bounds.size.z;
        itemSizeY = ren.bounds.size.y;
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        // ItemType이 Move 이면 아이템 이동
        if(itemSO.itemType == ItemType.Move) transform.position += itemSO.speed * Time.deltaTime * moveDirection;
        
        // 바나나 밟으면 회전
        if (isBanana)
        {
            collisionCar.transform.Rotate(0f, 1080f / itemSO.durationTime * Time.deltaTime, 0f);
        }
    }

    // 움직이는 객체만 Move로 이동
    public void CheckMoveItem(Vector3 forward, VehicleStatus vehicle, PlayerInput input)
    {
        vehicleStatus = vehicle;
        SetPosition();
        if (itemSO.itemType == ItemType.Move) Move(forward);
    }

    // 이동 방향 설정
    public void Move(Vector3 forward)
    {
        moveDirection = forward;
    }

    private void SetPosition()
    {
        // 아이템 위치 계산
        if (itemSO.speed > 0) transform.position += itemSizeX * 0.5f * transform.forward;
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
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            disableItem();
            collisionCar = collision.gameObject;
            switch (itemSO.itemName)
            {
                case ItemName.Banana: CollideBanana(vehicleStatus.currentSpeed); break;
                case ItemName.Tomato: CollideTomato(); break;
                case ItemName.Coffee: CollideCoffee(vehicleStatus.currentSpeed); break;
                case ItemName.Cake: CollideCake(vehicleStatus.currentSpeed); break;
                case ItemName.Watermelon: CollideWatermelon(vehicleStatus.currentSpeed); break;
            }
        }
        
    }

    // 바나나 밟을 때
    private void CollideBanana(float initialSpeed)
    {
        isBanana = true;
        vehicleStatus.currentSpeed = 0f;
        StartCoroutine(CoCollideBanana(initialSpeed));
    }

    private IEnumerator CoCollideBanana(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        isBanana = false;
        vehicleStatus.currentSpeed = initialSpeed;
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
        vehicleStatus.acceleration *= 2f;
        StartCoroutine(CoCollideCoffee(initialSpeed));
    }


    private IEnumerator CoCollideCoffee(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleStatus.acceleration = initialSpeed;
    }

    private void CollideCake(float initialSpeed)
    {
        vehicleStatus.acceleration *= 0.5f;
        StartCoroutine(CoCollideCake(initialSpeed));
    }

    private IEnumerator CoCollideCake(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleStatus.acceleration = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }

    // 수박 맞으면 정해진 시간동안 멈춤
    private void CollideWatermelon(float initialSpeed)
    {
        vehicleStatus.acceleration = 0f;
    }

    private IEnumerator CoCollideWatermelon(float initialSpeed)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        vehicleStatus.acceleration = initialSpeed;
        enableItem();
        gameObject.SetActive(false);
    }
}
