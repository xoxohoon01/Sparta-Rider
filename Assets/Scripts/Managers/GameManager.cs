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
    
    public void LoadStartScene() => MySceneManager.Instance.ChangeScene(SceneType.StartScene);
    public void LoadMapChoiceScene() => MySceneManager.Instance.ChangeScene(SceneType.MapChoiceScene);
    public void LoadCarChoiceScene() => MySceneManager.Instance.ChangeScene(SceneType.CarChoiceScene);
    public void LoadLevelScene() => MySceneManager.Instance.ChangeScene(SceneType.LevelScene);

    public void LoadMainMapScene()
    {
        SceneManager.LoadScene($"Map{mapNumber}Scene");
    }
}
