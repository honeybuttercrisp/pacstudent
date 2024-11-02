using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI gameOverText;


    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
        UpdateScoreText(gameManager.score);
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerText();
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateTimerText()
    {
        float time = Time.timeSinceLevelLoad;

        int mins = Mathf.FloorToInt(time / 60);
        int secs = Mathf.FloorToInt(time % 60);
        int millis = Mathf.FloorToInt((time * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", mins, secs, millis);
    }

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

}
