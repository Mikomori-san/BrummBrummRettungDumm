using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSelf : MonoBehaviour
{
    private Color startcolor;
    [SerializeField] private Camera cam;

    void Start()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        startcolor = objectRenderer.material.color;
    }

    void OnMouseOver()
    {
        float distance = Vector3.Distance(cam.transform.position, transform.position);

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