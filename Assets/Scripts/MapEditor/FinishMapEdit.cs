using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

[Serializable]
public class MapData
{
    public string mapName;
    public List<Data> data;
}

[Serializable]
public class Data
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
}

// 프리팹 종류, 위치, 방향, 스케일

public class FinishMapEdit : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    private TMP_Text placeholderText;
    public MapData mapData;
    PrefabSpawner spawner;

    private void Awake()
    {
        spawner = GameObject.FindGameObjectWithTag("MapEditor").GetComponent<PrefabSpawner>();
        placeholderText = input.placeholder.GetComponent<TMP_Text>();
    }

    public void OnConfirm()
    {
        string mapName = input.text;

        if (string.IsNullOrEmpty(mapName))
        {
            placeholderText.text = "Map Name cannot be empty";
            return;
        }

        Save(mapName);
        MySceneManager.Instance.ChangeScene(SceneType.StartScene);
    }

    private void Save(string name)
    {
        mapData.data.Clear();

        mapData.mapName = name;

        foreach (var currentPrefab in spawner.placedPrefab) {
            Data data = new Data
            {
                prefabName = currentPrefab.name,
                position = currentPrefab.transform.position,
                rotation = currentPrefab.transform.rotation
            };
            mapData.data.Add(data);
        }

        // TODO path 만들기 Json 직렬화하기 Write하기
        string json = JsonUtility.ToJson(mapData);
        string path = $"./Assets/Resources/Maps/{mapData.mapName}.json";
        File.WriteAllText(path, json);
    }
}
