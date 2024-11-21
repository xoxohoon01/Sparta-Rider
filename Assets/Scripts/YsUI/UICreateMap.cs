using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICreateMap : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    List<GameObject> buttons = new List<GameObject>();

    public void GetMaps()
    {
        OnDestroy();
        
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Maps");
        for (int i = 0; i < jsonFiles.Length; i++)
        {
            string json = jsonFiles[i].name;
            GameObject obj = Instantiate(buttonPrefab, transform);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = json;
            obj.GetComponent<UserMapButton>().SetUp(json);
            
            buttons.Add(obj);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i]);
        }
    }
}
