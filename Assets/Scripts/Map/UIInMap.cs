using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInMap : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Image item;

    [SerializeField] Sprite[] itemImages;

    private GameObject player;
    private NewVehicleSystem vehicleController;
    private ItemController itemController;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        vehicleController = player.GetComponent<NewVehicleSystem>();
        itemController = player.GetComponent<ItemController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 랩 / 전체 랩
        lapText.text = $"{MidPointManager.Instance.currentLap} / {MidPointManager.Instance.totalLaps}";
        
        // 현재 속도
        //speedText.text = vehicleController.VehicleStatus.maxSpeed.ToString();

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
            item = null;
        }
    }
}
