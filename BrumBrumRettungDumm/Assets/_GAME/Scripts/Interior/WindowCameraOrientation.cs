using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WindowCameraOrientation : MonoBehaviour
{
    private Transform paramedicTransform;
    
    [HideInInspector] public float distanceFromPlayer;
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        paramedicTransform = InputSafe.instance.GetParamedic().GetComponent<Transform>();
        distanceFromPlayer = Vector3.Distance(paramedicTransform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + (transform.position - paramedicTransform.position));
        distanceFromPlayer = Vector3.Distance(paramedicTransform.position, transform.position);
    }
}
