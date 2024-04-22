using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateToTarget : MonoBehaviour
{

    [Header("Targeting")]
    public bool stopAtTarget = false;
    public bool useMousePosAsTarget = false;
    public Transform target;
    public float rotationSpeed = 25;

    [Header("Movement")]
    public bool moveToTarget = true;
    public float moveSpeed = 10;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        Vector2 moveDirection;

        if (useMousePosAsTarget)
        {
            Vector3 curMousePos = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -mainCamera.transform.position.z));
            moveDirection = curMousePos - this.transform.position;

            if (stopAtTarget && Vector3.Distance(curMousePos, this.transform.position) <= 0.3f) { return; }
        }
        else
        {
            if (target == null) { Debug.LogError("The target of \"" + this + "\" has not been assigned!", this); return; }
            moveDirection = target.position - this.transform.position;

            if (stopAtTarget && Vector3.Distance(target.transform.position, this.transform.position) <= 0.3f) { return; }
        }

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (moveToTarget)
        {
            transform.position += this.transform.right * moveSpeed * Time.deltaTime;
        }
    }
}
