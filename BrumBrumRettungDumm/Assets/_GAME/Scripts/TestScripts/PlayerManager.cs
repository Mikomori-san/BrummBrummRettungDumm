using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private InputSafe inputSafe;

    [SerializeField]
    private GameObject ambulance;
    [SerializeField]
    private GameObject paramedic;

    public static event Action<string, PlayerInput> OnPlayerJoined;

    // Start is called before the first frame update
    void Start()
    {
        inputSafe = InputSafe.instance;
        playerInputManager = GetComponent<PlayerInputManager>();

        PlayerInput ambulanceInput = playerInputManager.JoinPlayer(playerIndex: playerInputManager.playerCount, splitScreenIndex: playerInputManager.playerCount, controlScheme: inputSafe.ambulanceInput.controlScheme.name, pairWithDevices: inputSafe.ambulanceInput.devices);
        ambulanceInput.transform.SetParent(ambulance.transform);
        ambulanceInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        ambulanceInput.camera = ambulance.GetComponentInChildren<Camera>();
        OnPlayerJoined.Invoke("ambulance", ambulanceInput);

        PlayerInput paramedicInput = playerInputManager.JoinPlayer(playerIndex: playerInputManager.playerCount, splitScreenIndex: playerInputManager.playerCount, controlScheme: inputSafe.paramedicInput.controlScheme.name, pairWithDevices: inputSafe.paramedicInput.devices);
        paramedicInput.transform.SetParent(paramedic.transform);
        paramedicInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        paramedicInput.camera = paramedic.GetComponentInChildren<Camera>();
        OnPlayerJoined.Invoke("paramedic", paramedicInput);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
