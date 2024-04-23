using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IneriorSceneLoader : MonoBehaviour
{
    public Vector3 sceneOffset;

    void Start()
    {
        if(SceneManager.GetSceneByName("InteriorScene").isLoaded == false)
            SceneManager.LoadScene("InteriorScene", LoadSceneMode.Additive);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InteriorScene")
        {
            // Move all objects in the scene by the offset
            foreach (GameObject rootObject in scene.GetRootGameObjects())
            {
                rootObject.transform.position += sceneOffset;
            }
            GameObject ambulanceBody = GameObject.FindGameObjectWithTag("AmbulanceBody");

            if (ambulanceBody == null)
            {
                Debug.LogError("Ambulance body not found");
                return;
            }
            else
            {
                List<GameObject> rbGameObjects = GetAllGameObjectsWithRigidbodyInScene(SceneManager.GetSceneByName("InteriorScene"));
                foreach (GameObject go in rbGameObjects)
                {
                    ApplyInversedForces applyInversedComp = go.AddComponent<ApplyInversedForces>();
                    applyInversedComp.target = ambulanceBody;
                }
            }
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Update()
    {
    }
    private static List<GameObject> GetAllGameObjectsWithRigidbodyInScene(Scene scene)
    {
        List<GameObject> allObjectsWithRigidbody = new List<GameObject>();

        // Get all root GameObjects in the scene
        var rootObjects = scene.GetRootGameObjects();

        foreach (var rootObject in rootObjects)
        {
            // Check if the root GameObject has a Rigidbody
            if (rootObject.GetComponent<Rigidbody>() != null)
            {
                allObjectsWithRigidbody.Add(rootObject);
            }

            // Check all child GameObjects
            var childObjects = rootObject.GetComponentsInChildren<Transform>(true);
            foreach (var child in childObjects)
            {
                if (child.gameObject.GetComponent<Rigidbody>() != null)
                {
                    if(!allObjectsWithRigidbody.Contains(child.gameObject))
                        allObjectsWithRigidbody.Add(child.gameObject);
                }
            }
        }

        return allObjectsWithRigidbody;
    }
}
