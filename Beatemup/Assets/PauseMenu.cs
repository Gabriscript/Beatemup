using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;
    [HideInInspector]
    public  bool gameIspause = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (gameIspause)
                Resume();
            else
                Pause();
        
        
        }
    }
    public void Resume() {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIspause = false;
    }

    void Pause() {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIspause = true;

    }
    public void QuitGame() {
        Application.Quit();

    }

}
