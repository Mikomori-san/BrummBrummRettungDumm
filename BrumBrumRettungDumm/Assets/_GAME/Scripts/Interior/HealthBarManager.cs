using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private GameObject patientUIprefab;
    [SerializeField] private float healthBarHeight = 40f;
    
    private List<GameObject> patientHealthbars = new List<GameObject>();

    
    
    private float offset = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealthBarNumberPlus(GameObject patient)
    {
        GameObject patientHealthbar = Instantiate(patientUIprefab, transform);
        patientHealthbars.Add(patientHealthbar);
        StartCoroutine(UpdateHealthbar(patientHealthbar, patient));
        UpdateHealthBarPositions();
    }
    
    private void UpdateHealthBarPositions()
    {
        try
        {
            for (int i = 0; i < patientHealthbars.Count; i++)
            {
                patientHealthbars[i].transform.position = new Vector3(90, i * healthBarHeight + offset, 0);
            }
        }
        catch (Exception e)
        {
            print("Healthbar is missing, restarting healthbar rearaangement.");
        }
    }

    private IEnumerator UpdateHealthbar(GameObject patientHealthbar, GameObject patient)
    {
        float deathTimeAccumulator = 1f;
        
        PatientLifespan patientLifespan = patient.GetComponent<PatientLifespan>();
        Slider healthSlider = patientHealthbar.GetComponentInChildren<Slider>();
        TextMeshProUGUI name = patientHealthbar.GetComponentInChildren<TextMeshProUGUI>();
        name.SetText(patient.name);
        
        while(true)
        {
            healthSlider.value = patientLifespan.GetPatientHealth();
            
            if (patientLifespan.GetPatientHealth() <= 0)
            {
                deathTimeAccumulator -= Time.deltaTime;
                if (deathTimeAccumulator <= 0)
                {
                    patientHealthbars.Remove(patientHealthbar);
                    Destroy(patientHealthbar);
                    Destroy(patient);
                    
                    UpdateHealthBarPositions();
                    break;
                }
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
}
