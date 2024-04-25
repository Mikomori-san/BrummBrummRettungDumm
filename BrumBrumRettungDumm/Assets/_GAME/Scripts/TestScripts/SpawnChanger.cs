using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnChanger : MonoBehaviour
{
    public GameObject interior;
    public GameObject forceController;
    public GameObject gameController;


    public GameObject ambulancePrefab;
    public GameObject paramedicPrefab;

    private GameObject ambulancePlayer;
    private GameObject paramedicPlayer;

    private PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        //playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if(ambulancePlayer == null)
        {
            ambulancePlayer = playerInput.gameObject;
            playerInputManager.playerPrefab = paramedicPrefab;

            interior.GetComponent<InteriorOrientation>().target = GameObject.FindGameObjectWithTag("AmbulanceBody");
            forceController.GetComponent<ForceController>().SetTarget(GameObject.FindGameObjectWithTag("AmbulanceBody"));

            Debug.Log("Ambulance Joined");
        }
        else if(paramedicPlayer == null)
        {
            paramedicPlayer = playerInput.gameObject;
            paramedicPlayer.transform.parent = interior.transform;

            Debug.Log("Paramedic Joined");
        }
        else
        {
            playerInput.DeactivateInput();  // Deactivate the input of the player
        }
    }
}
