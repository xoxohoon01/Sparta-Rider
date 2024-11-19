using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    StartScene,
    MapChoiceScene,
    CarChoiceScene,
    LevelScene,
    MainScene
}

public class GameManager : MonoSingleton<GameManager>
{
    public int mapNumber;
    
    public void LoadScene(SceneType sceneType)
    {
        SceneManager.LoadScene($"{sceneType.ToString()}");
    }
    
    public void LoadStartScene() => LoadScene(SceneType.StartScene);
    public void LoadMapChoiceScene() => LoadScene(SceneType.MapChoiceScene);
    public void LoadCarChoiceScene() => LoadScene(SceneType.CarChoiceScene);
    public void LoadLevelScene() => LoadScene(SceneType.LevelScene);
    public void LoadMainScene() => LoadScene(SceneType.MainScene);

    public void OnMapNumber(int number)
    {
        mapNumber = number;
    }
}
