using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShakeAllCamerasOnCollision : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private GameObject[] cameras;

    // How long the object should shake for.
    public float shakeDuration = 0.5f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmountMultiplier = 1f;
    public float maxShakeAmount = 1f;
    public float velocityThreshold = 10f;

    private Coroutine activeCoroutine;

    private void Start()
    {
        cameras = GameObject.FindGameObjectsWithTag("MainCamera");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > velocityThreshold && activeCoroutine == null)
        {
            activeCoroutine = StartCoroutine(shakeCoroutine(cameras, collision.relativeVelocity.magnitude * 0.01f * shakeAmountMultiplier));
        }
    }
    public IEnumerator shakeCoroutine(GameObject[] shakeThis, float shakeAmount)
    {
        Vector3[] originalLocalPositions = new Vector3[shakeThis.Length];
        for (int i = 0; i < shakeThis.Length; i++)
        {
            originalLocalPositions[i] = shakeThis[i].transform.localPosition;
        }
        shakeAmount = Mathf.Clamp(shakeAmount, 0, maxShakeAmount);
        float duration = shakeDuration;
        Vector3[] shakeLastFrame = new Vector3[shakeThis.Length];
        while (duration > 0)
        {
            for (int i = 0; i < shakeThis.Length; i++)
            {
                Vector3 shake = Random.insideUnitSphere * shakeAmount;
                shakeThis[i].transform.localPosition = shakeThis[i].transform.localPosition + shake - shakeLastFrame[i];
                shakeLastFrame[i] = shake;
            }
            duration -= Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < shakeThis.Length; i++)
        {
            shakeThis[i].transform.localPosition = originalLocalPositions[i];
        }
        activeCoroutine = null;
    }
}