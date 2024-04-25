using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject ambulance;
    public GameObject paramedic;
    public InputActionProperty joinAction;

    private InputDevice deviceAmbulance;
    private InputDevice deviceParamedic;

    public int maxPlayers = 2;
    private int playerCount = 0;

    void Awake()
    {
        if (joinAction.action != null)
        {
            joinAction.action.performed += JoinPlayerFromAction;
            joinAction.action.Enable();
        }
        else
        {
            Debug.LogError(
                $"No join action configured on PlayerInputManager but join behavior is set to {nameof(PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered)}",
                this);
        }
    }

    public void JoinPlayerFromAction(InputAction.CallbackContext context)
    {
        if (!CheckIfPlayerCanJoin())
            return;

        var device = context.control.device;
        if (playerCount == 0)
        {
            deviceAmbulance = device;
            Debug.Log("Ambulance Joined");
        }
        else if (playerCount == 1)
        {
            deviceParamedic = device;
            Debug.Log("Paramedic Joined");
        }
        playerCount++;

        // After incrementing playerCount, check if both players have joined
        if (playerCount == maxPlayers)
        {
            // Enable the GameObjects
            ambulance.SetActive(true);
            paramedic.SetActive(true);

            // Switch the control schemes
            ambulance.GetComponent<PlayerInput>().SwitchCurrentControlScheme(deviceAmbulance);
            paramedic.GetComponent<PlayerInput>().SwitchCurrentControlScheme(deviceParamedic);
        }
    }


    private bool CheckIfPlayerCanJoin()
    {
        if (playerCount >= maxPlayers)
        {
            Debug.Log("Max players reached");
            return false;
        }
        return true;
    }
}
