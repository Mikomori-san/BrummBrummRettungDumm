    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PillManager : MonoBehaviour
{
    private Queue<GameObject> AvailablePills = new Queue<GameObject>();
    private GameObject selectedPill;
    private RaycastHit[] results = new RaycastHit[10];
    
    private Camera paramedicCamera;
    [SerializeField] private GameObject pillPrefab;
    [SerializeField] private int pillAmount = 10;
    [SerializeField] private Transform pillSpawnPos;
    [SerializeField] private short lifeToRestore = 40;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = InputSafe.instance.GetParamedic().GetComponent<PlayerInput>();
        playerInput.onActionTriggered += Input_GivePill;

        paramedicCamera = InputSafe.instance.GetParamedic().GetComponentInChildren<Camera>();
        for (int i = 0; i < pillAmount; i++)
        {
            GameObject pill = Instantiate(pillPrefab, pillSpawnPos.position, Quaternion.identity, pillSpawnPos);
            pill.SetActive(false);
            AvailablePills.Enqueue(pill);
        }
        StartCoroutine(SpawnPill());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Input_GivePill(InputAction.CallbackContext context)
    {
        if(context.action.name != "Give") 
            return;

        if (context.performed)
        {
            if (ObjectDragging.Instance.grabbedObject && ObjectDragging.Instance.grabbedObject.CompareTag("Pill"))
            {
                selectedPill = ObjectDragging.Instance.grabbedObject;
                
                float maxRange = 5f;
                Ray ray = paramedicCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                var size = Physics.RaycastNonAlloc(ray, results, maxRange);

                if (results.Length > 0)
                {
                    for(int i = 0; i < size; i++)
                    {
                        if (results[i].collider != null && results[i].collider.gameObject.name == "Head")
                        {
                            results[i].collider.gameObject.GetComponentInParent<PatientLifespan>().IncreasePatientHealth(lifeToRestore);
                            AvailablePills.Enqueue(selectedPill);
                            selectedPill.GetComponent<Collider>().enabled = true;
                            selectedPill.SetActive(false);
                            ObjectDragging.Instance.grabbedObject = null;
                            ScoreSystem.Instance.AddScorePill();
                        }
                    }
                }
            }
        }
    }
    
    private IEnumerator SpawnPill()
    {
        while(true)
        {
            if(AvailablePills.Count != 0)
            {
                GameObject pill = AvailablePills.Dequeue();
                pill.SetActive(true);
                pill.transform.position = pillSpawnPos.position;
                print("Pill spawned! " + pill.transform.position);
            }

            yield return new WaitForSeconds(2f);
        }
    }
    
    private Vector3 GetRandomPoint()
    {
        float areaSize = 50f;

        while (true)
        {
            Vector3 randomPoint = new Vector3
            (
                UnityEngine.Random.Range(-areaSize / 2f, areaSize / 2f),
                50f,
                UnityEngine.Random.Range(-areaSize / 2f, areaSize / 2f)
            );
            
            Ray ray = new Ray(randomPoint, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                if (hit.collider.gameObject.CompareTag("InteriorGround"))
                { 
                    return hit.point + new Vector3(0, 3, 0);
                }
            }
        }
    }
}
