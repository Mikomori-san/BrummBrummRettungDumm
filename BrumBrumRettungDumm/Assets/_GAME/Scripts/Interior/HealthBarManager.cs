using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    #region This is a Singleton
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
    #endregion

    [SerializeField] private GameObject patientUiPrefab;
    [SerializeField] private float healthBarHeight = 40f;
    [SerializeField] private float yOffset = 15f;
    
    [HideInInspector] public List<GameObject> allHealthbars = new List<GameObject>();


    private void Awake()
    {
        InitializeSingleton();
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
            print("Healthbar is missing, restarting healthbar rearaangement.");
        }
    }

    private IEnumerator UpdateHealthbar(GameObject patientHealthbar, PatientManager.Patient patient)
    {
        float deathTimer = 1f;
        
        PatientLifespan patientLifespan = patient.ragdoll.GetComponent<PatientLifespan>();
        Slider healthSlider = patientHealthbar.GetComponentInChildren<Slider>();
        TextMeshProUGUI name = patientHealthbar.GetComponentInChildren<TextMeshProUGUI>();
        name.SetText(patient.ragdoll.name);
        
        while(true)
        {
            healthSlider.value = patientLifespan.GetPatientHealth();
            
            if (patientLifespan.GetPatientHealth() <= 0)
            {
                deathTimer -= Time.deltaTime;
                if (deathTimer <= 0)
                {
                    
                    PatientManager.Instance.RemovePatient(patient);
                    ScoreSystem.Instance.AddScorePatientDeath();
                    break;
                }
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
}
