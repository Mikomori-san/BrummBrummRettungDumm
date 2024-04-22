using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAcceleration : MonoBehaviour
{
    private Vector3 acceleration;
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        lastVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = (player.transform.position - 2 * velocity + lastVelocity) / Time.deltaTime;
        lastVelocity = velocity;
        velocity = player.transform.position;
        Debug.Log("Acceleration: " + acceleration);
    }
}
