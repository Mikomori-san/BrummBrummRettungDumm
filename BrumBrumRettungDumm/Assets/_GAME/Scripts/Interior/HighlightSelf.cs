using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSelf : MonoBehaviour
{
    private Color startcolor;
    private Camera paramedicCamera;

    void Start()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        startcolor = objectRenderer.material.color;
        paramedicCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
    }

    void OnMouseOver()
    {
        float distance = Vector3.Distance(paramedicCamera.transform.position, transform.position);

        if (distance <= 1.5f)
        {
            Renderer objectRenderer = GetComponent<Renderer>();
            objectRenderer.material.color = Color.yellow;
        }
        else
        {
            Renderer objectRenderer = GetComponent<Renderer>();
            objectRenderer.material.color = startcolor;
        }
    }

    void OnMouseExit()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.color = startcolor;
    }

    // Update is called once per frame
    void Update()
    {

    }
}