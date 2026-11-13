
using UnityEngine;
using System.Collections;

public class DrinkCanAnimation : MonoBehaviour
{
    [Header("Can")]
    public Transform drinkCan;

    [Header("Audio")]
    public AudioClip canOpenSound;
    public AudioClip gulpSound;

    [Range(0f, 1f)]
    public float soundVolume = 0.8f;

    [Header("Animation Positions")]
    public Vector3 hiddenPosition =
        new Vector3(0.35f, -0.65f, 0.65f);

    public Vector3 raisedPosition =
        new Vector3(0.25f, -0.18f, 0.55f);

    public Vector3 drinkingPosition =
        new Vector3(0.08f, -0.08f, 0.35f);

    [Header("Animation Timing")]
    public float raiseDuration = 0.4f;
    public float tiltDuration = 0.3f;
    public float drinkDuration = 0.8f;
    public float lowerDuration = 0.4f;

    private AudioSource audioSource;

    public bool IsAnimating { get; private set; }

    void Start()
{
    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.playOnAwake = false;
    audioSource.spatialBlend = 0f;
    audioSource.volume = soundVolume;

    drinkCan.localPosition = raisedPosition;
    drinkCan.localRotation = Quaternion.identity;

    drinkCan.gameObject.SetActive(true);
}

    public void PlayDrinkAnimation()
    {
        if (IsAnimating)
            return;

        StartCoroutine(DrinkSequence());
    }

    IEnumerator DrinkSequence()
    {
        IsAnimating = true;

        drinkCan.gameObject.SetActive(true);

        // Raise the can
        yield return MoveCan(
            hiddenPosition,
            raisedPosition,
            Quaternion.identity,
            Quaternion.identity,
            raiseDuration
        );

        // Open the can
        if (canOpenSound != null)
        {
            audioSource.PlayOneShot(canOpenSound);
        }

        yield return new WaitForSeconds(0.2f);

        // Tilt the can toward the camera
        Quaternion tiltedRotation =
            Quaternion.Euler(-70f, 0f, 0f);

        yield return MoveCan(
            raisedPosition,
            drinkingPosition,
            Quaternion.identity,
            tiltedRotation,
            tiltDuration
        );

        // Play gulp sound
        if (gulpSound != null)
        {
            audioSource.PlayOneShot(gulpSound);
        }

        yield return new WaitForSeconds(drinkDuration);

        // Lower the can
        yield return MoveCan(
            drinkingPosition,
            hiddenPosition,
            tiltedRotation,
            Quaternion.identity,
            lowerDuration
        );

        drinkCan.gameObject.SetActive(false);

        IsAnimating = false;
    }

    IEnumerator MoveCan(
        Vector3 startPosition,
        Vector3 endPosition,
        Quaternion startRotation,
        Quaternion endRotation,
        float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            t = Mathf.SmoothStep(0f, 1f, t);

            drinkCan.localPosition = Vector3.Lerp(
                startPosition,
                endPosition,
                t
            );

            drinkCan.localRotation = Quaternion.Slerp(
                startRotation,
                endRotation,
                t
            );

            yield return null;
        }

        drinkCan.localPosition = endPosition;
        drinkCan.localRotation = endRotation;
    }
}
