using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreateMap : MonoBehaviour
{
    public void GetMaps()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Maps");
        for (int i = 0; i < jsonFiles.Length; i++)
        {
            string json = jsonFiles[i].name;
            
        }
    }
}
