using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSafe : MonoBehaviour
{
    private class InputInstance
    {
        public InputDevice[] devices;
        public InputControlScheme controlScheme;
    }

    public static InputSafe instance;

    private InputInstance ambulanceInput;
    private InputInstance paramedicInput;

    private GameObject ambulance;
    private GameObject paramedic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
    public InputDevice[] GetAmbulanceDevices()
    {
        return ambulanceInput.devices;
    }

    public InputControlScheme GetAmbulanceControlScheme()
    {
        return ambulanceInput.controlScheme;
    }

    public InputDevice[] GetParamedicDevices()
    {
        return paramedicInput.devices;
    }

    public InputControlScheme GetParamedicControlScheme()
    {
        return paramedicInput.controlScheme;
    }
    public void SetAmbulanceInput(InputDevice[] inputDevices, InputControlScheme controlScheme)
    {
        ambulanceInput = new InputInstance
        {
            devices = inputDevices,
            controlScheme = controlScheme
        };
    }
    public void SetParamedicInput(InputDevice[] inputDevices, InputControlScheme controlScheme)
    {
        paramedicInput = new InputInstance
        {
            devices = inputDevices,
            controlScheme = controlScheme
        };
    }
}
