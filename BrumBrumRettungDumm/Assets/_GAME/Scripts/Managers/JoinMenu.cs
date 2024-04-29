using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class JoinMenu : MonoBehaviour
{
    private Inputs inputs;
    public List<InputDevice> player0Input;
    public List<InputDevice> player1Input;

    public static event Action<string> OnPlayerJoined;

    public void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();

        inputs.Player.Join.started += JoinPlayer;
        inputs.Player.Leave.started += LeavePlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        }
        else if(player1Input.Contains(inputDevice))
        {
            player1Input.Clear();
        }
        else
        {
            Debug.Log("Player has not joined the game yet.");
        }
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    void OnEnable()
    {
        inputs.Enable();
    }
    public void StartGame()
    {
        if (PlayersJoined())
        {
            Debug.Log("Game started");
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Log("Not all players joined");
        }
    }
    public bool PlayersJoined()
    {
        return player0Input.Count != 0 && player1Input.Count != 0;
    }
}
