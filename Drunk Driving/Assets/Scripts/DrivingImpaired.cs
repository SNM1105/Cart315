
using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingImpairment : MonoBehaviour
{
    [Header("Drinking Animation")]
    public DrinkCanAnimation drinkAnimation;

    [Header("Impairment Settings")]
    [Range(0f, 1f)]
    public float impairment = 0f;

    public float impairmentIncrease = 0.2f;

    [Header("Swerving Settings")]
    public float swerveStrength = 15f;
    public float swerveSpeed = 2f;

    private int drinkCount = 0;

    [Header("Recovery Settings")]
    public float recoveryDelay = 5f;
    public float recoverySpeed = 0.025f;

    private float lastDrinkTime;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // Increase the fictional impairment level
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            Drink();
        }

        // Only swerve while driving forward
        if (Keyboard.current.wKey.isPressed)
        {
            ApplySwerve();
        }

        Recover();
    }

    
void Drink()
{
    if (drinkAnimation != null &&
        drinkAnimation.IsAnimating)
    {
        return;
    }

    if (drinkAnimation != null)
    {
        Debug.Log("Starting can animation");
        drinkAnimation.PlayDrinkAnimation();
    }
    else
    {
        Debug.LogWarning("Drink Animation is not assigned!");
    }

    drinkCount++;

    impairment = Mathf.Clamp01(
        impairment + impairmentIncrease
    );

    lastDrinkTime = Time.time;

    Debug.Log("Drinks: " + drinkCount);
    Debug.Log("Impairment: " + impairment);
}


    void ApplySwerve()
    {
        float swerve = Mathf.Sin(
            Time.time * swerveSpeed
        );

        float rotation = swerve
            * swerveStrength
            * impairment
            * Time.deltaTime;

        transform.Rotate(
            0f, rotation, 0f, Space.World
        );
    }

    void Recover()
{
    if (Time.time - lastDrinkTime >= recoveryDelay)
    {
        impairment = Mathf.MoveTowards(
            impairment,
            0f,
            recoverySpeed * Time.deltaTime
        );
    }
}
}
