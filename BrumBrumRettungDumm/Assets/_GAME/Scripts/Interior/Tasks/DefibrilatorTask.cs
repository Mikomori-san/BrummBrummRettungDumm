using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Scripts.Interior;

public class DefibrilatorTask : MonoBehaviour
{
    private float progress = 0f;
    private bool isMakingProgress = false;
    private float defibrilatorCooldown = 0;
    private bool patientRevived = false;
    public float radius = 5.0F;
    public float power = 100.0F;
    
    private Camera paramedicCamera;
    [SerializeField] private GameObject defibrilator;
    [SerializeField] private float chargeModifier = 1f;
    [SerializeField] private int healthIncrease = 30;
    [SerializeField] private DefibrilatorUI defiUI;
    RaycastHit[] results = new RaycastHit[10];
    
    // Start is called before the first frame update
    void Start()
    {
        paramedicCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
        PlayerInput playerInput = InputSafe.instance.GetParamedic().GetComponent<PlayerInput>();
        playerInput.onActionTriggered += Input_GiveDefi;
    }

    // Update is called once per frame
    void Update()
    {
        if (defibrilatorCooldown > 0)
        {
            defibrilatorCooldown -= Time.deltaTime;
        }
        
        if (ObjectDragging.Instance.grabbedObject && ObjectDragging.Instance.grabbedObject.name == defibrilator.name && defibrilatorCooldown <= 0)
        {
            if (isMakingProgress)
            {
                defiUI.ShowDefibrilatorUI();
                progress += 10f * chargeModifier * Time.deltaTime;
                // ProgressBar Load
                if (progress >= 100)
                {
                    defiUI.HideDefibrilatorUI();
                    patientRevived = false;
                    float maxRange = 5f;
                    Ray ray = paramedicCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    var size = Physics.RaycastNonAlloc(ray, results, maxRange);

                    if (results.Length > 0)
                    {
                        for(int i = 0; i < size; i++)
                        {
                            if (results[i].collider && results[i].collider.gameObject.name == "Spine_02")
                            {   
                                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                                results[i].collider.gameObject.GetComponentInParent<PatientLifespan>().IncreasePatientHealth(healthIncrease);
                                patientRevived = true;
                                ScoreSystem.Instance.AddScoreDefibrilator();
                                
                                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                                EmitShockWave();
                                break;
                            }
                        }
                    }
                    progress = 0;
                    defibrilatorCooldown = 2;
                }
            }
            else if(progress != 0)
            {
                // ProgressBar -> 0
                if (progress < 0)
                {
                    progress = 0;
                    defiUI.HideDefibrilatorUI();
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
                defiUI.HideDefibrilatorUI();
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
        if (context.action.name != "Give")
            return;

        if (context.started)
        {
            isMakingProgress = true;
        }
        else if(context.canceled)
        {
            isMakingProgress = false;
        }
    }

    public float GetProgress()
    {
        return this.progress;
    }

    void EmitShockWave()
    {
        Vector3 explosionPos = defibrilator.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
}
