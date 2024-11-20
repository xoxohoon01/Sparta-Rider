using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInMap : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Image itemImage; 

    // Update is called once per frame
    void Update()
    {
        lapText.text = $"{MidPointManager.Instance.currentLap} / {MidPointManager.Instance.totalLaps}";
        // speedText.text = .ToString();
        //if(itemImage != null)
        //{
        //    // TODO : itemImage 바꾸기
        //}
        //else
        //{
        //    itemImage = null;
        //}
    }
}
