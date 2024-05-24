using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class SoundOnCollision : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] audioClips;
    public float soundRange = 100;
    public float soundCooldown = 0f;
    private float soundCooldownTimer = 0f;
    public float velocityThreshold = 5f;
    public float maxVelocity = 10f;
    public float minVolume = 0f;
    public float maxVolume = 1f;
    public float pitchMultiplier = 1f;
    public float pitchRandomness = 0f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.maxDistance = soundRange;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }
    void Update()
    {
        soundCooldownTimer -= Time.deltaTime;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > velocityThreshold && soundCooldownTimer <= 0)
        {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, collision.relativeVelocity.magnitude / maxVelocity);
            audioSource.pitch = pitchMultiplier * Random.Range(1 - pitchRandomness, 1 + pitchRandomness) * Mathf.Lerp(0.5f, 1.5f, collision.relativeVelocity.magnitude / maxVelocity);
            audioSource.PlayOneShot(audioSource.clip);
            soundCooldownTimer = soundCooldown;
        }
    }
}
