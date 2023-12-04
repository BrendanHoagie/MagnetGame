using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpauseButton : MonoBehaviour
{
    public GameObject PauseMenuUI;

    public void Unpause()
    {
        PauseMenu.GameIsPaused = false;
        PauseMenuUI.SetActive(PauseMenu.GameIsPaused);
        Time.timeScale = 1f;
    }
}
