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
    Map1Scene,
    Map2Scene,
    Map3Scene
}

public class GameManager : MonoSingleton<GameManager>
{
    public int mapNumber;
    public int carNumber;
    
    public void LoadScene(SceneType sceneType)
    {
        SceneManager.LoadScene($"{sceneType.ToString()}");
    }
    
    public void LoadStartScene() => LoadScene(SceneType.StartScene);
    public void LoadMapChoiceScene() => LoadScene(SceneType.MapChoiceScene);
    public void LoadCarChoiceScene() => LoadScene(SceneType.CarChoiceScene);
    public void LoadLevelScene() => LoadScene(SceneType.LevelScene);
    public void LoadMap1Scene() => LoadScene(SceneType.Map1Scene);
    public void LoadMap2Scene() => LoadScene(SceneType.Map2Scene);
    public void LoadMap3Scene() => LoadScene(SceneType.Map3Scene);
}
