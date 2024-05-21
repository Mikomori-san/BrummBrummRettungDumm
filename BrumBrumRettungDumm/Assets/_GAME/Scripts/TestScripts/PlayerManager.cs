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
    private GameObject ambulancePrefab;
    public GameObject ambulanceParent;
    public Vector3 ambulanceLocalSpawnPoint;
    [SerializeField]
    private GameObject paramedicPrefab;
    public GameObject paramedicParent;
    public Vector3 paramedicLocalSpawnPoint;

    // Start is called before the first frame update
    void Awake()
    {
        inputSafe = InputSafe.instance;
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.EnableJoining();

        playerInputManager.onPlayerJoined += inputSafe.OnPlayerJoined;
        playerInputManager.onPlayerLeft += inputSafe.OnPlayerLeft;

        playerInputManager.playerPrefab = paramedicPrefab;
        PlayerInput paramedicInput = playerInputManager.JoinPlayer(playerIndex: playerInputManager.playerCount, controlScheme: inputSafe.paramedicInput.controlScheme.name, pairWithDevices: inputSafe.paramedicInput.devices);
        if (paramedicParent != null)
            paramedicInput.gameObject.transform.SetParent(paramedicParent.transform);
        paramedicInput.gameObject.transform.localPosition = paramedicLocalSpawnPoint;

        playerInputManager.playerPrefab = ambulancePrefab;
        PlayerInput ambulanceInput = playerInputManager.JoinPlayer(playerIndex: playerInputManager.playerCount, controlScheme: inputSafe.ambulanceInput.controlScheme.name, pairWithDevices: inputSafe.ambulanceInput.devices);
        if (ambulanceParent != null)
            ambulanceInput.gameObject.transform.SetParent(ambulanceParent.transform);
        ambulanceInput.gameObject.transform.localPosition = ambulanceLocalSpawnPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
