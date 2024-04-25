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
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private GameObject ground;
    
    private bool minimapActive = false;
    private GameObject marker;
    private Simple3DMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        minimapCursor.SetActive(false);
        UnityEngine.Cursor.visible = false;
        marker = Instantiate(markerPrefab, ground.transform);
        marker.SetActive(false);   
        
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
                Vector3 screenMiddle = new Vector3(Screen.width / 2f, Screen.height / 2f, playerCamera.nearClipPlane);
                Ray ray = playerCamera.ScreenPointToRay(screenMiddle);

                float maxRange = 1.5f;

                if (Physics.Raycast(ray, out hit, maxRange))
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

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider && hit.collider.gameObject.name == ground.name)
                {
                    marker.transform.position = hit.point;
                    marker.SetActive(true);
                }
            }
        }
    }
}
