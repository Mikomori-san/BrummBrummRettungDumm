using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ObjectDragging : MonoBehaviour
{
    private Camera paramedicCamera;
    [SerializeField] private GameObject defibrilator;
    
    [HideInInspector] public bool isDragging = false;
    [HideInInspector] public GameObject grabbedObject;
    private ForceObjectLogic dragObjectForceObjectLogic;
    public static ObjectDragging Instance { get; private set; }

    private Vector3 oldPositionDefi;
    private Quaternion oldRotationDefi;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = InputSafe.instance.GetParamedic().GetComponent<PlayerInput>();
        playerInput.onActionTriggered += Input_Grab;

        paramedicCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void DragObject()
    {
        if (grabbedObject)
        {
            if(grabbedObject.GetComponent<ForceObjectLogic>())
                dragObjectForceObjectLogic.DisableForce();
            
            if(grabbedObject.GetComponent<Collider>())
                grabbedObject.GetComponent<Collider>().enabled = false;
            
            Quaternion cameraRotation = paramedicCamera.transform.rotation;

            Vector3 objectPosition = paramedicCamera.transform.position + paramedicCamera.transform.forward;

            grabbedObject.transform.position = objectPosition;
            grabbedObject.transform.rotation = cameraRotation;
        }
    }

    public void Input_Grab(InputAction.CallbackContext context)
    {
        if(context.action.name != "Grab")
            return;

        if (context.started)
        {
            RaycastHit hit;
            Ray ray = paramedicCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            float maxRange = 1.5f;
            
            if (Physics.Raycast(ray, out hit, maxRange))
            {
                
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
                {
                    if (hit.collider.gameObject.name == defibrilator.name)
                    {
                        oldPositionDefi = hit.collider.transform.position;
                        oldRotationDefi = hit.collider.transform.rotation;
                    }

                    grabbedObject = hit.collider.gameObject;
                    isDragging = true;
                    if(grabbedObject.GetComponent<ForceObjectLogic>())
                        dragObjectForceObjectLogic = grabbedObject.GetComponent<ForceObjectLogic>();
                }
            }
        }
        else if (context.canceled)
        {
            isDragging = false;
            if(grabbedObject)
            {
                if(grabbedObject.name == defibrilator.name)
                {
                    grabbedObject.transform.position = oldPositionDefi;
                    grabbedObject.transform.rotation = oldRotationDefi;
                }

                if (grabbedObject.GetComponent<ForceObjectLogic>())
                {
                    grabbedObject.GetComponent<ForceObjectLogic>().EnableForce();

                    grabbedObject.GetComponent<Rigidbody>().AddForce(paramedicCamera.transform.forward * 1.5f, ForceMode.Impulse);
                }

                if(grabbedObject.GetComponent<Collider>())
                    grabbedObject.GetComponent<Collider>().enabled = true;
            }

            grabbedObject = null;
        }
    }
}
