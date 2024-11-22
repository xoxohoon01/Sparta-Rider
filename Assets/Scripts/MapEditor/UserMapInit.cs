using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserMapInit : MonoBehaviour
{
    public string mapName;

    // Start is called before the first frame update
    void Awake()
    {
        mapName = GameManager.Instance.userMapName;
        string path = $"./Assets/Resources/Maps/{mapName}.json";
        string json = File.ReadAllText(path);
        MapData mapData = JsonUtility.FromJson<MapData>(json);
        foreach (Data data in mapData.data) {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{data.prefabName}");
            Instantiate(prefab, data.position, data.rotation, transform);
        }
    }
}
