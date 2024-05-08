using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefibrilatorTask : MonoBehaviour
{
    private float progress = 0f;
    private bool isMakingProgress = false;
    private float timer = 0;
    private bool patientRevived = false;
    
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject defibrilator;
    [SerializeField] private float chargeModifier = 1f;
    [SerializeField] private int healthIncrease = 30;
    RaycastHit[] results = new RaycastHit[10];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        
        if (ObjectDragging.Instance.grabbedObject && ObjectDragging.Instance.grabbedObject.name == defibrilator.name && timer <= 0)
        {
            if (isMakingProgress)
            {
                progress += 10f * chargeModifier * Time.deltaTime;
                // ProgressBar Load
                if (progress >= 100)
                {
                    patientRevived = false;
                    float maxRange = 5f;
                    Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    var size = Physics.RaycastNonAlloc(ray, results, maxRange);

                    if (results.Length > 0)
                    {
                        for(int i = 0; i < size; i++)
                        {
                            if (results[i].collider && results[i].collider.gameObject.name == "Torso")
                            {   
                                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                                results[i].collider.gameObject.GetComponentInParent<PatientLifespan>().IncreasePatientHealth(healthIncrease);
                                patientRevived = true;
                                ScoreSystem.Instance.AddScoreDefibrilator();
                                break;
                            }
                        }
                    }
                    progress = 0;
                    timer = 2;
                    
                    if(patientRevived)
                        print("Patient revived");
                    else
                        print("Patient not revived");
                }
                print("Progress: " + progress);
            }
            else if(progress != 0)
            {
                // ProgressBar -> 0
                if (progress < 0)
                {
                    progress = 0;
                }
                else
                {
                    progress -= 0.1f;
                }
                print("Progress: " + progress);
            }
        }
        else if(progress != 0)
        {
            // ProgressBar -> 0
            if (progress < 0)
            {
                progress = 0;
            }
            else
            {
                progress -= 0.1f;
            }
            print("Progress: " + progress);
        }
    }

    public void Input_GiveDefi(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isMakingProgress = true;
        }
        else if(context.canceled)
        {
            isMakingProgress = false;
        }
    }
}
