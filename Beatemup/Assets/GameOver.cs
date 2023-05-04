using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{    [HideInInspector]
    public  bool GameIsOver = false;
    public GameObject gameOverMenuUI;
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void NewGame() {
        SceneManager.LoadScene("levelTestThomas");
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsOver = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void GameOverfunction() {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsOver = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void QuitGame() {
        Application.Quit();
    }
}
