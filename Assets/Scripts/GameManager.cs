using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject[] ghosts;
    public GameObject pacStudent;
    [SerializeField] public GameObject pacStudentController;
    public Tweener tweener;

    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }

    private InGameUI inGameUI;
    public bool isGameOver;

    [SerializeField] private AudioSource audioSource;
    public AudioClip pelletEatSound;
    public AudioClip deathSound;


    private bool roundStarted = false;
    public float timerOffset;


    private void Start()
    {
        pacStudentController.gameObject.SetActive(false);

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

        pacStudentController.gameObject.SetActive(true);
    }

    private void RespawnPacStudent()
    {
        pacStudent.GetComponent<BoxCollider2D>().enabled = true;
        pacStudent.GetComponent<Animator>().SetBool("IsDead", false);
        pacStudent.transform.position = new Vector3(-9.5f, 3.5f, 0);
        pacStudentController.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        pacStudentController.gameObject.SetActive(false);

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
        PlaySound(pelletEatSound);
    }

    public void PacStudentSlain()
    {
        tweener.ClearTweens();
        pacStudentController.gameObject.SetActive(false);
        pacStudent.GetComponent<BoxCollider2D>().enabled = false;
        PlaySound(deathSound);
        pacStudent.GetComponent<Animator>().SetBool("IsDead", true);
        SetLives(lives - 1);
        inGameUI.UpdateLives(lives);

        if (lives > 0)
        {
            Invoke(nameof(RespawnPacStudent), 3.0f);
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
            GameOver();
        }
    }

    public void PowerPelletCollected(PowerPellet powerPellet)
    {
        PelletCollected(powerPellet);
        PlaySound(pelletEatSound);

        // Set all ghosts to the Scared state
        foreach (GameObject ghost in ghosts)
        {
            Ghost ghostComponent = ghost.GetComponent<Ghost>();
            if (ghostComponent != null)
            {
                ghostComponent.SetScared();
            }
        }

        inGameUI.ShowGhostTimer(10);

        StartCoroutine(GhostScaredTimer());
    }

    private IEnumerator GhostScaredTimer()
    {
        int timer = 10;

        // Play scared music when timer starts
        AudioManager.Instance.PlayScaredGhostsMusic();

        while (timer > 0)
        {
            if (timer == 3)
            {
                // Change ghosts to Recovering at 3 seconds left
                foreach (GameObject ghost in ghosts)
                {
                    Ghost ghostComponent = ghost.GetComponent<Ghost>();
                    if (ghostComponent != null && ghostComponent.currentState == Ghost.GhostState.Scared)
                    {
                        ghostComponent.SetRecovering();
                    }
                }

            }

            inGameUI.UpdateGhostTimer(timer);
            yield return new WaitForSeconds(1);
            timer--;
        }

        inGameUI.HideGhostTimer();

        // Set all ghosts back to Normal state
        foreach (GameObject ghost in ghosts)
        {
            Ghost ghostComponent = ghost.GetComponent<Ghost>();
            if (ghostComponent != null && ghostComponent.currentState == Ghost.GhostState.Recovering)
            {
                ghostComponent.SetNormal();
            }
        }

        // Change back to normal music
        AudioManager.Instance.PlayNormalGhostsMusic();
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
