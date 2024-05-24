using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ObjectDragging : MonoBehaviour
{
    private Camera paramedicCamera;
    [SerializeField] private GameObject defibrilator;
    [SerializeField] private AudioSource defiPaddles;
    [SerializeField] private AudioClip defiPickUpSound;
    [SerializeField] private AudioClip itemPickUpSound;

    [SerializeField] private Transform defiPos;
    [SerializeField] private float maxRange = 1.5f;
    [HideInInspector] public bool isDragging = false;
    [HideInInspector] public GameObject grabbedObject;

    [SerializeField] private Image CrossHair;
    public Sprite normalCrossHair;
    public Vector3 normalCrossHairScale = Vector3.one;
    public Sprite grabableCrossHair;
    public Vector3 grabableCrossHairScale = Vector3.one;
    private GameObject targetedObject;
    
    private ForceObjectLogic dragObjectForceObjectLogic;
    public static ObjectDragging Instance { get; private set; }
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
        if(grabbedObject)
            return;
        
        RaycastHit hit;
        Ray ray = paramedicCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, maxRange))
        {
            if (hit.collider && hit.collider.gameObject.layer == LayerMask.NameToLayer("Draggable"))
            {
                CrossHair.sprite = grabableCrossHair;
                CrossHair.GetComponent<RectTransform>().localScale = grabableCrossHairScale;
                targetedObject = hit.collider.gameObject;
            }
            else
            {
                CrossHair.sprite = normalCrossHair;
                CrossHair.GetComponent<RectTransform>().localScale = normalCrossHairScale;
                targetedObject = null;
            }
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
            grabbedObject.transform.Rotate(90, 0, 0);
        }
    }

    public void Input_Grab(InputAction.CallbackContext context)
    {
        if(context.action.name != "Grab")
            return;

        if (context.started && !isDragging)
        {
            if (targetedObject)
            {
                if (targetedObject.CompareTag("Defi"))
                {
                    defiPaddles.PlayOneShot(defiPickUpSound);
                }
                else
                {
                    targetedObject.GetComponent<AudioSource>().PlayOneShot(itemPickUpSound);
                }

                grabbedObject = targetedObject;
                isDragging = true;
                if (grabbedObject.GetComponent<ForceObjectLogic>())
                    dragObjectForceObjectLogic = grabbedObject.GetComponent<ForceObjectLogic>();
            }
        }
        else if (context.canceled)
        {
            isDragging = false;
            if(grabbedObject)
            {
                if(grabbedObject.name == defibrilator.name)
                {
                    grabbedObject.transform.position = defiPos.position;
                    grabbedObject.transform.rotation = defiPos.rotation;
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
