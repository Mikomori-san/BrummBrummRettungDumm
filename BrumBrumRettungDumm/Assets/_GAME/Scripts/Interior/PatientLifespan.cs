using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientLifespan : MonoBehaviour
{
    [SerializeField] private int patientHealth = 100;
    [SerializeField] private int healthSeverity = 2;
    [SerializeField] private int patientHealthDecreaseRate = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DecreasePatientHealth());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DecreasePatientHealth()
    {
        while (true)
        {
            if (patientHealth > 0)
            {
                patientHealth -= healthSeverity;
        
                // ReSharper disable once PossibleLossOfFraction
                yield return new WaitForSeconds(1.0f * (1 / patientHealthDecreaseRate));
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
    
    public void IncreasePatientHealth(int healthIncrease)
    {
        patientHealth += healthIncrease;
        
        if(patientHealth > 100)
            patientHealth = 100;
    }
    
    public void IncreaseSeverity(int severityIncrease)
    {
        healthSeverity += severityIncrease;
    }

    public void IncreaseHealthDecreaseRate(int healthDecreaseRateIncrease)
    {
        patientHealthDecreaseRate += healthDecreaseRateIncrease;
    }
    
    public int GetPatientHealth()
    {
        return patientHealth;
    }
}
