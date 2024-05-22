using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientLifespan : MonoBehaviour
{
    [SerializeField] private int patientHealth = 100;
    [SerializeField] private int healthSeverity = 2;
    [SerializeField] private int patientHealthDecreaseRate = 1;
    public float deathTime = 12f;

    private bool isCurrentlyDying = false;
    private float deathTimer = 0;
    
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
        deathTimer = deathTime;
        while (true)
        {
            if (patientHealth > 0)
            {
                patientHealth -= healthSeverity;
                
                // ReSharper disable once PossibleLossOfFraction
                yield return new WaitForSeconds(1 / patientHealthDecreaseRate);
            }
            else
            {
                deathTimer -= Time.deltaTime;
                
                if (deathTimer <= 0)
                {
                    isCurrentlyDying = true;
                }
                
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void ResetDeathTime()
    {
        deathTimer = deathTime;
        isCurrentlyDying = false;
    }
    public float GetDeathTimer()
    {
        return deathTimer;
    }

    public void SetDying()
    {
        isCurrentlyDying = true;
    }
    
    public void IncreasePatientHealth(int healthIncrease)
    {
        patientHealth += healthIncrease;
        
        if(patientHealth > 100)
            patientHealth = 100;
    }

    public bool IsCurrentlyDying()
    {
        return isCurrentlyDying;
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
