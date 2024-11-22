using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneMove : MonoBehaviour
{
    public void OnStartClick()
    {
        GameManager.Instance.LoadMapChoiceScene();
    }
    
    public void OnCreateMap()
    {
        GameManager.Instance.LoadMapEditorScene();
    }
}
