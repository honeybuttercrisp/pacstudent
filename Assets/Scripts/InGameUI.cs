using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private List<GameObject> lifeIcons;
    [SerializeField] private TextMeshProUGUI ghostTimerText;

    private GameManager gameManager;
    private bool timerRunning = false;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
        UpdateScoreText(gameManager.score);
        gameOverText.gameObject.SetActive(false);
        exitButton.onClick.AddListener(ReturnToStartScene);
        UpdateLives(gameManager.lives);
        ghostTimerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!gameManager.isGameOver && timerRunning)
        {
            UpdateTimerText();
        }
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateTimerText()
    {
        
        float elapsed = Time.timeSinceLevelLoad - gameManager.timerOffset;


        int mins = Mathf.FloorToInt(elapsed / 60);
        int secs = Mathf.FloorToInt(elapsed % 60);
        int millis = Mathf.FloorToInt((elapsed * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", mins, secs, millis);
    }

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    private void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void ShowCountdown(string text)
    {
        countdownText.gameObject.SetActive(true);
        countdownText.text = text;
    }

    public void HideCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }

    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeIcons.Count; i++)
        {
            lifeIcons[i].SetActive(i < lives);
        }
    }

    public void ShowGhostTimer(int seconds)
    {
        ghostTimerText.gameObject.SetActive(true);
        ghostTimerText.text = seconds.ToString(); 
    }

    public void UpdateGhostTimer(int seconds)
    {
        ghostTimerText.text = seconds.ToString(); 
    }

    public void HideGhostTimer()
    {
        ghostTimerText.gameObject.SetActive(false); 
    }
}
