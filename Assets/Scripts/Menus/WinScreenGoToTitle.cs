using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenGoToTitle : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Debug.Log("button pressed");
        SceneManager.LoadScene(0);
    }
}
