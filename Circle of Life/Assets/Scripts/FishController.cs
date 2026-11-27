
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FishController : MonoBehaviour
{
    public float swimSpeed = 5f;
    public float rotationSpeed = 5f;
    public float acceleration = 4f;

    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float forward = Input.GetAxisRaw("Vertical");
        float vertical = 0f;

        if (Input.GetKey(KeyCode.Space))
            vertical = 1f;

        if (Input.GetKey(KeyCode.LeftControl))
            vertical = -1f;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        movement = (
            cameraForward * forward +
            cameraRight * horizontal +
            Vector3.up * vertical
        ).normalized;
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = movement * swimSpeed;

        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );

        Vector3 facing = new Vector3(
            movement.x, 0f, movement.z
        );

        if (facing.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(facing);

            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            ));
        }
    }
}
