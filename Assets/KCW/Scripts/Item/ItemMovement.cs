using System.Collections;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Collider col;
    private float itemSizeZ;
    private float itemSizeY;

    private VehicleController vehicleController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    // 움직이는 객체만 Move로 이동
    public void CheckMoveItem(Vector3 forward, VehicleController vController)
    {
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
}
