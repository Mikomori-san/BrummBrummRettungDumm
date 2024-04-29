using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class DeviceInput : MonoBehaviour
{
    protected Inputs inputs;
    [HideInInspector]
    public List<InputDevice> devices
    {
        get;
        private set;
    }

    protected InputUser inputUser;

    protected void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();
        devices = new List<InputDevice>();
    }

    public void SetUser(InputDevice[] devices)
    {
        this.devices = devices.ToList();

        InputControlScheme? inputControlScheme = FindControlScheme(this.devices.ToArray(), inputs);
        if (inputControlScheme == null)
        {
            Debug.Log("Control scheme not found");
            return;
        }
        TryActivateControlScheme(inputControlScheme.Value, this.devices.ToArray());
        inputUser.AssociateActionsWithUser(inputs);
        inputUser.ActivateControlScheme(inputControlScheme.Value).AndPairRemainingDevices();
    }
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
    private bool TryActivateControlScheme(InputControlScheme controlScheme, InputDevice[] inputDevices)
    {
        foreach (var device in inputDevices)
        {
            if (!controlScheme.SupportsDevice(device))
                return false;

            inputUser = InputUser.PerformPairingWithDevice(device, inputUser);
        }
        return true;
    }
    public bool IsDevicesNotEmpty()
    {
        return devices.Count > 0;
    }
}
