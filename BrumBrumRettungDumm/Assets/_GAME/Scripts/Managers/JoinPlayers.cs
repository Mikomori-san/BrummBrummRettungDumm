using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayers : MonoBehaviour
{
    private Inputs inputs;
    public PlayerController playerController;
    public CarInputForTCCA carController;

    public static event Action<string> OnPlayerJoined;

    public void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();

        inputs.Player.Join.started += JoinPlayer;
        inputs.Player.Leave.started += LeavePlayer;
        playerController.SetEnable(false);
        carController.enableInput = false;
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
        InputDevice[] inputDevices = { context.control.device };
        if (playerController.devices.Contains(context.control.device) || carController.devices.Contains(context.control.device))
        {
            Debug.Log("Player already joined");
            return;
        }

        if (carController.devices.Count == 0)
        {
            carController.SetUser(inputDevices);
            Debug.Log("Car joined");
            OnPlayerJoined?.Invoke("Car");
        }
        else if(playerController.devices.Count == 0)
        {
            playerController.SetUser(inputDevices);
            Debug.Log("Paramedic joined");
            OnPlayerJoined?.Invoke("Paramedic");
        }
        else
        {
            Debug.Log("No more players allowed");
            return;
        }
    }
    void LeavePlayer(InputAction.CallbackContext context)
    {
        //This is currently causing a bug as the Menu doesnt react to Leaving Players also SetUser Method doesnt support unbinding a device yet
        //if(playerController.device == context.control.device)
        //{
        //    playerController.enableInput = false;
        //    playerController.SetUser(null);
        //    Debug.Log("Paramedic left");
        //}
        //if(carController.device == context.control.device)
        //{
        //    carController.enableInput = false;
        //    carController.SetUser(null);
        //    Debug.Log("Car left");
        //}
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    void OnEnable()
    {
        inputs.Enable();
    }
    public void EnableInput()
    {
        playerController.SetEnable(true);
        carController.enableInput = true;
        enabled = false;
        Debug.Log("Input enabled");
    }
    public bool PlayersJoined()
    {
        return playerController.IsDevicesNotEmpty() && carController.IsDevicesNotEmpty();
    }
}
