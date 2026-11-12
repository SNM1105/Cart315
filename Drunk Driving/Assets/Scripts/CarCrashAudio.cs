
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CarCrashAudio : MonoBehaviour
{
    [Header("Crash Audio")]
    public AudioClip crashSound;

    [Range(0f, 1f)]
    public float crashVolume = 1f;

    [Header("Collision Settings")]
    public float minimumSpeed = 5f;
    public float crashCooldown = 1f;

    private CharacterController controller;
    private AudioSource crashSource;
    private float lastCrashTime = -999f;
    private Vector3 previousPosition;
    private float currentSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        crashSource = gameObject.AddComponent<AudioSource>();
        crashSource.playOnAwake = false;
        crashSource.spatialBlend = 0f;

        previousPosition = transform.position;
    }

    void Update()
    {
        currentSpeed = Vector3.Distance(
            transform.position,
            previousPosition
        ) / Mathf.Max(Time.deltaTime, 0.0001f);

        previousPosition = transform.position;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.gameObject.CompareTag("Obstacle"))
            return;

        // Ignore floor-like contacts
        if (Mathf.Abs(hit.normal.y) > 0.5f)
            return;

        if (Time.time - lastCrashTime < crashCooldown)
            return;

        if (currentSpeed >= minimumSpeed)
        {
            float volume = Mathf.Clamp01(
                currentSpeed / 25f
            ) * crashVolume;

            crashSource.PlayOneShot(crashSound, volume);

            lastCrashTime = Time.time;
        }
    }
}
