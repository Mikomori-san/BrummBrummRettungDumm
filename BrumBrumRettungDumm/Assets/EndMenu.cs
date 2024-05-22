using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public string mainMenuScene;
    public string joinMenuScene;
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    public void GoToJoinMenu()
    {
        SceneManager.LoadScene(joinMenuScene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
