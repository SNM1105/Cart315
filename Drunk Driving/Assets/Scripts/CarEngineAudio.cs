
using UnityEngine;
using UnityEngine.InputSystem;

public class CarEngineAudio : MonoBehaviour
{
    [Header("Engine Sounds")]
    public AudioClip idleSound;
    public AudioClip accelerateSound;
    public AudioClip drivingSound;
    public AudioClip decelerateSound;
    [Header("Engine Volume")]
[Range(0f, 1f)]
public float engineVolume = 0.3f;

    private AudioSource engineSource;
    private bool wasAccelerating;
    private bool isDriving;
    private bool isDecelerating;

    void Start()
    {
        engineSource = gameObject.AddComponent<AudioSource>();

        engineSource.playOnAwake = false;
        engineSource.spatialBlend = 0f;
        engineSource.volume = engineVolume;

        PlayIdle();
    }

    void Update()
    {
        bool pressingForward = Keyboard.current != null &&
                       Keyboard.current.wKey.isPressed;

        if (pressingForward && !wasAccelerating)
        {
            StartAcceleration();
        }

        if (!pressingForward && wasAccelerating)
        {
            StartDeceleration();
        }

        if (pressingForward && isDriving &&
            !engineSource.isPlaying)
        {
            PlayDriving();
        }

        if (!pressingForward && isDecelerating &&
            !engineSource.isPlaying)
        {
            PlayIdle();
        }

        wasAccelerating = pressingForward;
    }

    void StartAcceleration()
    {
        isDriving = true;
        isDecelerating = false;

        engineSource.Stop();
        engineSource.clip = accelerateSound;
        engineSource.loop = false;
        engineSource.Play();
    }

    void PlayDriving()
    {
        engineSource.Stop();
        engineSource.clip = drivingSound;
        engineSource.loop = true;
        engineSource.Play();
    }

    void StartDeceleration()
    {
        isDriving = false;
        isDecelerating = true;

        engineSource.Stop();
        engineSource.clip = decelerateSound;
        engineSource.loop = false;
        engineSource.Play();
    }

    void PlayIdle()
    {
        isDriving = false;
        isDecelerating = false;

        engineSource.Stop();
        engineSource.clip = idleSound;
        engineSource.loop = true;
        engineSource.Play();
    }
}
