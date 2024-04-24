using DavidJalbert.TinyCarControllerAdvance;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class CarInputForTCCA : MonoBehaviour
{
    public TCCAPlayer carController;

    [Tooltip("Whether to let this script control the vehicle.")]
    public bool enableInput = true;

    private float movementX = 0f;
    private float movementZ = 0f;
    private float boostInput = 0f;
    private bool handbrake = false;

    public void Input_Movement(InputAction.CallbackContext context)
    {
        movementX = context.ReadValue<Vector2>().x;
        movementZ = context.ReadValue<Vector2>().y;
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
            float motorDelta = movementZ;
            float steeringDelta = movementX;

            carController.setMotor(motorDelta);
            carController.setSteering(steeringDelta);
            carController.setHandbrake(handbrake);
            carController.setBoost(boostInput);
        }
    }
}
