using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject OptionsMenu;
    public GameObject PauseMenuUI;
    public void Start()
    {
        resume();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }

    public void pause()
    {
        GameIsPaused = true;
        PauseMenuUI.SetActive(GameIsPaused);
        Time.timeScale = 0f;
    }

    public void resume()
    {
        GameIsPaused = false;
        PauseMenuUI.SetActive(GameIsPaused);
        Time.timeScale = 1f;
    }

    public void Options()
    {
        PauseMenuUI.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void Back()
    {
        OptionsMenu.SetActive(false);
        PauseMenuUI.SetActive(true);
    }
    public void StartGame()
    {
        Debug.Log("button pressed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToMainMenu()
    {
        Debug.Log("button pressed");
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
