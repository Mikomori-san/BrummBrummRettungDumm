using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    private float multiplier = 1;
    private float score = 0;
    private const short PILL_SCORE = 100;
    private const short DEFIBRILATOR_SCORE = 150;
    private const short DELIVERY_SCORE = 500;
    private const short REMAINING_PATIENT_SCORE = 50;
    private const short PATIENT_DEATH_SCORE = -1000;
    
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI multiplierText;
    
    private static ScoreSystem instance;
    
    public static ScoreSystem Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        scoreText.text = score.ToString("F0");
        multiplierText.text = "x" + multiplier.ToString("F2");
    }

    public float GetScore()
    {
        return score;
    }
    
    public void AddScorePill()
    {
        score += PILL_SCORE * multiplier;
    }
    
    public void AddScoreDefibrilator()
    {
        score += DEFIBRILATOR_SCORE * multiplier;
    }
    
    public void AddScoreDelivery(float patientHealthPercent)
    {
        multiplier += patientHealthPercent;
        score += DELIVERY_SCORE * multiplier;
    }
    
    public void AddScoreRemainingPatient(int remainingPatientCount)
    {
        score += REMAINING_PATIENT_SCORE * remainingPatientCount * multiplier;
    }
    
    public void AddScorePatientDeath()
    {
        score += PATIENT_DEATH_SCORE;
        multiplier = 1;
    }
}
