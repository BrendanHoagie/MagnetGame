using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    void Start()
    {
        resume();
    }

    // Update is called once per frame
    void Update()
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

    void pause()
    {
        GameIsPaused = true;
        PauseMenuUI.SetActive(GameIsPaused);
        Time.timeScale = 0f;
    }

    void resume()
    {
        GameIsPaused = false;
        PauseMenuUI.SetActive(GameIsPaused);
        Time.timeScale = 1f;
    }
}
