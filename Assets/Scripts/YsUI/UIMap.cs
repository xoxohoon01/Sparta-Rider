using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMap : MonoBehaviour
{
    [SerializeField] private GameObject mapObject;

    public void OnMap()
    {
        if (mapObject.transform.localPosition.x > -1470)
        {
            mapObject.transform.DOLocalMoveX(mapObject.transform.localPosition.x - 1460, 1);
        }
        else
        {
            mapObject.transform.DOLocalMoveX(0, 1);
        }
        
    }
}
