using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ApplyInversedForces : MonoBehaviour
{
    public GameObject target;
    private Vector3 force;
    private Vector3 angularTorque;
    private Vector3 gravity;
    private Vector3 lastVelocity;
    private Vector3 lastAngularVelocity;
    private Rigidbody rb;
    private Rigidbody targetRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        targetRigidbody = target.GetComponent<Rigidbody>();
        force = Vector3.zero;
        lastVelocity = rb.velocity;
        lastAngularVelocity = rb.angularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaVelocity = targetRigidbody.velocity - lastVelocity;
        Vector3 deltaAngularVelocity = targetRigidbody.angularVelocity - lastAngularVelocity;

        //determine in which direction the gravity affects the object from the object's perspective
        gravity = target.transform.InverseTransformDirection(Physics.gravity);

        Vector3 acceleration = deltaVelocity / Time.fixedDeltaTime;
        Vector3 angularAcceleration = deltaAngularVelocity / Time.fixedDeltaTime;

        Vector3 totalForce = rb.mass * acceleration;
        Vector3 totalTorque = Vector3.Scale(rb.inertiaTensor, angularAcceleration);

        force = totalForce;
        angularTorque = totalTorque;

        rb.AddForce(-totalForce);
        rb.AddForce(gravity, ForceMode.Acceleration);   //the gravity applied may be a bit to strong
        //rb.AddTorque(-totalTorque);   //this works but i think the torque doesnt need to be applied

        lastVelocity = targetRigidbody.velocity;
        lastAngularVelocity = targetRigidbody.angularVelocity;
    }
}
