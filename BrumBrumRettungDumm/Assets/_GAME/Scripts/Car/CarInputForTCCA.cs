using DavidJalbert.TinyCarControllerAdvance;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CarInputForTCCA : MonoBehaviour
{
    private Inputs inputs;
    [HideInInspector]
    public InputDevice device
    {
        get;
        private set;
    }
    public TCCAPlayer carController;

    [Tooltip("Whether to let this script control the vehicle.")]
    public bool enableInput = true;

    private float steering = 0f;
    private float motor = 0f;
    private float boostInput = 0f;
    private bool handbrake = false;

    private InputUser inputUser;

    private void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();

        inputs.Player.CarThrottle.started += Input_Throttle;
        inputs.Player.CarThrottle.performed += Input_Throttle;
        inputs.Player.CarThrottle.canceled += Input_Throttle;
        inputs.Player.CarSteering.started += Input_Steering;
        inputs.Player.CarSteering.performed += Input_Steering;
        inputs.Player.CarSteering.canceled += Input_Steering;
        inputs.Player.CarRespawn.performed += Input_Respawn;
        inputs.Player.CarHandbrake.started += Input_Handbrake;
        inputs.Player.CarHandbrake.performed += Input_Handbrake;
        inputs.Player.CarHandbrake.canceled += Input_Handbrake;
        inputs.Player.CarBoost.started += Input_Boost;
        inputs.Player.CarBoost.performed += Input_Boost;
        inputs.Player.CarBoost.canceled += Input_Boost;
    }
    // Update is called once per frame
    void Update()
    {
        if (!enableInput || carController == null)
            return;
        if(device == null)
            return;

        if (enableInput && carController != null)
        {
            float motorDelta = motor;
            float steeringDelta = steering;

            carController.setMotor(motorDelta);
            carController.setSteering(steeringDelta);
            carController.setHandbrake(handbrake);
            carController.setBoost(boostInput);
        }
    }

    public void Input_Throttle(InputAction.CallbackContext context)
    {
        if (!enableInput || carController == null)
            return;
        if (context.control.device != device)
            return;

        motor = context.ReadValue<float>();
    }
    public void Input_Steering(InputAction.CallbackContext context)
    {
        if (!enableInput || carController == null)
            return;
        if (context.control.device != device)
            return;

        steering = context.ReadValue<float>();
    }
    public void Input_Respawn(InputAction.CallbackContext context)
    {
        if (!enableInput || carController == null)
            return;
        if (context.control.device != device)
            return;

        if (context.performed)
        {
            carController.immobilize();
            carController.setPosition(carController.getInitialPosition());
            carController.setRotation(carController.getInitialRotation());

            foreach (TrailRenderer t in carController.GetComponentsInChildren<TrailRenderer>())
            {
                t.Clear();
            }
        }
    }
    public void Input_Handbrake(InputAction.CallbackContext context)
    {
        if(!enableInput || carController == null)
            return;
        if (context.control.device != device)
            return;

        if (context.performed)
        {
            handbrake = true;
        }
        else if (context.canceled)
        {
            handbrake = false;
        }
    }
    public void Input_Boost(InputAction.CallbackContext context)
    {
        if (!enableInput || carController == null)
            return;
        if (context.control.device != device)
            return;

        boostInput = context.ReadValue<float>();
    }
    public void SetUser(InputDevice device)
    {
        this.device = device;
        var controlScheme = InputControlScheme.FindControlSchemeForDevice(device, inputs.controlSchemes);
        InputControlScheme inputControlScheme;
        if (controlScheme == null)
        {
            Debug.Log("Control scheme not found");
            inputControlScheme = inputs.KeyboardMouseScheme;
        }
        else
        {
            inputControlScheme = controlScheme.Value;
        }
        if (!inputControlScheme.SupportsDevice(device))
        {
            Debug.Log("Device not supported");
            return;
        }
        inputUser = InputUser.PerformPairingWithDevice(device, inputUser);
        inputUser.AssociateActionsWithUser(inputs);
        inputUser.ActivateControlScheme(inputControlScheme).AndPairRemainingDevices();
    }
    //public void SetUser(InputDevice device)
    //{
    //    this.device = device;
    //    InputDevice[] inputDevices = { device };
    //    //var controlScheme = InputControlScheme.FindControlSchemeForDevice(device, inputs.controlSchemes);   //When the device is a KeyBoard , the control scheme is null, because the input mapping for KeyboardMouseScheme also contains mappings for the mouse. In order to get the correct control scheme this method needs to be passed an array of devices containing the Keyboard and Mouse device.
    //    InputControlScheme? inputControlScheme = FindControlScheme(inputDevices);
    //    if (inputControlScheme == null)
    //    {
    //        InputDevice secondInputDevice = InputSystem.GetDevice("Mouse");
    //        inputDevices = new InputDevice[] { device, secondInputDevice };
    //        inputControlScheme = FindControlScheme(inputDevices);
    //    }
    //    if (inputControlScheme == null)
    //    {
    //        Debug.Log("Control scheme not found");
    //        return;
    //    }
    //    if (!inputControlScheme.Value.SupportsDevice(device))
    //    {
    //        Debug.Log("Device not supported");
    //        return;
    //    }
    //    else
    //    {
    //        TryActivateControlScheme(inputControlScheme.Value, inputDevices);
    //        inputUser.AssociateActionsWithUser(inputs);
    //        inputUser.ActivateControlScheme(inputControlScheme.Value).AndPairRemainingDevices();
    //    }
    //}
    //private InputControlScheme? FindControlScheme(InputDevice[] devices)
    //{
    //    var controlScheme = InputControlScheme.FindControlSchemeForDevices(devices, inputs.controlSchemes);
    //    if(controlScheme == null)
    //    {
    //        Debug.Log("Control scheme not found");
    //        return null;
    //    }
    //    else
    //    {
    //        return controlScheme.Value;
    //    }
    //}
    //private bool TryActivateControlScheme(InputControlScheme controlScheme, InputDevice[] inputDevices)
    //{
    //    foreach (var device in inputDevices)
    //    {
    //        if (!controlScheme.SupportsDevice(device))
    //            return false;

    //        inputUser = InputUser.PerformPairingWithDevice(device, inputUser);
    //    }
    //    return true;
    //}
}
