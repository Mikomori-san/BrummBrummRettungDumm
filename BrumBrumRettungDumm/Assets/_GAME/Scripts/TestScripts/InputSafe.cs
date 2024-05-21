using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSafe : MonoBehaviour
{
    public static InputSafe instance;

    public InputInstance ambulanceInput;
    public InputInstance paramedicInput;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public class InputInstance
    {
        public InputDevice[] devices;
        public InputControlScheme controlScheme;
        public static InputControlScheme? FindControlScheme(InputDevice[] devices, Inputs inputs)
        {
            var controlScheme = InputControlScheme.FindControlSchemeForDevices(devices, inputs.controlSchemes);
            if (controlScheme == null)
            {
                Debug.Log("Control scheme not found");
                return null;
            }
            else
            {
                return controlScheme.Value;
            }
        }
    }
}
