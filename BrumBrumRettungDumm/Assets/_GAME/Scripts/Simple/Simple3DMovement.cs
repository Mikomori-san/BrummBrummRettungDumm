using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Simple3DMovement : MonoBehaviour
{
    [Header("Movement")]
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
    private Vector3 velocity;
    private bool isGrounded = false;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 25f;
    [SerializeField] private float cameraClamp = 90f;
    private Camera cam;
    private float xRotation = 0f;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Input_Movement(InputAction.CallbackContext context)
    {
        movementX = context.ReadValue<Vector2>().x;
        movementZ = context.ReadValue<Vector2>().y;
    }

    public void Input_Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            //print("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        //Camera Look
        float mouseX = Mouse.current.delta.x.ReadUnprocessedValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadUnprocessedValue() * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -cameraClamp, cameraClamp);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * mouseX);

        //Movement
        isGrounded = Physics.CheckSphere(this.transform.position + groundCheckOffset, groundCheckSize, groundMask);

        if(isGrounded && velocity.y < 0) { velocity.y = 0f; }

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
        if(Physics.Raycast(new Ray(this.transform.position, Vector3.down), out RaycastHit hitInfo, distanceToLookForSlope, groundMask))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;
            if(adjustedVelocity.y < 0)
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
}