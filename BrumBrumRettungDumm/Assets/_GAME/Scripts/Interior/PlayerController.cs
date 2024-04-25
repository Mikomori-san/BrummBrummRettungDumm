using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerController : MonoBehaviour
{
    private Inputs inputs;
    [HideInInspector]
    public InputDevice device
    {
        get;
        private set;
    }

    [Header("Movement")]
    public bool enableInput = true;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float gravity = -9.81f;
    private CharacterController characterController;

    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, -0.7f, 0);
    [SerializeField] private float groundCheckSize = 0.4f;
    [SerializeField] private float distanceToLookForSlope = 5f;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float jumpHeight = 5f;

    private float movementX = 0f;
    private float movementZ = 0f;
    float mouseX;
    float mouseY;
    private Vector3 velocity;
    private bool isGrounded = false;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 25f;
    [SerializeField] private float cameraClamp = 90f;
    private Camera cam;
    private float xRotation = 0f;

    [Header("GameObjects")]
    public GameObject gameController;

    private InputUser inputUser;

    private void Awake()
    {
        inputs = new Inputs();
        inputs.Enable();

        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        inputs.Player.Look.started += Input_Look;
        inputs.Player.Look.performed += Input_Look;
        inputs.Player.Look.canceled += Input_Look;

        inputs.Player.Move.started += Input_Movement;
        inputs.Player.Move.performed += Input_Movement;
        inputs.Player.Move.canceled += Input_Movement;

        inputs.Player.Jump.started += Input_Jump;
        inputs.Player.Jump.performed += Input_Jump;
        inputs.Player.Jump.canceled += Input_Jump;

        inputs.Player.Grab.started += gameController.GetComponent<ObjectDragging>().Input_Grab;
        inputs.Player.Grab.performed += gameController.GetComponent<ObjectDragging>().Input_Grab;
        inputs.Player.Grab.canceled += gameController.GetComponent<ObjectDragging>().Input_Grab;
        inputs.Player.Grab.started += gameController.GetComponent<Minimap>().Input_SetMarker;
        inputs.Player.Grab.performed += gameController.GetComponent<Minimap>().Input_SetMarker;
        inputs.Player.Grab.canceled += gameController.GetComponent<Minimap>().Input_SetMarker;

        inputs.Player.Give.started += gameController.GetComponent<PillManager>().Input_GivePill;
        inputs.Player.Give.performed += gameController.GetComponent<PillManager>().Input_GivePill;
        inputs.Player.Give.canceled += gameController.GetComponent<PillManager>().Input_GivePill;
        inputs.Player.Give.started += gameController.GetComponent<DefibrilatorTask>().Input_GiveDefi;
        inputs.Player.Give.performed += gameController.GetComponent<DefibrilatorTask>().Input_GiveDefi;
        inputs.Player.Give.canceled += gameController.GetComponent<DefibrilatorTask>().Input_GiveDefi;
        inputs.Player.Give.started += gameController.GetComponent<Minimap>().Input_InteractWithMinimap;
        inputs.Player.Give.performed += gameController.GetComponent<Minimap>().Input_InteractWithMinimap;
        inputs.Player.Give.canceled += gameController.GetComponent<Minimap>().Input_InteractWithMinimap;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableInput)
            return;
        if(device == null)
            return;

        //Camera Look
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * mouseX);

        //Movement
        isGrounded = Physics.CheckSphere(this.transform.position + groundCheckOffset, groundCheckSize, groundMask);

        if (isGrounded && velocity.y < 0) { velocity.y = 0f; }

        if (movementX != 0 || movementZ != 0)
        {
            Vector3 movementDirection = transform.right * movementX + transform.forward * movementZ;
            //print(movementDirection);
            movementDirection = AdjustVelocityToSlope(movementDirection);
            //print(movementDirection);
            characterController.Move(movementDirection * movementSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        if (Physics.Raycast(new Ray(this.transform.position, Vector3.down), out RaycastHit hitInfo, distanceToLookForSlope, groundMask))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;
            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position + groundCheckOffset, groundCheckSize);
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0, distanceToLookForSlope, 0));
    }
    private void Input_Look(InputAction.CallbackContext context)
    {
        if(!enableInput)
            return;
        if (device.displayName == "Keyboard" && context.control.device.displayName == "Mouse")
        {
            mouseX = context.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
            mouseY = context.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;
        }
        else if(context.control.device == device)
        {
            mouseX = context.ReadValue<Vector2>().x;
            mouseY = context.ReadValue<Vector2>().y;
        }
    }
    public void Input_Movement(InputAction.CallbackContext context)
    {
        if (!enableInput)
            return;
        if (context.control.device == device)
        {
            movementX = context.ReadValue<Vector2>().x;
            movementZ = context.ReadValue<Vector2>().y;
        }
    }
    public void Input_Jump(InputAction.CallbackContext context)
    {
        if (!enableInput)
            return;
        if (isGrounded && context.performed && context.control.device == device)
        {
            //print("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
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
}
