using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeChoice : MonoBehaviour
{
    public void OnGameModeSelect(int number)
    {
        GameManager.Instance.gameMode = number;
        GameManager.Instance.LoadMainMapScene();
    }
}
