using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteriorOrientation : MonoBehaviour
{
    GameObject target;
    [SerializeField] private string ambulanceBodyTag = "AmbulanceBody";

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag(ambulanceBodyTag);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
            transform.rotation = target.transform.rotation; 
    }
}
