using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyWindowRotation : MonoBehaviour
{
    [SerializeField] private GameObject leftWindowSimulation;
    [SerializeField] private GameObject rightWindowSimulation;
    
    private GameObject leftWindow;
    private GameObject rightWindow;
    
    // Start is called before the first frame update
    void Start()
    {
        leftWindow = InputSafe.instance.GetAmbulance().GetComponentsInChildren<Camera>()[1].gameObject;
        rightWindow = InputSafe.instance.GetAmbulance().GetComponentsInChildren<Camera>()[2].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        leftWindow.transform.rotation = leftWindowSimulation.transform.rotation;
        rightWindow.transform.rotation = rightWindowSimulation.transform.rotation;

        float leftDistance = leftWindowSimulation.GetComponent<WindowCameraOrientation>().distanceFromPlayer;
        float rightDistance = rightWindowSimulation.GetComponent<WindowCameraOrientation>().distanceFromPlayer;

        float baseFOV = 10.0f;  // base field of view when distance is 1
        float factor = -0.5f;   // factor to adjust field of view based on distance

        // Calculate the field of view based on the distance
        float leftFOV = baseFOV + factor * (leftDistance - 1);
        float rightFOV = baseFOV + factor * (rightDistance - 1);

        // Set the field of view of each camera
        leftWindow.GetComponent<Camera>().fieldOfView = leftFOV;
        rightWindow.GetComponent<Camera>().fieldOfView = rightFOV;
    }
}
