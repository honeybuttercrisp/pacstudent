using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainAudioSource;
    public AudioSource secondaryAudioSource;
    public AudioClip introMusic;
    public AudioClip normalGhostsMusic;
    public AudioClip scaredGhostsMusic;
    public AudioClip deadGhostsMusic;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (secondaryAudioSource == null)
        {
            secondaryAudioSource = gameObject.AddComponent<AudioSource>();
            secondaryAudioSource.volume = 0.1f;
        }
    }

    void Start()
    {
        mainAudioSource.volume = 0.5f;
        secondaryAudioSource.volume = 0.5f;
        PlayIntroMusic();
    }

    void PlayIntroMusic()
    {
        mainAudioSource.clip = introMusic;
        mainAudioSource.loop = false;
        mainAudioSource.Play();
        Invoke("PlayNormalGhostsMusic", 3);
    }

    public void PlayNormalGhostsMusic()
    {
        ChangeBGM(normalGhostsMusic, true);
    }

    public void PlayScaredGhostsMusic()
    {
        ChangeBGM(scaredGhostsMusic, true);
    }

    public void PlayDeadGhostsMusic()
    {
        StartCoroutine(PlayDeadGhostsMusicCoroutine());
    }

    private IEnumerator PlayDeadGhostsMusicCoroutine()
    {
        if (deadGhostsMusic != null)
        {
            secondaryAudioSource.clip = deadGhostsMusic;
            secondaryAudioSource.loop = true;
            secondaryAudioSource.Play();

            yield return new WaitForSeconds(5f);

            secondaryAudioSource.Stop();
        }
    }

    private void ChangeBGM(AudioClip newClip, bool shouldLoop)
    {
        if (newClip != null && mainAudioSource.clip != newClip)
        {
            mainAudioSource.Stop();
            mainAudioSource.clip = newClip;
            mainAudioSource.loop = shouldLoop;
            mainAudioSource.Play();
        }
    }
}