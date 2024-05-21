using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSafe : MonoBehaviour
{
    public class InputInstance
    {
        public InputDevice[] devices;
        public InputControlScheme controlScheme;
    }

    public static InputSafe instance;

    public InputInstance ambulanceInput;
    public InputInstance paramedicInput;

    private GameObject ambulance;
    private GameObject paramedic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ambulanceInput = new InputInstance();
            paramedicInput = new InputInstance();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.gameObject.tag == "Ambulance")
        {
            ambulance = playerInput.gameObject;
        }
        else if (playerInput.gameObject.tag == "Paramedic")
        {
            paramedic = playerInput.gameObject;
        }
    }
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        if (playerInput.gameObject.tag == "Ambulance")
        {
            ambulance = null;
        }
        else if (playerInput.gameObject.tag == "Paramedic")
        {
            paramedic = null;
        }
    }
    public GameObject GetAmbulance()
    {
        return ambulance;
    }
    public GameObject GetParamedic()
    {
        return paramedic;
    }
}
