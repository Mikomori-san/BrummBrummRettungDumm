using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DeliverObject : MonoBehaviour
{
    [SerializeField] private string patientTag;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(patientTag)) return;
        if(other.gameObject.GetComponentInParent<PatientLifespan>().IsCurrentlyDying()) return;
            
        PatientManager.Instance.RemovePatient(other.GetComponentInParent<RandomNameGiver>().gameObject);
        ScoreSystem.Instance.AddScoreDelivery((float)other.GetComponentInParent<PatientLifespan>().GetPatientHealth() / 100);
    }
}
