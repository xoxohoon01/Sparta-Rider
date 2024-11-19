using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChoice : MonoBehaviour
{
    public void OnChoiceCar()
    {
        GameManager.Instance.LoadLevelScene();
    }
}
