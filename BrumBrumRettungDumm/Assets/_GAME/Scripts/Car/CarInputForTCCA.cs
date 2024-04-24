using DavidJalbert.TinyCarControllerAdvance;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputForTCCA : MonoBehaviour
{
    public TCCAPlayer carController;

    [Tooltip("Whether to let this script control the vehicle.")]
    public bool enableInput = true;

    private float steering = 0f;
    private float motor = 0f;
    private float boostInput = 0f;
    private bool handbrake = false;

    public void Input_Throttle(InputAction.CallbackContext context)
    {
        motor = context.ReadValue<float>();
    }
    public void Input_Steering(InputAction.CallbackContext context)
    {
        steering = context.ReadValue<float>();
    }

    public void Input_Respawn(InputAction.CallbackContext context)
    {
        if (enableInput && carController != null && context.performed)
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
        boostInput = context.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
