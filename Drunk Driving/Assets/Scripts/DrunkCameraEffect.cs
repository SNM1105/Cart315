
using UnityEngine;

public class DrunkCameraEffect : MonoBehaviour
{
    public Material distortionMaterial;
    public DrivingImpairment drivingImpairment;

    [Header("Distortion Settings")]

    public float maxIntensity = 1.5f;

    public float transitionSpeed = 0.5f;

    private float currentIntensity = 0f;

    void Start()
    {
        if (distortionMaterial != null)
        {
            distortionMaterial.SetFloat("_Intensity", 0f);
        }
    }

    void Update()
    {
        if (drivingImpairment == null ||
            distortionMaterial == null)
            return;

        // Get the current impairment level
        float targetIntensity =
            drivingImpairment.impairment * maxIntensity;

        // Gradually increase the visual effect
        currentIntensity = Mathf.MoveTowards(
            currentIntensity,
            targetIntensity,
            transitionSpeed * Time.deltaTime
        );

        // Apply the effect to the material
        distortionMaterial.SetFloat(
            "_Intensity",
            currentIntensity
        );
    }

    void OnDisable()
    {
        if (distortionMaterial != null)
        {
            distortionMaterial.SetFloat("_Intensity", 0f);
        }
    }
}

