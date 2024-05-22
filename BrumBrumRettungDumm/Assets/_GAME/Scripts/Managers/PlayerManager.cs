using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

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
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.EnableJoining();

        // Join paramedic
        playerInputManager.playerPrefab = paramedicPrefab;
        PlayerInput paramedicInput = playerInputManager.JoinPlayer(playerIndex: playerInputManager.playerCount, controlScheme: InputSafe.instance.GetParamedicControlScheme().name, pairWithDevices: InputSafe.instance.GetParamedicDevices());
        if (paramedicParent != null)
            paramedicInput.gameObject.transform.SetParent(paramedicParent.transform);
        paramedicInput.gameObject.transform.localPosition = paramedicLocalSpawnPoint;

        // Join ambulance
        playerInputManager.playerPrefab = ambulancePrefab;
        PlayerInput ambulanceInput = playerInputManager.JoinPlayer(playerIndex: playerInputManager.playerCount, controlScheme: InputSafe.instance.GetAmbulanceControlScheme().name, pairWithDevices: InputSafe.instance.GetAmbulanceDevices());
        if (ambulanceParent != null)
            ambulanceInput.gameObject.transform.SetParent(ambulanceParent.transform);
        ambulanceInput.gameObject.transform.localPosition = ambulanceLocalSpawnPoint;

        playerInputManager.DisableJoining();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += InputSafe.instance.OnPlayerJoined;
        playerInputManager.onPlayerLeft += InputSafe.instance.OnPlayerLeft;
    }
    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= InputSafe.instance.OnPlayerJoined;
        playerInputManager.onPlayerLeft -= InputSafe.instance.OnPlayerLeft;
    }
}
