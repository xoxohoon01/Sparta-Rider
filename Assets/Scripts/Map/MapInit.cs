using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInit : MonoBehaviour
{
    [SerializeField] GameObject mapPrefab;
    [SerializeField] GameObject map;

    private int mapSize = 20;

    private void Start()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                Instantiate(mapPrefab, new Vector3((mapSize/2 - j) * 15, -0.501f, (mapSize/2 - i) * 15), Quaternion.identity, map.transform);
            }
        }
    }
}
