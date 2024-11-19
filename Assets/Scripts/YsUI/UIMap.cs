using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMap : MonoBehaviour
{
    [SerializeField] private GameObject mapObject;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
    }

    public void OnMapRight()
    {
        if (mapObject.transform.localPosition.x > -1470 && time >= 1)
        {
            mapObject.transform.DOLocalMoveX(mapObject.transform.localPosition.x - 1460, 1);
            time = 0;
        }
        else
        {
            if (time >= 1)
            {
                mapObject.transform.DOLocalMoveX(0, 1);
                time = 0;
            }
        }
    }

    public void OnMapLeft()
    {
        if (mapObject.transform.localPosition.x < -10 && time >= 1)
        {
            mapObject.transform.DOLocalMoveX(mapObject.transform.localPosition.x + 1460, 1);
            time = 0;
        }
        else
        {
            if (time >= 1)
            {
                mapObject.transform.DOLocalMoveX(-2920, 1);
                time = 0;
            }
        }
    }
}
