using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarChoice : MonoBehaviour
{
    [SerializeField] private GameObject carObject;
    private float time = 1;
    private int number = 0;

    private void Update()
    {
        time += Time.deltaTime;
    }
    
    public void OnRightClick()
    {
        if (carObject.transform.localPosition.x > -35 && time >= 1)
        {
            carObject.transform.DOLocalMoveX(carObject.transform.localPosition.x - 13, 1);
            time = 0;
            number += 1;
        }
        else
        {
            if (time >= 1)
            {
                carObject.transform.DOLocalMoveX(0, 1);
                time = 0;
                number = 0;
            }
        }
    }
    
    public void OnLeftClick()
    {
        if (carObject.transform.localPosition.x < -1 && time >= 1)
        {
            carObject.transform.DOLocalMoveX(carObject.transform.localPosition.x + 13, 1);
            time = 0;
            number -= 1;
        }
        else
        {
            if (time >= 1)
            {
                carObject.transform.DOLocalMoveX(-39, 1);
                time = 0;
                number = 3;
            }
        }
    }
    
    public void OnChoiceCar()
    {
        GameManager.Instance.carNumber = (CarType)number;
        GameManager.Instance.LoadLevelScene();
    }
}
