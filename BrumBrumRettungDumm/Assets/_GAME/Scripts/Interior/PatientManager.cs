using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PatientManager : MonoBehaviour
{
    
    [SerializeField] private GameObject patientPrefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject canvasForHealthBarManager;
    
    private HealthBarManager healthBarManager;
    public static PatientManager Instance { get; private set; }
    
    void Awake()
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
    
    // Start is called before the first frame update
    void Start()
    {
        healthBarManager = canvasForHealthBarManager.GetComponent<HealthBarManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnPatient()
    {
        GameObject patient = Instantiate(patientPrefab, spawnPoint.transform.position, Quaternion.identity);
        healthBarManager.HealthBarNumberPlus(patient);   
        return patient;
    }
    
    public void RemovePatient(GameObject patient)
    {
        Destroy(patient);
    }
    
    public void Input_SpawnPatient(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnPatient();
        }
    }
}
