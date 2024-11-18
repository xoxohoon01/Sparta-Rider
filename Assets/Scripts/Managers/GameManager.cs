using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    StartScene,
    MapChoiceScene,
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

    // Button 전용 메서드
    public void LoadStartScene() => LoadScene(SceneType.StartScene);
    public void LoadMapChoiceScene() => LoadScene(SceneType.MapChoiceScene);
    public void LoadCarScene() => LoadScene(SceneType.CarScene);
    public void LoadLevelScene() => LoadScene(SceneType.LevelScene);
    public void LoadMainScene() => LoadScene(SceneType.MainScene);
}
