using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introMusic;
    public AudioClip normalGhostsMusic;


    void Start()
    {
        audioSource.volume = 0.1f;
        PlayIntroMusic();
    }

    void Update()
    {

    }

    void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.Play();
        Invoke("PlayNormalGhostsMusic", 3);
    }

    void PlayNormalGhostsMusic()
    {
        audioSource.clip = normalGhostsMusic;
        audioSource.Play();
        audioSource.loop = true;
    }
}
