using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public string mainMenuScene;
    public string joinMenuScene;
    public TextMeshProUGUI scoreText;
    private void OnEnable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }
    private void Start()
    {
        scoreText.text = "Score: " + GetScore() + "\nHigh Score: " + GetHighScore();
    }
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
    private int GetScore()
    {
        int score = 0;
        int highScore = 0;
        ScoreSystem.GetScoresFromFile(out score, out highScore);
        return score;
    }
    private int GetHighScore()
    {
        int score = 0;
        int highScore = 0;
        ScoreSystem.GetScoresFromFile(out score, out highScore);
        return highScore;
    }
}
