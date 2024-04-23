using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceController : MonoBehaviour
{
    public static ForceController Instance { get; private set; }

    private List<GameObject> forceObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {

    }

    public void AddForceObject(GameObject forceObject)
    {
        forceObjects.Add(forceObject);
    }

    public void RemoveForceObject(GameObject forceObject)
    {
        forceObjects.Remove(forceObject);
    }
}
