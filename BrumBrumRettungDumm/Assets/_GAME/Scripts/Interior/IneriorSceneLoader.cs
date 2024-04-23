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
                GameObject.Find("ForceController").GetComponent<ForceController>().target = ambulanceBody;
                GameObject.FindGameObjectWithTag("Ambulance").GetComponent<InteriorOrientation>().target = ambulanceBody;
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
}
