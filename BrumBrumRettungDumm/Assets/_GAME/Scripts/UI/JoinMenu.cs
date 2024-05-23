using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

[RequireComponent(typeof(AudioSource))]
public class JoinMenu : MonoBehaviour
{
    [Serializable]
    public class Player
    {
        [HideInInspector] public List<InputDevice> inputDevices = new List<InputDevice>();
        public Image image;
        public TextMeshProUGUI text;
        [HideInInspector] public bool ready = false;
    }
    private Inputs inputs;

    public InputActionAsset inputActions;
    public Player[] players;

    [Header("UI")]
    public Sprite driverSprite;
    public Sprite paramedicSprite;
    [SerializeField] private TextMeshProUGUI infoText;
    public Color notJoinedColor = new Color(0.1f, 0.1f, 0.1f, 1f);
    public Color joinedColor = new Color(1f, 1f, 1f, 1f);

    [Header("Scene")]
    public string sceneToLoad;

    [Header("Sounds")]
    private AudioSource audioSource;
    public AudioClip joinSound;
    public AudioClip leaveSound;
    public AudioClip clickSound;
    public AudioClip selectSound;
    public AudioClip errorSound;

    public void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayersJoined())
        {
            if (!PlayersHaveDifferentRoles())
                infoText.text = "Players must have different roles!";
            else
                infoText.text = "Ready up to begin!";
        }
        else
        {
            infoText.text = "Waiting for players to join...";
        }

        for (int i = 0; i < players.Length; i++)
        {
            UpdateImageColor(i);
            UpdatePlayerText(i);
        }

    }
    void JoinPlayer(InputAction.CallbackContext context)
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        inputDevices.Add(context.control.device);

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].inputDevices.Contains(inputDevices[0]))
            {
                return;
            }
        }

        if (inputDevices[0].displayName == "Keyboard")
        {
            InputDevice secondInputDevice = InputSystem.GetDevice("Mouse");
            inputDevices.Add(secondInputDevice);
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].inputDevices.Count == 0)
            {
                audioSource.PlayOneShot(joinSound);
                players[i].inputDevices = inputDevices;
                Debug.Log("Player " + i + " joined!");
                return;
            }
        }
    }
    void LeavePlayer(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].inputDevices.Contains(inputDevice))
            {
                audioSource.PlayOneShot(leaveSound);
                players[i].inputDevices.Clear();
                players[i].ready = false;
                return;
            }
        }
        Debug.Log("Player has not joined yet!");
    }
    void ReadyUp(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].inputDevices.Contains(inputDevice))
            {
                audioSource.PlayOneShot(clickSound);
                players[i].ready = !players[i].ready;
                break;
            }
        }

        if (PlayersReady())
        {
            if(!PlayersHaveDifferentRoles())
            {
                audioSource.PlayOneShot(errorSound);
                return;
            }
            Debug.Log("Game started");

            if (string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.LogError("Scene to load is not set!");
            }
            else
            {
                GameObject inputSafeGO = new GameObject("InputSafe");
                InputSafe inputSafe = inputSafeGO.AddComponent<InputSafe>();
                if (players[0].image.sprite == driverSprite)
                {
                    inputSafe.SetAmbulanceInput(players[0].inputDevices.ToArray(), InputControlScheme.FindControlSchemeForDevices(players[0].inputDevices.ToArray(), inputActions.controlSchemes).Value);
                    inputSafe.SetParamedicInput(players[1].inputDevices.ToArray(), InputControlScheme.FindControlSchemeForDevices(players[1].inputDevices.ToArray(), inputActions.controlSchemes).Value);
                }
                else
                {
                    inputSafe.SetAmbulanceInput(players[1].inputDevices.ToArray(), InputControlScheme.FindControlSchemeForDevices(players[1].inputDevices.ToArray(), inputActions.controlSchemes).Value);
                    inputSafe.SetParamedicInput(players[0].inputDevices.ToArray(), InputControlScheme.FindControlSchemeForDevices(players[0].inputDevices.ToArray(), inputActions.controlSchemes).Value);
                }

                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
    void ChangeRole(InputAction.CallbackContext context)
    {
        InputDevice inputDevice = context.control.device;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].inputDevices.Contains(inputDevice))
            {
                audioSource.PlayOneShot(selectSound);
                players[i].image.sprite = players[i].image.sprite == driverSprite ? paramedicSprite : driverSprite;
                players[i].ready = false;
                return;
            }
        }
    }
    void UpdateImageColor(int playerIndex)
    {
        players[playerIndex].image.color = PlayerJoined(playerIndex) ? joinedColor : notJoinedColor;
    }
    void UpdatePlayerText(int playerIndex)
    {
        if (!PlayerJoined(playerIndex))
        {
            players[playerIndex].text.text = "Press X to join";
        }
        else
        {
            players[playerIndex].text.text = players[playerIndex].image.sprite == driverSprite ? "< Driver >" : "< Paramedic >";
            players[playerIndex].text.text += players[playerIndex].ready ? "\n- Ready!" : "\n- Not Ready!";
        }
    }
    bool PlayerJoined(int playerIndex)
    {
        return players[playerIndex].inputDevices.Count != 0;
    }
    bool PlayersJoined()
    {
        foreach (Player player in players)
        {
            if (player.inputDevices.Count == 0)
            {
                return false;
            }
        }
        return true;
    }
    bool PlayersHaveDifferentRoles()
    {
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = i + 1; j < players.Length; j++)
            {
                if (players[i].image.sprite == players[j].image.sprite)
                {
                    return false;
                }
            }
        }
        return true;
    }
    bool PlayersReady()
    {
        foreach (Player player in players)
        {
            if (!player.ready)
            {
                return false;
            }
        }
        return true;
    }
    private void OnEnable()
    {
        inputs.UI.Submit.canceled += JoinPlayer;
        inputs.UI.Submit.started += ReadyUp;
        inputs.UI.Cancel.started += LeavePlayer;
        inputs.UI.Switch.started += ChangeRole;

        foreach (Player player in players)
        {
            player.ready = false;
            player.inputDevices = new List<InputDevice>();
        }
    }
    private void OnDisable()
    {
        inputs.UI.Submit.canceled -= JoinPlayer;
        inputs.UI.Submit.started -= ReadyUp;
        inputs.UI.Cancel.started -= LeavePlayer;
        inputs.UI.Switch.started -= ChangeRole;
    }
}