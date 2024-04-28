using DavidJalbert.TinyCarControllerAdvance;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CarInputForTCCA : DeviceInput
{
    public TCCAPlayer carController;

    [Tooltip("Whether to let this script control the vehicle.")]
    public bool enableInput = true;

    private float steering = 0f;
    private float motor = 0f;
    private float boostInput = 0f;
    private bool handbrake = false;

    private new void Awake()
    {
        base.Awake();

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
        if(devices.Count == 0)
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
        if (!devices.Contains(context.control.device))
            return;

        motor = context.ReadValue<float>();
    }
    public void Input_Steering(InputAction.CallbackContext context)
    {
        if (!enableInput || carController == null)
            return;
        if (!devices.Contains(context.control.device))
            return;

        steering = context.ReadValue<float>();
    }
    public void Input_Respawn(InputAction.CallbackContext context)
    {
        if (!enableInput || carController == null)
            return;
        if (!devices.Contains(context.control.device))
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
        if (!devices.Contains(context.control.device))
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
        if (!devices.Contains(context.control.device))
            return;

        boostInput = context.ReadValue<float>();
    }
}
