using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    StartScene,
    MapScene,
    CarScene,
    LevelScene,
    MainScene
}

public class GameManager : MonoSingleton<GameManager>
{
    public void LoadScene(SceneType sceneType)
    {
        SceneManager.LoadScene($"{sceneType.ToString()}");
    }
}
