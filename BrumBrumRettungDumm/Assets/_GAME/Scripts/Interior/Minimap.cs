using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    [SerializeField] private LayerMask markerLayer;

    private bool minimapActive = false;
    private Simple3DMovement playerMovement;

    public float cursorSpeed = 5;
    
    private Vector2 stickInput;
    public float mapDragSpeedMultiplier = 1f;
    public float viewMarginForDrag = 0.1f;

    [SerializeField] private float minOrthographicSize = 20;
    [SerializeField] private float maxOrthographicSize = 700;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = InputSafe.instance.GetParamedic().GetComponent<PlayerInput>();
        playerInput.onActionTriggered += Input_SetMarker;
        playerInput.onActionTriggered += Input_InteractWithMinimap;
        playerInput.onActionTriggered += Input_MoveCursor;
        playerInput.onActionTriggered += Input_RemoveMarker;
        playerInput.onActionTriggered += Input_Zoom;

        playerCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
        minimapCursor.SetActive(false);
        UnityEngine.Cursor.visible = false;
        playerMovement = playerCamera.gameObject.GetComponentInParent<Simple3DMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (minimapActive)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 viewportPoint = minimapCamera.ScreenToViewportPoint(mousePosition);
            // Check if the mouse is at of the camera viewport
            if (viewportPoint.x < 0 + viewMarginForDrag || viewportPoint.x > 1 - viewMarginForDrag || viewportPoint.y < 0 + viewMarginForDrag || viewportPoint.y > 1 - viewMarginForDrag)
            {
                //the closer the cursor to the edge of the viewport, the faster the camera moves
                float mapDragSpeed = 1 - Mathf.Clamp01(Mathf.Min(viewportPoint.x, 1 - viewportPoint.x, viewportPoint.y, 1 - viewportPoint.y) / viewMarginForDrag);

                minimapCamera.transform.position = new Vector3(minimapCamera.transform.position.x + (mousePosition.x - Screen.width / 4) * Time.deltaTime * mapDragSpeedMultiplier * mapDragSpeed, minimapCamera.transform.position.y, minimapCamera.transform.position.z + (mousePosition.y - Screen.height / 2) * Time.deltaTime * mapDragSpeedMultiplier * mapDragSpeed);
            }
            Mouse.current.WarpCursorPosition(Mouse.current.position.ReadValue() + stickInput * cursorSpeed);
        }
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
                    UnityEngine.Cursor.lockState = CursorLockMode.Confined;
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
            stickInput = context.ReadValue<Vector2>();
        }
    }
    public void Input_RemoveMarker(InputAction.CallbackContext context)
    {
        if(context.action.name != "RemoveMarker")
            return;
        
        if (!context.started || !minimapActive || ObjectDragging.Instance.grabbedObject) 
            return;
        
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = minimapCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, maxDistance:10000, markerLayer))
        {
            if (hit.collider != null)
            {
                NavigationManager.Instance.RemoveMarker(hit.collider.gameObject);
            }
        }
    }
    public void Input_Zoom(InputAction.CallbackContext context)
    {
        if(context.action.name != "Zoom")
            return;
        
        if (!minimapActive || ObjectDragging.Instance.grabbedObject) 
            return;

        minimapCamera.orthographicSize += context.ReadValue<float>() * 50;
        minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize, minOrthographicSize, maxOrthographicSize);
    }
}
