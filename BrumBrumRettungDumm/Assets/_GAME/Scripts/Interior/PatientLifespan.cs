using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientLifespan : MonoBehaviour
{
    [SerializeField] private int patientHealth = 100;
    [SerializeField] private int healthSeverity = 2;
    [SerializeField] private int patientHealthDecreaseRate = 1;
    
    public static PatientLifespan Instance { get; private set; }
    
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
        while (patientHealth > 0)
        {
            patientHealth -= healthSeverity;
            print("Patient Health: " + patientHealth);
            
            // ReSharper disable once PossibleLossOfFraction
            yield return new WaitForSeconds(1.0f * (1 / patientHealthDecreaseRate));
        }
    }
    
    public void IncreasePatientHealth(int healthIncrease)
    {
        if(patientHealth > 0)
            patientHealth += healthIncrease;
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
