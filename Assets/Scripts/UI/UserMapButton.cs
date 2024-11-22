using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMapButton : MonoBehaviour
{
    private string index;
    
    public void SetUp(string idex)
    {
        index = idex;
    }
    
    public void OnClick()
    {
        GameManager.Instance.userMapName = index;
        GameManager.Instance.LoadUserMapScene();
    }
}
