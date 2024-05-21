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
    [SerializeField] private string ambulanceBodyTag = "AmbulanceBody";
    private GameObject ambulance;
    [SerializeField] private float distanceFromAmbulance = 1;
    [SerializeField] private float distanceBetweenArrows = 0.5f;
    [SerializeField] private float markerDeleteDistance = 5;
    
    private Queue<Material> unassignedColors = new Queue<Material>();
    private List<GameObject> markers = new List<GameObject>();
    private List<GameObject> arrows = new List<GameObject>();

    private void Awake()
    {
        InitializeSingleton();
        for (int i = 0; i < colors.Length; i++)
        {
            unassignedColors.Enqueue(colors[i]);
        }
    }

    private void Start()
    {
        ambulance = GameObject.FindGameObjectWithTag(ambulanceBodyTag);
    }

    void Update()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            arrows[i].transform.position = ambulance.transform.position + Vector3.up * distanceFromAmbulance + Vector3.up * distanceBetweenArrows * i;
            arrows[i].transform.LookAt(markers[i].transform);
            print(markers[i].transform.GetInstanceID());
            Debug.DrawLine(arrows[i].transform.position, markers[i].transform.position, Color.magenta);

            if (Vector3.Distance(markers[i].transform.position, ambulance.transform.position) <= markerDeleteDistance)
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
        newArrow.transform.position = ambulance.transform.position + Vector3.up * distanceFromAmbulance + Vector3.up * distanceBetweenArrows * markers.Count;
        newArrow.GetComponentInChildren<Renderer>().material = color;

        markers.Add(marker);
        arrows.Add(newArrow);
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
