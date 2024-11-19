using UnityEngine.SceneManagement;

public enum SceneType
{
    StartScene,
    MapChoiceScene,
    CarChoiceScene,
    LevelScene
}

public class GameManager : MonoSingleton<GameManager>
{
    public int mapNumber;
    public int carNumber;
    public int gameMode;        //0 = 스피드전, 1 = 아이템전
    
    public void LoadScene(SceneType sceneType)
    {
        SceneManager.LoadScene($"{sceneType.ToString()}");
    }
    
    public void LoadStartScene() => LoadScene(SceneType.StartScene);
    public void LoadMapChoiceScene() => LoadScene(SceneType.MapChoiceScene);
    public void LoadCarChoiceScene() => LoadScene(SceneType.CarChoiceScene);
    public void LoadLevelScene() => LoadScene(SceneType.LevelScene);

    public void LoadMainMapScene()
    {
        SceneManager.LoadScene($"Map{mapNumber}Scene");
    }
}
