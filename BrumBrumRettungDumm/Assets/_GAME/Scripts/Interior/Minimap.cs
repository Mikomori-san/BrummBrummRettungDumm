using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Licensing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject minimapSocket;
    [SerializeField] private GameObject minimapCursor;
    [SerializeField] private GameObject marker;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float minimapActivationRange = 1.5f;
    [SerializeField] private float markerDeletionRange = 50;

    private bool minimapActive = false;
    private Simple3DMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
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
        if (context.started)
        {
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
                RaycastHit hit;
                Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                if (Physics.Raycast(ray, out hit, minimapActivationRange))
                {
                    if (hit.collider != null && hit.collider.gameObject.name == minimapSocket.name)
                    {
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
    }

    public void Input_SetMarker(InputAction.CallbackContext context)
    {
        if (context.started && minimapActive)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = minimapCamera.ScreenPointToRay(mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000, ground))
            {
                GameObject newMarker = Instantiate(marker, hit.point,Quaternion.identity);
                NavigationManager.Instance.AddMarker(ref newMarker);
            }
        }
    }
}
