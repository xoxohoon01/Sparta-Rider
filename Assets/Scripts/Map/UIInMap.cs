using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInMap : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI currentLapTimeText;
    [SerializeField] private TextMeshProUGUI bestLapTimeText;
    [SerializeField] private Image item;

    [SerializeField] Sprite[] itemImages;

    private GameObject player;
    private Rigidbody rb;
    private VehicleController vehicleController;
    private ItemController itemController;

    private int kmPerHour = 5;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        vehicleController = player.GetComponent<VehicleController>();
        itemController = player.GetComponent<ItemController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 랩 / 전체 랩
        lapText.text = $"{MidPointManager.Instance.currentLap} / {MidPointManager.Instance.totalLaps}";

        // 현재 속도
        speedText.text = (rb.velocity.magnitude * kmPerHour).ToString("F0");

        // 현재 랩 시간
        float currentLapTime = MidPointManager.Instance.currentLapTime;
        currentLapTimeText.text = $"Current Lap\n{FormatTime(currentLapTime)}";

        // 최고 랩 시간
        float bestLapTime = MidPointManager.Instance.bestLapTime;
        string bestLapTimeString = FormatTime(bestLapTime);
        if (bestLapTime == float.MaxValue)
        {
            bestLapTimeString = FormatTime(currentLapTime);
        }
        bestLapTimeText.text = $"Best Lap\n{bestLapTimeString}";

        // 보유중인 아이템 이미지
        if (itemController.item != null)
        {
            // TODO : itemImage 바꾸기
            string itemName = itemController.itemSO.itemName.ToString();

            foreach (Sprite itemImage in itemImages)
            {
                if (itemName == itemImage.name)
                {
                    item.sprite = itemImage; break;
                }
            }
        }
        else
        {
            item.sprite = null;
        }
    }

    private string FormatTime(float amount)
    {
        string str = $"{(int)amount / 60}.{(amount % 60).ToString("F2")}";
        return str;
    }
}
