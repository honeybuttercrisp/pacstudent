using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    void Start()
    {
        LoadHighScore();

        GameObject quitButton = GameObject.FindGameObjectWithTag("QuitButton");
        if (quitButton != null)
        {
            Button buttonComponent = quitButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(QuitGame);
        }
    }

    private void LoadHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0);

        highScoreText.text = "High Score: " + highScore;
        bestTimeText.text = "Best Time: " + FormatTime(bestTime);

    }

    private string FormatTime(float time)
    {
        int mins = Mathf.FloorToInt(time / 60);
        int secs = Mathf.FloorToInt(time % 60);
        int millis = Mathf.FloorToInt((time * 100) % 100);
        return string.Format("{0:00}:{1:00}:{2:00}", mins, secs, millis);
    }

    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("RecreatedLevel", LoadSceneMode.Single);
        DontDestroyOnLoad(this.gameObject);
    }
}