using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.InputSystem;


public class ForceController : MonoBehaviour
{
    public static ForceController Instance { get; private set; }

    [SerializeField] private GameObject target;
    private Rigidbody targetRigidbody;
    private Vector3 gravity;
    private Vector3 lastVelocity;
    private Vector3 lastAngularVelocity;
    private List<ForceObjectLogic> forceObjects = new List<ForceObjectLogic>();

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

    void Start()
    {
        foreach (ForceObjectLogic forceObject in forceObjects)
        {
            forceObject.GetComponent<Rigidbody>().useGravity = true;
        }
        if(target != null)
            SetTarget(target);
    }

    void FixedUpdate()
    {
        if (target == null)
            return;
        if (targetRigidbody == null)
            targetRigidbody = target.GetComponent<Rigidbody>();
        if (targetRigidbody == null) return;

        Vector3 deltaVelocity = targetRigidbody.velocity - lastVelocity;
        Vector3 deltaAngularVelocity = targetRigidbody.angularVelocity - lastAngularVelocity;

        //determine in which direction the gravity affects the object from the object's perspective
        gravity = target.transform.InverseTransformDirection(Physics.gravity);

        Vector3 acceleration = deltaVelocity / Time.fixedDeltaTime;
        Vector3 angularAcceleration = deltaAngularVelocity / Time.fixedDeltaTime;

        foreach (ForceObjectLogic forceObject in forceObjects)
        {
            Rigidbody rb = forceObject.GetComponent<Rigidbody>();
            Vector3 totalForce = rb.mass * acceleration * forceObject.forceMultiplier;
            Vector3 totalTorque = Vector3.Scale(rb.inertiaTensor, angularAcceleration) * forceObject.torqueMultiplier;

            rb.AddForce(totalForce);
            rb.AddForce(gravity * forceObject.gravityMultiplier, ForceMode.Acceleration);   //the gravity applied may be a bit to strong
            rb.AddTorque(totalTorque);   //this works but i think the torque doesnt need to be applied
        }

        lastVelocity = targetRigidbody.velocity;
        lastAngularVelocity = targetRigidbody.angularVelocity;
    }

    public void AddForceObject(ForceObjectLogic forceObject)
    {
        if(!forceObjects.Contains(forceObject))
            forceObjects.Add(forceObject);
    }

    public void RemoveForceObject(ForceObjectLogic forceObject)
    {
        forceObjects.Remove(forceObject);
    }
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
