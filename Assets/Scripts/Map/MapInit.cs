using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInit : MonoBehaviour
{
    [SerializeField] GameObject mapPrefab;
    [SerializeField] GameObject map;

    [SerializeField] private int mapSize = 20;
    [SerializeField] private int mapScale = 300;

    private void Start()
    {
        int pixelScale = mapScale / mapSize;
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                Instantiate(mapPrefab, new Vector3((mapSize/2 - j) * pixelScale, -0.5001f, (mapSize/2 - i) * pixelScale), Quaternion.identity, map.transform);
            }
        }
    }
}
