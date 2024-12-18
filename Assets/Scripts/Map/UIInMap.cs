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
    [SerializeField] private GameObject GameOverUI;

    [SerializeField] Sprite[] itemImages;

    private GameObject player;
    private Rigidbody rb;
    private VehicleController vehicleController;
    private ItemController itemController;

    private int kmPerHour = 5;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody>();
        vehicleController = player.GetComponent<VehicleController>();
        itemController = player.GetComponent<ItemController>();
        GameOverUI.SetActive(false);
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

        // 게임 클리어 시
        if(MidPointManager.Instance.isClear)
        {
            GameOverUI.SetActive(true);
        }
    }

    private string FormatTime(float amount)
    {
        string str = $"{(int)amount / 60}.{(amount % 60).ToString("F2")}";
        return str;
    }
}
