using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Licensing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;
    private Camera playerCamera;
    [SerializeField] private GameObject minimapSocket;
    [SerializeField] private GameObject minimapCursor;
    [SerializeField] private GameObject marker;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float minimapActivationRange = 5f;
    [SerializeField] private float markerDeletionRange = 50;

    private bool minimapActive = false;
    private Simple3DMovement playerMovement;

    public float cursorSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = InputSafe.instance.GetParamedic().GetComponent<PlayerInput>();
        playerInput.onActionTriggered += Input_SetMarker;
        playerInput.onActionTriggered += Input_InteractWithMinimap;
        playerInput.onActionTriggered += Input_MoveCursor;

        playerCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
        minimapCursor.SetActive(false);
        UnityEngine.Cursor.visible = false;
        playerMovement = playerCamera.gameObject.GetComponentInParent<Simple3DMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Input_InteractWithMinimap(InputAction.CallbackContext context)
    {
        if(context.action.name != "Give")
            return;
        
        if (!context.started || ObjectDragging.Instance.grabbedObject) return;
        
        if (minimapActive)
        {
            minimapCamera.enabled = false;
            playerCamera.enabled = true;
                
            minimapActive = false;
                
            minimapCursor.SetActive(false);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
                
            playerMovement.enabled = true;
                
            print("Minimap Deactivated!");
        }
        else
        {
            var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            if (Physics.Raycast(ray, out var hit, minimapActivationRange))
            {
                print(hit.collider.gameObject.name);
                if (hit.collider != null && hit.collider.gameObject.name == minimapSocket.name)
                {
                    print("Should go into minimap mode!");
                    playerCamera.enabled = false;
                    minimapCamera.enabled = true;
                        
                    minimapActive = true;
                        
                    minimapCursor.SetActive(true);
                    UnityEngine.Cursor.visible = true;
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                        
                    playerMovement.enabled = false;
                        
                    print("Minimap Active!");
                }
            }
        }
    }

    public void Input_SetMarker(InputAction.CallbackContext context)
    {
        if(context.action.name != "Grab")
            return;
        
        if (!context.started || !minimapActive || ObjectDragging.Instance.grabbedObject) return;
        
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = minimapCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, 10000, ground))
        {
            GameObject newMarker = Instantiate(marker, hit.point,Quaternion.identity);
            NavigationManager.Instance.AddMarker(ref newMarker);
        }
    }

    public void Input_MoveCursor(InputAction.CallbackContext context)
    {
        if (context.action.name != "Look" || context.action.actionMap.asset.bindingMask.Value.groups != "Gamepad")
            return;

        if (minimapActive)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Mouse.current.WarpCursorPosition(Mouse.current.position.ReadValue() + input * cursorSpeed);
        }
    }
}
