using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    public GameObject shieldPrefab;
    public Collider2D gridArea;
    public float spawnInterval = 15f;

    private Snake snake;
    private float nextSpawn;

    private void Start()
    {
        snake = FindFirstObjectByType<Snake>();
        nextSpawn = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawn)
        {
            SpawnShield();
            nextSpawn = Time.time + spawnInterval;
        }
    }

    private void SpawnShield()
    {
        // Don't spawn another if one is already on the board
        if (FindFirstObjectByType<ShieldPickup>() != null)
            return;

        if (snake == null || shieldPrefab == null || gridArea == null)
        {
            Debug.LogError("ShieldSpawner is missing a reference!");
            return;
        }

        Bounds bounds = gridArea.bounds;

        for (int attempt = 0; attempt < 100; attempt++)
        {
            int x = Random.Range(
                Mathf.CeilToInt(bounds.min.x),
                Mathf.FloorToInt(bounds.max.x) + 1
            );

            int y = Random.Range(
                Mathf.CeilToInt(bounds.min.y),
                Mathf.FloorToInt(bounds.max.y) + 1
            );

            // Don't spawn on the snake
            if (snake.Occupies(x, y))
                continue;

            // Don't spawn on an enemy
            bool occupied = false;

            Enemy[] enemies =
                FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            foreach (Enemy enemy in enemies)
            {
                Vector2 pos = enemy.transform.position;

                if (Mathf.RoundToInt(pos.x) == x &&
                    Mathf.RoundToInt(pos.y) == y)
                {
                    occupied = true;
                    break;
                }
            }

            if (occupied)
                continue;

            Instantiate(
                shieldPrefab,
                new Vector2(x, y),
                Quaternion.identity
            );

            Debug.Log("Shield spawned!");
            return;
        }
    }
}