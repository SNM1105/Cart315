using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float attackInterval = 1f;
    public GameObject laserPrefab;

    private Snake snake;
    private float attackTimer;

    private void Start()
    {
        snake = FindFirstObjectByType<Snake>();

        if (snake == null)
        {
            Debug.LogError("ERROR: Snake not found!");
        }

        attackTimer = attackInterval;
    }

    private void Update()
    {
        if (snake == null) return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            ShootLaser();
            attackTimer = attackInterval;
        }
    }

    private void ShootLaser()
    {
        if (laserPrefab == null)
        {
            Debug.LogError("Laser prefab not assigned!");
            return;
        }

        // Find direction toward the snake
        Vector2 direction =
            (snake.transform.position - transform.position).normalized;

        // Spawn laser slightly in front of enemy
        Vector2 spawnPosition =
            (Vector2)transform.position + direction * 0.6f;

        // Create laser
        GameObject laserObject = Instantiate(
            laserPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Tell laser which direction to travel
        Laser laser = laserObject.GetComponent<Laser>();

        if (laser != null)
        {
            laser.Initialize(direction);
        }

        Debug.Log("Enemy fired a laser!");
    }

    public void GetEaten()
    {
        if (snake != null)
        {
            snake.Grow();
        }

        Debug.Log("Enemy eaten!");

        Destroy(gameObject);
    }
}