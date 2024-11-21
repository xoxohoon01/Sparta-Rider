using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{    
    public void OnRestart()
    {
        // 멈춘 시간 초기화
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExit()
    {
        // 멈춘 시간 초기화
        Time.timeScale = 1.0f;
        MySceneManager.Instance.ChangeScene(SceneType.StartScene);
    }
}
