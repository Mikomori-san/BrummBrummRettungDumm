using Barmetler.RoadSystem;
using DavidJalbert.TinyCarControllerAdvance;
using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class CarAI : MonoBehaviour
{
    private Rigidbody rb;
    private RoadSystemNavigator navi;
    private int curWayPoint = 0;
    [SerializeField] private Transform target;
    [SerializeField] private float maxSteerAngle = 50;
    [SerializeField] private WheelCollider wheel_F_L;
    [SerializeField] private WheelCollider wheel_F_R;
    [SerializeField] private float acceleration = 10;
    private float curspeed;
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float waypointDetectionRadius = 0.3f;
    [SerializeField] private float AntiRoll = 5000f;

    void Start()
    {
        navi = this.GetComponent<RoadSystemNavigator>();
        navi.Goal = target.position;
        rb = this.GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        navi.Goal = target.position;
        //BalanceVehicle(); <- Currently makes things worse
        SteerTowardsPath();
        Drive();
        CheckWaypoints();
    }

    private void BalanceVehicle()
    {
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;

        var groundedL = wheel_F_L.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-wheel_F_L.transform.InverseTransformPoint(hit.point).y - wheel_F_L.radius) / wheel_F_L.suspensionDistance;

        var groundedR = wheel_F_R.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-wheel_F_R.transform.InverseTransformPoint(hit.point).y - wheel_F_R.radius) / wheel_F_R.suspensionDistance;

        var antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            rb.AddForceAtPosition(wheel_F_L.transform.up * -antiRollForce,
                   wheel_F_L.transform.position);
        if (groundedR)
            rb.AddForceAtPosition(wheel_F_R.transform.up * antiRollForce,
                   wheel_F_R.transform.position);
    }

    private void CheckWaypoints()
    {
        if (navi.CurrentPoints.Count == 0) { return; }
        if (Vector3.Distance(transform.position, navi.CurrentPoints[0].position) < waypointDetectionRadius) 
        {
            curWayPoint++;
            print(curWayPoint);
        }
    }

    private void Drive()
    {
        curspeed = 2 * MathF.PI * wheel_F_L.radius * wheel_F_L.rpm * 60 / 1000f;
        if(curspeed < maxSpeed) 
        { 
            wheel_F_L.motorTorque = acceleration;
            wheel_F_R.motorTorque = acceleration;
        }
        else
        {
            wheel_F_L.motorTorque = 0;
            wheel_F_R.motorTorque = 0;
        }
    }

    private void SteerTowardsPath()
    {
        if(navi.CurrentPoints.Count == 0) { return; }
        Vector3 relativeVector = transform.InverseTransformPoint(navi.CurrentPoints[0].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheel_F_L.steerAngle = newSteer;
        wheel_F_R.steerAngle = newSteer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, waypointDetectionRadius);
    }
}
