using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DrawPath : MonoBehaviour
{
    
    [Header("Timer Settings")]
    public float spawnTimer = 0.03f;
    private float curSpawnTimer;
    public float lifeTime = 1;

    [Header("PathPoint Look")]
    public Mesh pathPoint;
    public Vector3 scale = new Vector3(0.3f, 0.3f, 0.3f);
    public Color startColor = Color.white;
    public Color endColor = Color.black;

    [HideInInspector] public Queue<GameObject> pathPoints = new Queue<GameObject>();

    private void OnValidate()
    {
        if(pathPoint == null)
        {
            pathPoint = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Models/Simple/LowPolySphere.fbx", typeof(Mesh));
        }
    }

    void Update()
    {
        DrawSpheres();
    }

    private void DrawSpheres()
    {
        this.curSpawnTimer -= Time.deltaTime;

        if (this.curSpawnTimer <= 0)
        {
            GameObject point;
            if (this.pathPoints.Count > 0)
            {
                point = this.pathPoints.Dequeue();
            }
            else
            {
                //Generate a new PathPoint
                point = new GameObject("Path_Point");

                MeshFilter filter = point.AddComponent<MeshFilter>();
                filter.mesh = pathPoint;

                point.AddComponent<MeshRenderer>();
                
                Path_Visualization visualization = point.AddComponent<Path_Visualization>();
                visualization.parent = this;
                visualization.startColor = this.startColor;
                visualization.endColor = this.endColor;
                visualization.lifeTime = this.lifeTime;

                point.transform.localScale = scale;
            }

            point.transform.position = this.transform.position;
            point.gameObject.SetActive(true);

            this.curSpawnTimer = this.spawnTimer;
        }
    }
}
