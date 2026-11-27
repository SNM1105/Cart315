
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FishAI : MonoBehaviour
{
    [Header("Movement")]
    public float swimSpeed = 2f;
    public float rotationSpeed = 3f;
    public float changeDirectionTime = 4f;

    [Header("Swimming Area")]
    public float swimArea = 40f;
    public float minHeight = 3f;
    public float maxHeight = 20f;

    [Header("Predator and Prey")]
    public float detectionRange = 8f;
    public float chaseSpeed = 4f;
    public float fleeSpeed = 3.5f;
    public float sizeRatio = 1.1f;

    private Rigidbody rb;
    private FishSize mySize;

    private Transform player;
    private FishSize playerSize;

    private Vector3 targetPosition;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mySize = GetComponent<FishSize>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerSize = playerObject.GetComponent<FishSize>();
        }

        PickNewTarget();
    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;

        Vector3 direction = targetPosition - rb.position;
        float currentSpeed = swimSpeed;

        if (timer <= 0f || direction.magnitude < 1f)
        {
            PickNewTarget();
            direction = targetPosition - rb.position;
        }

        if (player != null && playerSize != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                player.position
            );

            if (distance < detectionRange)
            {
                // Bigger fish chase the player
                if (mySize.size >= playerSize.size * sizeRatio)
                {
                    direction = player.position - rb.position;
                    currentSpeed = chaseSpeed;
                }

                // Smaller fish flee from the player
                else if (playerSize.size >= mySize.size * sizeRatio)
                {
                    direction = rb.position - player.position;
                    currentSpeed = fleeSpeed;
                }
            }
        }

        // Keep fish inside the swimming area
        if (Mathf.Abs(rb.position.x) > swimArea ||
            Mathf.Abs(rb.position.z) > swimArea ||
            rb.position.y < minHeight ||
            rb.position.y > maxHeight)
        {
            Vector3 center = new Vector3(
                0f,
                (minHeight + maxHeight) / 2f,
                0f
            );

            direction = center - rb.position;
        }

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        Quaternion newRotation = Quaternion.Slerp(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(newRotation);

        rb.linearVelocity =
            newRotation * Vector3.forward * currentSpeed;
    }

    void PickNewTarget()
    {
        targetPosition = new Vector3(
            Random.Range(-swimArea, swimArea),
            Random.Range(minHeight, maxHeight),
            Random.Range(-swimArea, swimArea)
        );

        timer = Random.Range(
            changeDirectionTime * 0.5f,
            changeDirectionTime * 1.5f
        );
    }
}
