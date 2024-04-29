using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PatientManager : MonoBehaviour
{
    #region This is a Singleton
    private static PatientManager instance = null;
    public static PatientManager Instance { get { return instance; } }

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

    [SerializeField] private GameObject patientPrefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject interior;

    public class Patient
    {
        public GameObject ragdoll;
        public GameObject lifeBar;
    }

    void Awake()
    {
        InitializeSingleton();
    }

    public Patient SpawnPatient()
    {
        print("Spawn Patient in manager");
        Patient patient = new Patient();
        patient.ragdoll = Instantiate(patientPrefab, spawnPoint.transform.position, Quaternion.identity, interior.transform);
        patient.lifeBar = HealthBarManager.Instance.HealthBarNumberPlus(patient);
        return patient;
    }
    
    public void RemovePatient(Patient patient)
    {
        HealthBarManager.Instance.allHealthbars.Remove(patient.lifeBar);
        Destroy(patient.ragdoll);
        Destroy(patient.lifeBar);
        HealthBarManager.Instance.UpdateHealthBarPositions();
    }
    
    public void Input_SpawnPatient(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnPatient();
        }
    }
}
