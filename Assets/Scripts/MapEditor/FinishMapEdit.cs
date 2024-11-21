using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapData
{
    public string name;
    public List<GameObject> ground;
    public Stack<GameObject> prefabs;
}

public class FinishMapEdit : MonoBehaviour
{
    [SerializeField] InputField input;
    [SerializeField] PrefabSpawner spawner;

    private void Awake()
    {
        spawner = GameObject.FindGameObjectWithTag("MapEditor").GetComponent<PrefabSpawner>();
    }

    public void OnConfirm()
    {
        //Todo: 맵 저장
        string mapName = input.text;
        Save(mapName);
        MySceneManager.Instance.ChangeScene(SceneType.StartScene);
    }

    private void Save(string mapName)
    {

    }
}
