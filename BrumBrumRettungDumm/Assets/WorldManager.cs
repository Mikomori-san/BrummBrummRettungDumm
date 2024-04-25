using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Transform ambulance;
    [Header("Patients")]
    [SerializeField] private GameObject[] patientModels;
    [SerializeField] private Transform[] patientSpawnPositions;
    [SerializeField] private float maxOffsetFromSpawnPosition;
    [SerializeField] private float minTime_PatientDeath = 15;
    [SerializeField] private float maxTime_PatientDeath = 20;
    [SerializeField] private float minTime_NewPatient = 10;
    [SerializeField] private float maxTime_NewPatient = 15;
    [SerializeField] private int startingPatients = 3;
    [SerializeField] private float patientCollectionRange = 3;
    private float timeUntilNewPatient;
    private List<Patient> patients = new List<Patient>();
    
    private class Patient
    {
        public int modelId;
        public float timeUntilDeath;
        public GameObject gameObject;

        public Patient(float timeUntilDeath, int modelId, GameObject gameObjectInstance)
        {
            this.timeUntilDeath = timeUntilDeath;
            this.modelId = modelId;
            this.gameObject = gameObjectInstance;
        }
    }



    void Start()
    {
        for (int i = 0; i < startingPatients; i++)
        {
            int modelIndex = Random.Range(0, patientModels.Length);
            GameObject patientObject = Instantiate(patientModels[modelIndex]);
            patientObject.transform.position = patientSpawnPositions[Random.Range(0, patientSpawnPositions.Length)].position +
                                                new Vector3(Random.Range(-maxOffsetFromSpawnPosition, maxOffsetFromSpawnPosition), 0, Random.Range(-maxOffsetFromSpawnPosition, maxOffsetFromSpawnPosition));

            patients.Add(new Patient(Random.Range(minTime_PatientDeath, maxTime_PatientDeath), modelIndex, patientObject));
        }

        timeUntilNewPatient = Random.Range(minTime_NewPatient, maxTime_NewPatient);
    }

    void Update()
    {
        for (int i = 0; i < patients.Count; i++)
        {
            patients[i].timeUntilDeath -= Time.deltaTime;

            if (Vector3.Distance(ambulance.transform.position, patients[i].gameObject.transform.position) < patientCollectionRange)
            {
                PatientManager.Instance.SpawnPatient();
                print($"Patient {patients[i].gameObject.transform.GetInstanceID()} collected");
                Destroy(patients[i].gameObject); 
                patients.RemoveAt(i);
                i--;
                continue;
            }
            
            if (patients[i].timeUntilDeath < 0)
            {
                print($"Patient {patients[i].gameObject.transform.GetInstanceID()} died");
                Destroy(patients[i].gameObject);
                patients.RemoveAt(i);
                i--;
            }
        }

        timeUntilNewPatient -= Time.deltaTime;
        if(timeUntilNewPatient <= 0)
        {
            timeUntilNewPatient = Random.Range(minTime_NewPatient, maxTime_NewPatient);
            
            int modelIndex = Random.Range(0, patientModels.Length);
            GameObject patientObject = Instantiate(patientModels[modelIndex]);
            patientObject.transform.position = patientSpawnPositions[Random.Range(0, patientSpawnPositions.Length)].position +
                                                new Vector3(Random.Range(-maxOffsetFromSpawnPosition, maxOffsetFromSpawnPosition), 0, Random.Range(-maxOffsetFromSpawnPosition, maxOffsetFromSpawnPosition));

            patients.Add(new Patient(Random.Range(minTime_PatientDeath, maxTime_PatientDeath), modelIndex, patientObject));
            print($"Patient {patientObject.transform.GetInstanceID()} created");
        }
    }

    private void OnDrawGizmos()
    {
        if(patientSpawnPositions == null) { return; }
        for (int i = 0; i < patientSpawnPositions.Length; i++)
        {
            if (patientSpawnPositions[i] == null) { continue; }
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(patientSpawnPositions[i].position, 1);
        }

        if(patients == null) { return; }
        for (int i = 0; i < patients.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(patients[i].gameObject.transform.position, patientCollectionRange);
        }
    }
}
