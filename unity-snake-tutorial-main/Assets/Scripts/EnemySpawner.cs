using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Collider2D gridArea;
    public float spawnInterval = 4f;

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
            SpawnEnemy();
            nextSpawn = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
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

            if (snake.Occupies(x, y))
                continue;

            Vector2 position = new Vector2(x, y);

            Instantiate(enemyPrefab, position, Quaternion.identity);
            Debug.Log("Enemy spawned at: " + position);
            return;
        }
    }
}