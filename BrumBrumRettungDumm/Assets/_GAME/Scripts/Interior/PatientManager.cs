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

    [SerializeField] private GameObject[] patientPrefabs;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject interior;
    public List<Patient> allPatients = new List<Patient>();

    public class Patient
    {
        public GameObject ragdoll;
        public GameObject lifeBar;
    }

    void Awake()
    {
        InitializeSingleton();
    }

    public void SpawnPatient(uint modelId)
    {
        print("Spawn Patient in manager");
        Patient newPatient = new Patient();
        newPatient.ragdoll = Instantiate(patientPrefabs[modelId], spawnPoint.transform.position, patientPrefabs[modelId].transform.rotation, interior.transform);
        newPatient.lifeBar = HealthBarManager.Instance.HealthBarNumberPlus(newPatient);
        allPatients.Add(newPatient);
    }
    
    public void RemovePatient(Patient patient)
    {
        HealthBarManager.Instance.allHealthbars.Remove(patient.lifeBar);
        Destroy(patient.ragdoll);
        Destroy(patient.lifeBar);
        HealthBarManager.Instance.UpdateHealthBarPositions();
    }

    public void RemovePatient(GameObject patientObject)
    {
        for (int i = 0; i < allPatients.Count; i++)
        {
            if (allPatients[i].ragdoll == patientObject)
            {
                RemovePatient(allPatients[i]);
                return;
            }
        }
    }

    public void Input_SpawnPatient(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SpawnPatient(0);
        }
    }
}
