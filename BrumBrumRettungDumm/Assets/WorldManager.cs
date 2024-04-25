using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Transform ambulance;
    [SerializeField] private Transform[] patientSpawnPositions;
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
        public Transform transform;
        public float timeUntilDeath;

        public Patient(Transform position, float timeUntilDeath)
        {
            this.transform = position;
            this.timeUntilDeath = timeUntilDeath;
        }
    }



    void Start()
    {
        for (int i = 0; i < startingPatients; i++)
        {
            patients.Add(new Patient(patientSpawnPositions[Random.Range(0, patientSpawnPositions.Length)], Random.Range(minTime_PatientDeath, maxTime_PatientDeath)));
        }

        timeUntilNewPatient = Random.Range(minTime_NewPatient, maxTime_NewPatient);
    }

    void Update()
    {
        for (int i = 0; i < patients.Count; i++)
        {
            patients[i].timeUntilDeath -= Time.deltaTime;

            if (Vector3.Distance(ambulance.transform.position, patients[i].transform.position) < patientCollectionRange)
            {
                //CALL PATIENTMANAGER.PATIENTCOLLECTED HERE ---------------------------------------------------------------------------------------------
                print($"Patient {patients[i].transform.GetInstanceID()} collected");
                patients.RemoveAt(i);
                i--;
                continue;
            }
            
            if (patients[i].timeUntilDeath < 0)
            {
                print($"Patient {patients[i].transform.GetInstanceID()} died");
                patients.RemoveAt(i);
                i--;
            }
        }

        timeUntilNewPatient -= Time.deltaTime;
        if(timeUntilNewPatient <= 0)
        {
            timeUntilNewPatient = Random.Range(minTime_NewPatient, maxTime_NewPatient);
            Patient patient = new Patient(patientSpawnPositions[Random.Range(0, patientSpawnPositions.Length)], Random.Range(minTime_PatientDeath, maxTime_PatientDeath));
            patients.Add(patient);
            print($"Patient {patient.transform.GetInstanceID()} created");
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
            Gizmos.DrawWireSphere(patients[i].transform.position, patientCollectionRange);
        }
    }
}
