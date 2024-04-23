using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

enum TiltDirection
{
    left,
    right,
    forward,
    back
}
public class ForceController : MonoBehaviour
{
    public static ForceController Instance { get; private set; }

    private List<GameObject> forceObjects = new List<GameObject>();

    private GameObject ambulance;

    private float timer = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        ambulance = GameObject.FindGameObjectWithTag("Ambulance");
    }

    void FixedUpdate()
    {
        if (timer > 1.5f)
        {
            int randomNr = Random.Range(0, 4);
            Vector3 force = Vector3.zero;
            switch (randomNr)
            {
                case 0:
                    force = Vector3.forward;
                    StartCoroutine(TiltGameObject(ambulance, 1f, 30f, TiltDirection.forward));
                    break;
                case 1:
                    force = Vector3.back;
                    StartCoroutine(TiltGameObject(ambulance, 1f, -30f, TiltDirection.back));
                    break;
                case 2:
                    force = Vector3.left;
                    StartCoroutine(TiltGameObject(ambulance, 1f, 30f, TiltDirection.left));
                    break;
                case 3:
                    force = Vector3.right;
                    StartCoroutine(TiltGameObject(ambulance, 1f, -30f, TiltDirection.right));
                    break;
                default:
                    break;
            }

            for (int i = 0; i < forceObjects.Count; i++)
            {
                Vector3 position = forceObjects[i].transform.position;
                Ray ray = new Ray(position, Vector3.down);
                float maxDistance = 1f;

                if (Physics.Raycast(ray, maxDistance, LayerMask.GetMask("Ground")))
                {
                    forceObjects[i].GetComponent<Rigidbody>().AddForce(force * 2, ForceMode.Impulse);
                }

                Vector3 lastPosition = forceObjects[i].GetComponent<ForceObjectLogic>().lastPosition;
                Ray correctionRay = new Ray(position, lastPosition - position);
                maxDistance = Vector3.Distance(lastPosition, position);
                
                if (Physics.Raycast(correctionRay, maxDistance, LayerMask.GetMask("Ground")))
                {
                    forceObjects[i].transform.position = lastPosition;
                }
                
                forceObjects[i].GetComponent<ForceObjectLogic>().lastPosition = forceObjects[i].transform.position;
            }
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    private IEnumerator TiltGameObject(GameObject gameObject, float tiltDuration, float tiltAngle, TiltDirection dir)
    {
        Quaternion startRotation = gameObject.transform.rotation;
        Quaternion targetRotation;
        switch (dir)
        {
            case TiltDirection.left:
            case TiltDirection.right:
                targetRotation = Quaternion.Euler(0f, 0f, tiltAngle);
                break;
            case TiltDirection.forward:
            case TiltDirection.back:
                targetRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
                break;
            default:
                targetRotation = Quaternion.Euler(0f, 0f, 0f);
                break;
        }
         

        float elapsedTime = 0f;
        while (elapsedTime < tiltDuration / 2)
        {
            float t = elapsedTime / tiltDuration;
            gameObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedTime < tiltDuration)
        {
            float t = elapsedTime / tiltDuration;
            gameObject.transform.rotation = Quaternion.Lerp(targetRotation, startRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.rotation = startRotation;
    }

    public void AddForceObject(GameObject forceObject)
    {
        forceObjects.Add(forceObject);
    }

    public void RemoveForceObject(GameObject forceObject)
    {
        forceObjects.Remove(forceObject);
    }
}
