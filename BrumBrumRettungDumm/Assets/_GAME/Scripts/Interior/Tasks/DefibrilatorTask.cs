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
    public float radius = 5.0F;
    public float power = 100.0F;
    
    private Camera paramedicCamera;
    [SerializeField] private GameObject defibrilator;
    [SerializeField] private float chargeModifier = 1f;
    [SerializeField] private int healthIncrease = 30;
    [SerializeField] private DefibrilatorUI defiUI;
    [SerializeField] private float maxRange = 5f;
    RaycastHit[] results = new RaycastHit[10];

    [SerializeField]private AudioClip defiChargeSound;
    [SerializeField]private AudioClip defiShockSound;
    [SerializeField]private AudioSource defiPaddles;
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
                defiPaddles.pitch = defiPaddles.pitch + 0.1f * Time.deltaTime;
                defiUI.ShowDefibrilatorUI();
                progress += 10f * chargeModifier * Time.deltaTime;
                // ProgressBar Load
                if (progress >= 100)
                {
                    defiUI.HideDefibrilatorUI();
                    Ray ray = paramedicCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    var size = Physics.RaycastNonAlloc(ray, results, maxRange);

                    if (results.Length > 0)
                    {
                        for(int i = 0; i < size; i++)
                        {
                            if (results[i].collider && results[i].collider.gameObject.name == "Spine_02" && results[i].collider.gameObject.GetComponentInParent<PatientLifespan>().GetPatientHealth() <= 0)
                            {   
                                defiPaddles.loop = false;
                                defiPaddles.Stop();
                                defiPaddles.pitch = 1;
                                StartCoroutine(PlayShockSound(results[i].collider.gameObject));
                                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                                results[i].collider.gameObject.GetComponentInParent<PatientLifespan>().IncreasePatientHealth(healthIncrease);
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
            if (ObjectDragging.Instance.grabbedObject && ObjectDragging.Instance.grabbedObject.name == defibrilator.name && defibrilatorCooldown <= 0)
            {
                defiPaddles.pitch = 1;
                defiPaddles.clip = defiChargeSound;
                defiPaddles.Play();
                defiPaddles.loop = true;
            }
            isMakingProgress = true;
        }
        else if(context.canceled)
        {
            defiPaddles.loop = false;
            if(defiPaddles.clip == defiChargeSound)
                defiPaddles.Stop();
            defiPaddles.pitch = 1;
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
    private IEnumerator PlayShockSound(GameObject gameObject)
    {
        gameObject.GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.4f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(defiShockSound);
        while (gameObject.GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }
    }
}
