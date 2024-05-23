using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class ForceObjectLogic : MonoBehaviour
{
    public float forceMultiplier = -1f;
    public float torqueMultiplier = 0f;
    public float gravityMultiplier = 0f;
    private AudioSource audioSource;
    private Rigidbody rb;
    public AudioClip audioClipOnCollision;
    public float soundRange = 10;
    public float soundCooldown = .5f;
    private float soundCooldownTimer = 0;
    public float velocityThreshold = 10f;
    public float minVolume = 0f;
    public float maxVolume = 0.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioClipOnCollision = audioClipOnCollision == null ? Resources.Load<AudioClip>("collision") : audioClipOnCollision;
        audioSource.clip = audioClipOnCollision;
        audioSource.maxDistance = soundRange;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ForceController.Instance.AddForceObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        soundCooldownTimer -= Time.deltaTime;
    }

    public void DisableForce()
    {
        ForceController.Instance.RemoveForceObject(this);
    }

    public void EnableForce()
    {
        ForceController.Instance.AddForceObject(this);
    }

    public void OnDestroy()
    {
        ForceController.Instance.RemoveForceObject(this);
    }

    public void OnDisable()
    {
        ForceController.Instance.RemoveForceObject(this);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (soundCooldownTimer > 0) 
            return;

        if (collision.relativeVelocity.magnitude > 5f)
        {
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, collision.relativeVelocity.magnitude / 20);
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f) * Mathf.Lerp(0.5f, 1.5f,collision.relativeVelocity.magnitude / 20) * Mathf.Lerp(0.3f, 3f, rb.mass);
            audioSource.Play();
            soundCooldownTimer = soundCooldown;
        }
    }
}
