using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class JoinMenu : MonoBehaviour
{
    private Inputs inputs;
    public List<InputDevice> player0Input = new List<InputDevice>();
    public List<InputDevice> player1Input = new List<InputDevice>();

    public Image player0Image;
    public Image player1Image;
    public Sprite driverSprite;
    public Sprite paramedicSprite;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI player0Text;
    public TextMeshProUGUI player1Text;

    private Color notJoinedColor = new Color(0.1f, 0.1f, 0.1f, 1f);
    private Color joinedColor = new Color(1f, 1f, 1f, 1f);

    public static event Action<string> OnPlayerJoined;
    public static event Action<string> OnPlayerLeft;

    public void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();

        inputs.UI.Submit.started += JoinPlayer;
        inputs.UI.Submit.started += StartGame;
        inputs.UI.Cancel.started += LeavePlayer;
        inputs.UI.Switch.started += ChangeRole;
    }

    // Start is called before the first frame update
    void Start()
    {
        player0Image.sprite = driverSprite;
        player1Image.sprite = paramedicSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayersJoined())
        {
            if(player0Image.sprite == player1Image.sprite)
                infoText.text = "Players must have different roles!";
            else
                infoText.text = "Press Start to begin!";
        }
        else
        {
            infoText.text = "Waiting for players to join...";
        }
        UpdateImageColor();
        UpdatePlayerText();
    }
    void JoinPlayer(InputAction.CallbackContext context)
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        inputDevices.Add(context.control.device);
        if (player0Input.Contains(inputDevices[0]) || player1Input.Contains(inputDevices[0]))
        {
            Debug.Log("Player already joined");
            return;
        }
        if (inputDevices[0].displayName == "Keyboard")
        {
            InputDevice secondInputDevice = InputSystem.GetDevice("Mouse");
            inputDevices.Add(secondInputDevice);
        }

        if (player0Input.Count == 0)
        {
            player0Input = inputDevices;
            Debug.Log("Player0 joined");
            OnPlayerJoined?.Invoke("Player0");
        }
        else if(player1Input.Count == 0)
        {
            player1Input = inputDevices;
            Debug.Log("Player1 joined");
            OnPlayerJoined?.Invoke("Player1");
        }
        else
        {
            Debug.Log("No more players allowed");
        }
    }
    void LeavePlayer(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;
        if(player0Input.Contains(inputDevice))
        {
            player0Input.Clear();
            OnPlayerLeft?.Invoke("Player0");
        }
        else if(player1Input.Contains(inputDevice))
        {
            player1Input.Clear();
            OnPlayerLeft?.Invoke("Player1");
        }
        else
        {
            Debug.Log("Player has not joined the game yet.");
        }
    }
    void StartGame(InputAction.CallbackContext context)
    {
        if(player0Image.sprite == player1Image.sprite)
        {
            Debug.Log("Players must have different roles");
            return;
        }
        if (!PlayersJoined())
        {
            Debug.Log("Not all players joined");
            return;
        }
        Debug.Log("Game started");
        SceneManager.LoadScene("Game");
    }
    void ChangeRole(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;
        if (player0Input.Contains(inputDevice))
        {
            player0Image.sprite = player0Image.sprite == driverSprite ? paramedicSprite : driverSprite;
        }
        else if (player1Input.Contains(inputDevice))
        {
            player1Image.sprite = player1Image.sprite == driverSprite ? paramedicSprite : driverSprite;
        }
    }
    void UpdateImageColor()
    {
        player0Image.color = player0Input.Count == 0 ? notJoinedColor : joinedColor;
        player1Image.color = player1Input.Count == 0 ? notJoinedColor : joinedColor;
    }
    void UpdatePlayerText()
    {
        if(player0Input.Count == 0)
        {
            player0Text.text = "Press X to join";
        }
        else
        {
            player0Text.text = player0Image.sprite == driverSprite ? "< Driver >" : "< Paramedic >";
        }
        if(player1Input.Count == 0)
        {
            player1Text.text = "Press X to join";
        }
        else
        {
            player1Text.text = player1Image.sprite == driverSprite ? "< Driver >" : "< Paramedic >";
        }
    }
    public bool PlayersJoined()
    {
        return player0Input.Count != 0 && player1Input.Count != 0;
    }
    private void OnEnable()
    {
        player0Input.Clear();
        player1Input.Clear();
    }
}
