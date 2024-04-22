using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceObjectLogic : MonoBehaviour
{
    [HideInInspector]
    public Vector3 lastPosition { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        ForceController.Instance.AddForceObject(gameObject);
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
