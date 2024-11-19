using System.Collections;
using UnityEngine;
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
    public GameObject tomatoCanvas;

    private void Awake()
    {
        ren = GetComponent<Renderer>();
        itemSizeX = ren.bounds.size.z;
        itemSizeY = ren.bounds.size.y;
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if(itemSO.speed > 0) transform.position += itemSO.speed * Time.deltaTime * moveDirection;
        if (isBanana)
        {
            collisionCar.transform.Rotate(0f, 1080f / itemSO.durationTime * Time.deltaTime, 0f);
        }
    }

    // 이동 방향 설정
    public void Move(Vector3 forward)
    {
        SetPosition();
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
            Debug.LogAssertion("Item Player Collision");
            disableItem();
            collisionCar = collision.gameObject;
            switch (itemSO.name)
            {
                case ItemName.Banana: CollideBanana(); break;
                case ItemName.Tomato: CollideTomato(); break;
                case ItemName.Coffee: return; break;
                case ItemName.Cake: break;
                case ItemName.Watermelon: break;
            }
        }
        
    }

    private void CollideBanana()
    {
        StartCoroutine(CoCollideBanana());
        isBanana = true;
    }

    private IEnumerator CoCollideBanana()
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        isBanana = false;
        enableItem();
        gameObject.SetActive(false);
    }

    private void CollideTomato()
    {
        tomatoCanvas.SetActive(true);
        StartCoroutine(CoCollideTomato(tomatoCanvas));
    }

    private IEnumerator CoCollideTomato(GameObject image)
    {
        yield return new WaitForSeconds(itemSO.durationTime);
        enableItem();
        image.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
