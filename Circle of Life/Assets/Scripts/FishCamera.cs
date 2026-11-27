
using UnityEngine;

public class FishCamera : MonoBehaviour
{
    public Transform target;

    public float distance = 6f;
    public float height = 2f;
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        if (target == null)
            return;

        float sizeMultiplier = target.localScale.x / 0.6f;

        Vector3 desiredPosition =
            target.position
            - target.forward * distance * sizeMultiplier
            + Vector3.up * height * sizeMultiplier;
        
                transform.position = Vector3.Lerp(
                    transform.position,
                    desiredPosition,
                    followSpeed * Time.deltaTime
                );

        Vector3 lookDirection =
            target.position - transform.position;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion desiredRotation =
                Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                desiredRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
