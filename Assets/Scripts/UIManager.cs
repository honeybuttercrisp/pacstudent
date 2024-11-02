using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject quitButton = GameObject.FindGameObjectWithTag("QuitButton");
        if (quitButton != null)
        {
            Button buttonComponent = quitButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(QuitGame);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

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