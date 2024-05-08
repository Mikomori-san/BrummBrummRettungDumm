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
    
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject pillPrefab;
    [SerializeField] private int pillAmount = 5;
    [SerializeField] private Transform pillSpawnPos;
    
    // Start is called before the first frame update
    void Start()
    {
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
        if (context.performed)
        {
            if (ObjectDragging.Instance.grabbedObject && ObjectDragging.Instance.grabbedObject.CompareTag("Pill"))
            {
                selectedPill = ObjectDragging.Instance.grabbedObject;
                
                float maxRange = 5f;
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                var size = Physics.RaycastNonAlloc(ray, results, maxRange);

                if (results.Length > 0)
                {
                    for(int i = 0; i < size; i++)
                    {
                        if (results[i].collider != null && results[i].collider.gameObject.name == "Head")
                        {
                            results[i].collider.gameObject.GetComponentInParent<PatientLifespan>().IncreasePatientHealth(20);
                            selectedPill.SetActive(false);
                            AvailablePills.Enqueue(selectedPill);
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
            GameObject pill;
            if(AvailablePills.Count == 0)
            {
                pill = Instantiate(pillPrefab, pillSpawnPos.position, Quaternion.identity, pillSpawnPos);
                //pill.transform.parent = pillSpawnPos;
            }
            else
            {
                pill = AvailablePills.Dequeue();
            }

            //Vector3 randomSpawnPoint = GetRandomPoint();

            pill.transform.position = pillSpawnPos.position;
            //pill.transform.SetParent(interior.transform); // Set the parent to the interior object
            pill.SetActive(true);

            print("Spawn");
            yield return new WaitForSeconds(5f);
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
