using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;

public class MainMenu : MonoBehaviour
{
    public JoinPlayers joinPlayers;
    public TextMeshProUGUI ambulanceJoinText;
    public TextMeshProUGUI paramedicJoinText;
    public TextMeshProUGUI infoText;
    public GameObject gameController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        if(joinPlayers.PlayersJoined())
        {
            joinPlayers.EnableInput();
            this.gameObject.SetActive(false);
        }
        else
        {
            infoText.text = "Both players must join to start the game";
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void HandlePlayerJoined(string player)
    {
        if (player == "Car")
        {
            ambulanceJoinText.text = "Ambulance joined";
            paramedicJoinText.enabled = true;
        }
        else if (player == "Paramedic")
        {
            paramedicJoinText.text = "Paramedic joined";
        }
    }
    private void OnEnable()
    {
        gameController.SetActive(false);
        paramedicJoinText.enabled = false;
        JoinPlayers.OnPlayerJoined += HandlePlayerJoined;
    }
    private void OnDisable()
    {
        gameController.SetActive(true);
        JoinPlayers.OnPlayerJoined -= HandlePlayerJoined;
    }
}
