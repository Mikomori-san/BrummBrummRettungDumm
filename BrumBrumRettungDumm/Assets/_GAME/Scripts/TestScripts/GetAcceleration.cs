using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAcceleration : MonoBehaviour
{
    public Vector3 force;
    public Vector3 angularTorque;
    private Vector3 lastVelocity;
    private Vector3 lastAngularVelocity;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        force = Vector3.zero;
        lastVelocity = rb.velocity;
        lastAngularVelocity = rb.angularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaVelocity = rb.velocity - lastVelocity;
        Vector3 deltaAngularVelocity = rb.angularVelocity - lastAngularVelocity;

        Vector3 acceleration = deltaVelocity / Time.fixedDeltaTime;
        Vector3 angularAcceleration = deltaAngularVelocity / Time.fixedDeltaTime;

        Vector3 totalForce = rb.mass * acceleration;
        Vector3 totalTorque = Vector3.Scale(rb.inertiaTensor, angularAcceleration);

        force = totalForce;
        angularTorque = totalTorque;

        lastVelocity = rb.velocity;
        lastAngularVelocity = rb.angularVelocity;
    }
}
