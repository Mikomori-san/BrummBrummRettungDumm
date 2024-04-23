using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PillManager : MonoBehaviour
{
    private GameObject selectedPill;
    [SerializeField] private Camera cam;

    public Queue<GameObject> AvailablePills = new Queue<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Input_GivePill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ObjectDragging.Instance.isDragging && ObjectDragging.Instance.grabbedObject.CompareTag("Pill"))
            {
                selectedPill = ObjectDragging.Instance.grabbedObject;
            
                RaycastHit hit;
                Vector3 screenMiddle = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
                Ray ray = cam.ScreenPointToRay(screenMiddle);

                float maxRange = 2f;

                if (Physics.Raycast(ray, out hit, maxRange))
                {
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("Patient"))
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
