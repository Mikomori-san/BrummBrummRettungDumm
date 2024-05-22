using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    private static HealthBarManager instance = null;
    public static HealthBarManager Instance { get { return instance; } }

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

    [SerializeField] private GameObject patientUiPrefab;
    [SerializeField] private float healthBarHeight = 40f;
    [SerializeField] private float yOffset = 15f;
    
    [HideInInspector] public List<GameObject> allHealthbars = new List<GameObject>();
    // private float deathTimer;
    // private bool timerReset = true;


    private void Awake()
    {
        InitializeSingleton();
    }

    private void Update()
    {

    }

    public GameObject HealthBarNumberPlus(PatientManager.Patient patient)
    {
        GameObject patientHealthbar = Instantiate(patientUiPrefab, transform);
        allHealthbars.Add(patientHealthbar);
        StartCoroutine(UpdateHealthbar(patientHealthbar, patient));
        UpdateHealthBarPositions();
        return patientHealthbar;
    }
    
    public void UpdateHealthBarPositions()
    {
        try
        {
            for (int i = 0; i < allHealthbars.Count; i++)
            {
                allHealthbars[i].transform.position = new Vector3(90, i * healthBarHeight + yOffset, 0);
            }
        }
        catch (Exception e)
        {
            print("Healthbar is missing, restarting healthbar rearrangement.");
        }
    }

    private IEnumerator UpdateHealthbar(GameObject patientHealthbar, PatientManager.Patient patient)
    {
        PatientLifespan patientLifespan = patient.ragdoll.GetComponent<PatientLifespan>();
        Slider healthSlider = patientHealthbar.GetComponentInChildren<Slider>();
        TextMeshProUGUI name = patientHealthbar.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI reviveTimer = patientHealthbar.GetComponentsInChildren<TextMeshProUGUI>(true)[1];
        name.SetText(patient.ragdoll.name);
        
        while(true)
        {
            healthSlider.value = patientLifespan.GetPatientHealth();
            
            if (patientLifespan.GetPatientHealth() <= 0)
            {
                reviveTimer.gameObject.SetActive(true);
                reviveTimer.SetText("Revive! " + (int)patientLifespan.GetDeathTimer());
                patientLifespan.SetDying();
                
                if (patientLifespan.GetDeathTimer() <= 0 && patientLifespan.GetPatientHealth() <= 0)
                {
                    PatientManager.Instance.RemovePatient(patient);
                    ScoreSystem.Instance.AddScorePatientDeath();
                    break;
                }
            }
            
            if(patientLifespan.GetPatientHealth() > 0 && patientLifespan.IsCurrentlyDying())
            {
                reviveTimer.gameObject.SetActive(false);
                
                patientLifespan.deathTime -= 3;
                patientLifespan.ResetDeathTime();
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
}
