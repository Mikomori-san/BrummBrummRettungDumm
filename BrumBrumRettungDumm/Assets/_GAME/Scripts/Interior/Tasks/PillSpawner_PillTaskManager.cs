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

    [SerializeField] private GameObject head;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pillAmount; i++)
        {
            GameObject pill = Instantiate(pillPrefab, transform);
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
            
                Vector3 screenMiddle = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
                float maxRange = 5f;
                Ray ray = cam.ScreenPointToRay(screenMiddle);
                var size = Physics.RaycastNonAlloc(ray, results, maxRange);

                if (results.Length > 0)
                {
                    for(int i = 0; i < size; i++)
                    {
                        if (results[i].collider != null && results[i].collider.gameObject.name == head.name)
                        {
                            PatientLifespan.Instance.IncreasePatientHealth(20);
                            selectedPill.SetActive(false);
                            AvailablePills.Enqueue(selectedPill);
                            ObjectDragging.Instance.grabbedObject = null;
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
            if (AvailablePills.Count > 0)
            {
                GameObject pill = AvailablePills.Dequeue();
                
                Vector3 randomSpawnPoint = GetRandomPoint();
                
                pill.transform.position = randomSpawnPoint;
                pill.SetActive(true);
            }
            print("Spawn");
            yield return new WaitForSeconds(5f);
        }
    }
    
    private Vector3 GetRandomPoint()
    {
        float areaSize = 50f;

        while (true)
        {
            Vector3 randomPoint = new Vector3(
                UnityEngine.Random.Range(-areaSize / 2f, areaSize / 2f),
                50f,
                UnityEngine.Random.Range(-areaSize / 2f, areaSize / 2f)
            );
            
            Ray ray = new Ray(randomPoint, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                return hit.point + new Vector3(0, 3, 0);
            }
        }
    }
}
