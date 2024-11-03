using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] ghosts;
    public GameObject pacStudent;

    public Transform pellets;
    [SerializeField] private PacStudentController pacStudentController;

    public int score { get; private set; }
    public int lives { get; private set; }

    private InGameUI inGameUI;
    public bool isGameOver;

    [SerializeField] private AudioSource audioSource;
    public AudioClip pelletEatSound;

    private bool roundStarted = false;
    public float timerOffset;


    private void Start()
    {
        pacStudentController.GetComponent<PacStudentController>().enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        inGameUI = Object.FindFirstObjectByType<InGameUI>();
        NewGame();
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        roundStarted = false;

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        StartCoroutine(RoundStartCountdown());
    }

    private IEnumerator RoundStartCountdown()
    {
        // Show the countdown sequence with delays
        inGameUI.ShowCountdown("3");
        yield return new WaitForSeconds(1);

        inGameUI.ShowCountdown("2");
        yield return new WaitForSeconds(1);

        inGameUI.ShowCountdown("1");
        yield return new WaitForSeconds(1);

        inGameUI.ShowCountdown("GO!");
        yield return new WaitForSeconds(1);

        inGameUI.HideCountdown();

        StartRound();
    }

    private void StartRound()
    {
        roundStarted = true;
        timerOffset = Time.timeSinceLevelLoad;
        inGameUI.StartTimer();

        pacStudentController.GetComponent<PacStudentController>().enabled = true;
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].SetActive(true);
        }

        pacStudent.SetActive(true);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        pacStudentController.GetComponent<PacStudentController>().enabled = false;

        inGameUI.ShowGameOver();

        SaveHighScore();
        Invoke(nameof(ReturnToStartScene), 3.0f);
    }

    private void SetScore(int score)
    {
        this.score = score;
        inGameUI.UpdateScoreText(this.score);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        SetScore(score + ghost.points);
    }

    public void PacStudentEaten()
    {
        pacStudent.gameObject.SetActive(false);

        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void PelletCollected(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);
        PlaySound(pelletEatSound);

        if (!HasRemainingPellets())
        {
            pacStudent.gameObject.SetActive(false);
            GameOver();
        }
    }

    public void PowerPelletCollected(PowerPellet powerPellet)
    {
        PelletCollected(powerPellet);
        PlaySound(pelletEatSound);

        // Change the state of the ghosts
    }

    public void CherryCollected(Cherry cherry)
    {
        PelletCollected(cherry);
        PlaySound(pelletEatSound);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0);

        float currentTime = Time.timeSinceLevelLoad - timerOffset;
        if (score > highScore || (score == highScore && currentTime < bestTime))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.SetFloat("BestTime", currentTime);
            PlayerPrefs.Save();
        }
    }

    private void ReturnToStartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
