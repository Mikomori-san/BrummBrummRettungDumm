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
    Coroutine startGameCoroutine;

    public void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();

        inputs.Player.Join.performed += JoinPlayer;
        inputs.Player.Leave.performed += LeavePlayer;
        playerController.enableInput = false;
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
        if (playerController.device == context.control.device || carController.device == context.control.device)
        {
            Debug.Log("Player already joined");
            return;
        }

        if (carController.device == null)
        {
            carController.SetUser(context.control.device);
            Debug.Log("Car joined");
            if(playerController.device != null)
            {
                startGameCoroutine = StartCoroutine(StartGame());
            }
        }
        else if(playerController.device == null)
        {
            playerController.SetUser(context.control.device);
            Debug.Log("Paramedic joined");
            if(carController.device != null)
            {
                startGameCoroutine = StartCoroutine(StartGame());
            }
        }
        else
        {
            Debug.Log("No more players allowed");
            return;
        }
    }
    void LeavePlayer(InputAction.CallbackContext context)
    {
        if(playerController.device == context.control.device)
        {
            playerController.enableInput = false;
            playerController.SetUser(null);
            Debug.Log("Paramedic left");
            StopCoroutine(startGameCoroutine);
        }
        if(carController.device == context.control.device)
        {
            carController.enableInput = false;
            carController.SetUser(null);
            Debug.Log("Car left");
            StopCoroutine(startGameCoroutine);
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
    IEnumerator StartGame()
    {
        Debug.Log("Game starting in 3 seconds");
        yield return new WaitForSeconds(3);
        playerController.enableInput = true;
        carController.enableInput = true;
        enabled = false;
        Debug.Log("Game started");
    }
}
