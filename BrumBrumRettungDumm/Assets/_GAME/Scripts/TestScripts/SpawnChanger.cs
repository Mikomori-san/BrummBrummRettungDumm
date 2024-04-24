using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnChanger : MonoBehaviour
{
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

            GameObject interior = GameObject.FindGameObjectWithTag("Interior");
            interior.GetComponent<InteriorOrientation>().target = GameObject.FindGameObjectWithTag("AmbulanceBody");

            GameObject forceController = GameObject.Find("ForceController");
            forceController.GetComponent<ForceController>().SetTarget(GameObject.FindGameObjectWithTag("AmbulanceBody"));

            Debug.Log("Ambulance Joined");
        }
        else if(paramedicPlayer == null)
        {
            paramedicPlayer = playerInput.gameObject;
            GameObject interior = GameObject.FindGameObjectWithTag("Interior");
            paramedicPlayer.transform.parent = interior.transform;
            Debug.Log("Paramedic Joined");
        }
        else
        {
            playerInput.DeactivateInput();  // Deactivate the input of the player
        }
    }
}
