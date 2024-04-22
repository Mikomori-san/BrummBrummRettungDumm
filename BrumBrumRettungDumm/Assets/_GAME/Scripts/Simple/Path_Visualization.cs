using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Visualization : MonoBehaviour
{
    [HideInInspector] public DrawPath parent;
    [HideInInspector] public Color startColor;
    [HideInInspector] public Color endColor;
    [HideInInspector] public float lifeTime;
    private float curLifeTime;
    private Renderer sphereRenderer;

    private void Update()
    {
        if(this.sphereRenderer == null) { this.sphereRenderer = this.GetComponent<Renderer>(); }

        this.curLifeTime -= Time.deltaTime;

        this.sphereRenderer.material.color = Color.Lerp(this.endColor, this.startColor, this.curLifeTime);       

        if (this.curLifeTime <= 0)
        {
            parent.pathPoints.Enqueue(this.gameObject);
            this.gameObject.SetActive(false);
            sphereRenderer.material.color = startColor;
            curLifeTime = lifeTime;
        }
    }  
}
