using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introMusic;
    public AudioClip normalGhostsMusic;


    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = 0.1f;
        PlayIntroMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.Play();
        Invoke("PlayNormalGhostsMusic", introMusic.length);
    }

    void PlayNormalGhostsMusic()
    {
        audioSource.clip = normalGhostsMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
