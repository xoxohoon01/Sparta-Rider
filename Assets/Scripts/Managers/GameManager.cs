using UnityEngine.SceneManagement;

public enum CarType
{
    Red,
    White,
    Brown,
    Yellow
}

public enum SceneType
{
    StartScene,
    MapChoiceScene,
    CarChoiceScene,
    LevelScene,
    UserMapScene,
    MapEditorScene
}

public class GameManager : MonoSingleton<GameManager>
{
    public int mapNumber;
    public CarType carNumber;
    public int gameMode;        //0 = 스피드전, 1 = 아이템전
    public string userMapName;
    
    public void LoadScene(SceneType sceneType)
    {
        SceneManager.LoadScene($"{sceneType.ToString()}");
    }
    
    public void LoadStartScene() => MySceneManager.Instance.ChangeScene(SceneType.StartScene);
    public void LoadMapChoiceScene() => MySceneManager.Instance.ChangeScene(SceneType.MapChoiceScene);
    public void LoadCarChoiceScene() => MySceneManager.Instance.ChangeScene(SceneType.CarChoiceScene);
    public void LoadLevelScene() => MySceneManager.Instance.ChangeScene(SceneType.LevelScene);
    public void LoadUserMapScene() => MySceneManager.Instance.ChangeScene(SceneType.UserMapScene);
    public void LoadMapEditorScene() => MySceneManager.Instance.ChangeScene(SceneType.MapEditorScene);

    public void LoadMainMapScene()
    {
        SoundManager.Instance.OnInGameBGM();
        SceneManager.LoadScene($"Map{mapNumber}Scene");
    }
}
