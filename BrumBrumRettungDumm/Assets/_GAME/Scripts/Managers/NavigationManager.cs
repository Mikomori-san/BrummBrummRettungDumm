using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    #region This is a Singleton
    private static NavigationManager instance = null;
    public static NavigationManager Instance { get { return instance; } }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private GameObject arrow;
    [SerializeField] private Material[] colors;
    [SerializeField] private Transform[] debugPositions;
    
    private Queue<Material> unassignedColors = new Queue<Material>();
    private List<GameObject> markers = new List<GameObject>();
    private List<GameObject> arrows = new List<GameObject>();
    private float markerDeleteDistance;

    private void Awake()
    {
        InitializeSingleton();
        for (int i = 0; i < colors.Length; i++)
        {
            unassignedColors.Enqueue(colors[i]);
        }
    }


    void Update()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            arrows[i].transform.LookAt(markers[i].transform);

            if (Vector2.Distance(new Vector2(markers[i].transform.position.x, markers[i].transform.position.z), new Vector2(markers[i].transform.position.x, markers[i].transform.position.z)) < markerDeleteDistance)
            {
                RemoveMarker(markers[i]);
                i--;
            }
        }
    }

    public void AddMarker(ref GameObject marker)
    {
        Material color = unassignedColors.Dequeue();
        marker.GetComponent<Renderer>().material = color;
        GameObject newArrow = Instantiate(arrow);
        arrow.GetComponent<Renderer>().material = color;

        markers.Add(marker);
        arrows.Add(arrow);
    }

    public void RemoveMarker(GameObject marker) 
    {
        for (int i = 0; i < markers.Count; i++)
        {
            if (markers[i] == marker)
            {
                unassignedColors.Enqueue(marker.GetComponent<Renderer>().material);

                Destroy(markers[i]);
                Destroy(arrows[i]);
                markers.RemoveAt(i);
                arrows.RemoveAt(i);
                return;
            }
        }

        Debug.LogWarning(marker + " doesn't exist!");
    }
}
