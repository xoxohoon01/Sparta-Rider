using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MySceneManager : MonoSingleton<MySceneManager>
{
    public CanvasGroup fadeImg;
    private float fadeTime = 2;
    
    public void ChangeScene()
    {
        fadeImg.DOFade(1, fadeTime)
               .OnStart(() =>
               {
                   fadeImg.blocksRaycasts = true;
               });
    }
}
