using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip startBGM;
    [SerializeField] private AudioClip inGameBGM;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = startBGM;
        audioSource.Play();
    }

    public void OnInGameBGM()
    {
        audioSource.clip = inGameBGM;
        audioSource.Play();
    }
}
