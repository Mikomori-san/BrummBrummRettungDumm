using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string JoinMenuScene;

    public void GoToJoinMenu()
    {
        SceneManager.LoadScene(JoinMenuScene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
