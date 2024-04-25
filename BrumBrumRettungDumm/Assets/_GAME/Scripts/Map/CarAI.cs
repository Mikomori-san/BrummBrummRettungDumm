using Barmetler.RoadSystem;
using DavidJalbert.TinyCarControllerAdvance;
using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class CarAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float maxSteerAngle = 50;
    [SerializeField] private WheelCollider wheel_F_L;
    [SerializeField] private WheelCollider wheel_F_R;
    [SerializeField] private WheelCollider wheel_R_L;
    [SerializeField] private WheelCollider wheel_R_R;
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float brakeSpeed = 20;
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float waypointDetectionRadius = 0.3f;
    [SerializeField] private float antiRoll = 5000f;
    [SerializeField] private Renderer carBody;
    [SerializeField] private Texture2D textureBraking;

    [Header("Sensors")]
    [SerializeField] private float sensorLength = 3;
    [SerializeField] private Vector3 mainSensorOffset;
    [SerializeField] private float sideSensorsXOffset = 0.5f;
    [SerializeField] private float sideSensorsAngle = 30;
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private string playerTag;


    private Rigidbody rb;
    private RoadSystemNavigator navi;
    private int curWayPoint = 0;
    private float curspeed;
    private bool isBraking;
    private bool isAvoiding;
    private Texture mainTexture;
    private Vector3 nextTargetPoint;
    void Start()
    {
        navi = this.GetComponent<RoadSystemNavigator>();
        navi.Goal = target.position;
        rb = this.GetComponent<Rigidbody>();
        mainTexture = carBody.material.mainTexture;
    }


    private void FixedUpdate()
    {
        navi.Goal = target.position;
        //BalanceVehicle(); <- Currently makes things worse
        Sensors();
        SteerTowardsPath();
        Drive();
        CheckWaypoints();
        Braking();
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 rightSensorOffset = mainSensorOffset;
        rightSensorOffset += this.transform.right * sideSensorsXOffset;
        Vector3 leftSensorOffset = mainSensorOffset;
        leftSensorOffset -= this.transform.right * sideSensorsXOffset;

        Vector3 basePos = this.transform.position + this.transform.forward + mainSensorOffset;
        isBraking = false;
        isAvoiding = false;
        float avoidanceMultipier = 0;
        int pressure = 0;

        






        //Sensor Right
        if (Physics.Raycast(this.transform.position + this.transform.forward + rightSensorOffset, this.transform.forward, out hit, sensorLength, objectLayer))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                isBraking = true;
                Debug.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, hit.point, Color.green);
            }

            pressure++;
            avoidanceMultipier -= 1;
        }
        else
        {
            Debug.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, this.transform.position + this.transform.forward + rightSensorOffset + transform.forward * sensorLength);
        }

        //Sensor Right Angled
        if (Physics.Raycast(this.transform.position + this.transform.forward + rightSensorOffset, Quaternion.AngleAxis(sideSensorsAngle, this.transform.up) * transform.forward, out hit, sensorLength, objectLayer))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                isBraking = true;
                Debug.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, hit.point, Color.green);
            }

            pressure++;
            avoidanceMultipier -= 0.5f;
        }
        else
        {
            Debug.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, (this.transform.position + this.transform.forward + rightSensorOffset) + Quaternion.AngleAxis(sideSensorsAngle, this.transform.up) * transform.forward * sensorLength);
        }

        //Sensor Left
        if (Physics.Raycast(this.transform.position + this.transform.forward + leftSensorOffset, this.transform.forward, out hit, sensorLength, objectLayer))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                isBraking = true;
                Debug.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, hit.point, Color.green);
            }

            pressure++;
            avoidanceMultipier += 1;
        }
        else
        {
            Debug.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, this.transform.position + this.transform.forward + leftSensorOffset + transform.forward * sensorLength);
        }

        //Sensor Left Angled
        if (Physics.Raycast(this.transform.position + this.transform.forward + leftSensorOffset, Quaternion.AngleAxis(-sideSensorsAngle, this.transform.up) * transform.forward, out hit, sensorLength, objectLayer))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                isBraking = true;
                Debug.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, hit.point, Color.green);
            }

            pressure++;
            avoidanceMultipier += 0.5f;
        }
        else
        {
            Debug.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, (this.transform.position + this.transform.forward + leftSensorOffset) + Quaternion.AngleAxis(-sideSensorsAngle, this.transform.up) * transform.forward * sensorLength);
        }

        //Main Sensor
        if (Physics.Raycast(basePos, this.transform.forward, out hit, sensorLength, objectLayer))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                isBraking = true;
            }

            pressure++;

            if (avoidanceMultipier == 0)
            {
                isBraking = true;
            }
            if (avoidanceMultipier > 0)
            {
                avoidanceMultipier -= 0.5f;
            }
            if (avoidanceMultipier < 0)
            {
                avoidanceMultipier += 0.5f;
            }

            Debug.DrawLine(this.transform.position + this.transform.forward + mainSensorOffset, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(this.transform.position + this.transform.forward + mainSensorOffset, this.transform.position + this.transform.forward + mainSensorOffset + transform.forward * sensorLength);
        }

        if(pressure >= 4)
        {
            isBraking = true;
        }

        if (pressure > 0)
        {
            isAvoiding = true;
            wheel_F_L.steerAngle = maxSteerAngle * avoidanceMultipier;
            wheel_F_R.steerAngle = maxSteerAngle * avoidanceMultipier;
        }
    }

    private void Braking()
    {
        if(isBraking)
        {
            if(textureBraking != null)
            {
                carBody.material.mainTexture = textureBraking;
            }
            wheel_R_L.brakeTorque = brakeSpeed;
            wheel_R_R.brakeTorque = brakeSpeed;
        }
        else
        {
            carBody.material.mainTexture = mainTexture;
            wheel_R_L.brakeTorque = 0;
            wheel_R_R.brakeTorque = 0;
        }
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

        var antiRollForce = (travelL - travelR) * antiRoll;

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
        if (Vector3.Distance(transform.position, nextTargetPoint) < waypointDetectionRadius) 
        {
            curWayPoint++;
            print(curWayPoint);
        }
    }

    private void Drive()
    {
        curspeed = 2 * MathF.PI * wheel_F_L.radius * wheel_F_L.rpm * 60 / 1000f;
        if(curspeed < maxSpeed && !isBraking) 
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
        if(navi.CurrentPoints.Count == 0 || isAvoiding) { return; }
        
        if(navi.CurrentPoints.Count > 1) 
        {
            nextTargetPoint = navi.CurrentPoints[0].position - navi.CurrentPoints[1].position;
            nextTargetPoint = Quaternion.AngleAxis(-90, this.transform.up) * nextTargetPoint;
            nextTargetPoint += navi.CurrentPoints[0].position;
        }
        else
        {
            nextTargetPoint = navi.CurrentPoints[0].position;
        }

        Vector3 relativeVector = transform.InverseTransformPoint(nextTargetPoint);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheel_F_L.steerAngle = newSteer;
        wheel_F_R.steerAngle = newSteer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, waypointDetectionRadius);

        Gizmos.DrawLine(this.transform.position + this.transform.forward + mainSensorOffset , this.transform.position + this.transform.forward + mainSensorOffset + transform.forward * sensorLength);

        Gizmos.DrawSphere(nextTargetPoint, 0.6f);
        //Vector3 rightSensorOffset = mainSensorOffset;
        //rightSensorOffset += this.transform.right * sideSensorsXOffset;
        //Gizmos.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, this.transform.position + this.transform.forward + rightSensorOffset + transform.forward * sensorLength);
        //Gizmos.DrawLine(this.transform.position + this.transform.forward + rightSensorOffset, (this.transform.position + this.transform.forward + rightSensorOffset) + Quaternion.AngleAxis(sideSensorsAngle, this.transform.up) * transform.forward * sensorLength);
        
        //Vector3 leftSensorOffset = mainSensorOffset;
        //leftSensorOffset -= this.transform.right * sideSensorsXOffset;
        //Gizmos.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, this.transform.position + this.transform.forward + leftSensorOffset + transform.forward * sensorLength);
        //Gizmos.DrawLine(this.transform.position + this.transform.forward + leftSensorOffset, (this.transform.position + this.transform.forward + leftSensorOffset) + Quaternion.AngleAxis(-sideSensorsAngle, this.transform.up) * transform.forward * sensorLength);
    }
}
