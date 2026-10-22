using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 5f;

    private Vector2 direction;
    private Snake snake;
    private bool hasHit = false;

    public void Initialize(Vector2 targetDirection)
    {
        direction = targetDirection.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x)
                      * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Start()
    {
        snake = FindFirstObjectByType<Snake>();

        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 newPosition =
            rb.position + direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit || snake == null) return;

        // Check if laser hits the snake's head
        if (other.GetComponent<Snake>() != null)
        {
            HitSnake();
        }

        // Check if laser hits a body segment
        else if (other.CompareTag("SnakeBody"))
        {
            HitSnake();
        }
    }

    private void HitSnake()
    {
        hasHit = true;

        snake.TakeDamage();

        Debug.Log("Laser hit the snake!");

        Destroy(gameObject);
    }
}