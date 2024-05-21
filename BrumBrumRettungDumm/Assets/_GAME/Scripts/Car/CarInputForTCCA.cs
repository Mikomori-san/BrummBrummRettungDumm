using DavidJalbert.TinyCarControllerAdvance;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CarInputForTCCA : MonoBehaviour
{
    public TCCAPlayer carController;

    private float steering = 0f;
    private float motor = 0f;
    private float boostInput = 0f;
    private bool handbrake = false;


    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += Input_Throttle;
        playerInput.onActionTriggered += Input_Steering;
        playerInput.onActionTriggered += Input_Respawn;
        playerInput.onActionTriggered += Input_Handbrake;
        playerInput.onActionTriggered += Input_Boost;
    }
    // Update is called once per frame
    void Update()
    {
        if (carController != null)
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
        if(context.action.name != "CarThrottle")
            return;

        motor = context.ReadValue<float>();
    }
    public void Input_Steering(InputAction.CallbackContext context)
    {
        if(context.action.name != "CarSteering")
            return;

        steering = context.ReadValue<float>();
    }
    public void Input_Respawn(InputAction.CallbackContext context)
    {
        if(context.action.name != "CarRespawn")
            return;

        if (context.started)
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
        if(context.action.name != "CarHandbrake")
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
        if (context.action.name != "CarBoost")
            return;

        boostInput = context.ReadValue<float>();
    }
}
