using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceObjectLogic : MonoBehaviour
{
    public float forceMultiplier = 1f;
    public float torqueMultiplier = 0f;
    public float gravityMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ForceController.Instance.AddForceObject(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableForce()
    {
        ForceController.Instance.RemoveForceObject(this);
    }

    public void EnableForce()
    {
        ForceController.Instance.AddForceObject(this);
    }
}
