using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] ghosts;
    public GameObject pacStudent;
    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }

    private InGameUI inGameUI;
    private bool isGameOver;

    private void Start()
    {
        inGameUI = Object.FindFirstObjectByType<InGameUI>();
        NewGame();
    }

    private void Update()
    {

    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
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

        if (!HasRemainingPellets())
        {
            pacStudent.gameObject.SetActive(false);
            GameOver();
        }
    }

    public void PowerPelletCollected(PowerPellet powerPellet)
    {
        PelletCollected(powerPellet);

        // Change the state of the ghosts
    }

    public void CherryCollected(Cherry cherry)
    {
        PelletCollected(cherry);
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

        float currentTime = Time.timeSinceLevelLoad;
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
