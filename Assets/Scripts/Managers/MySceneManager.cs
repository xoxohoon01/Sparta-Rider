using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoSingleton<MySceneManager>
{
    public CanvasGroup fadeImg;
    private float fadeTime = 0.7f;
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        fadeImg.DOFade(0, fadeTime)
               .OnStart(() => fadeImg.blocksRaycasts = false);
    }

    public void ChangeScene(SceneType sceneType)
    {
        fadeImg.DOFade(1, fadeTime)
               .OnStart(() =>
               {
                   fadeImg.blocksRaycasts = true;
               })
               .OnComplete(() =>
               {
                   GameManager.Instance.LoadScene(sceneType);
               });
    }
}
