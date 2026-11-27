
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;
    public Transform player;

    [Header("Population")]
    public int fishCount = 200;
    public float spawnArea = 140f;

    [Header("Fish Sizes")]
    public float minSize = 0.2f;
    public float maxSize = 2.5f;

    [Header("Depth")]
    public float minHeight = 5f;
    public float maxHeight = 40f;

    [Header("Starting Area")]
    public float safeRadius = 30f;
    public int starterFishCount = 15;

    void Start()
    {
        for (int i = 0; i < fishCount; i++)
        {
            SpawnFish(i < starterFishCount);
        }
    }

    void SpawnFish(bool starterFish)
    {
        float size;
        Vector3 spawnPosition;

        if (starterFish)
        {
            size = Random.Range(0.2f, 0.45f);

            Vector2 offset = Random.insideUnitCircle.normalized
                * Random.Range(5f, 25f);

            spawnPosition = player.position + new Vector3(
                offset.x,
                Random.Range(-2f, 5f),
                offset.y
            );

            spawnPosition.y = Mathf.Clamp(
                spawnPosition.y, minHeight, maxHeight
            );
        }
        else
        {
            size = Random.Range(minSize, maxSize);

            do
            {
                spawnPosition = new Vector3(
                    Random.Range(-spawnArea, spawnArea),
                    Random.Range(minHeight, maxHeight),
                    Random.Range(-spawnArea, spawnArea)
                );
            }
            while (Vector3.Distance(
                spawnPosition, player.position
            ) < safeRadius);
        }

        GameObject fish = Instantiate(
            fishPrefab,
            spawnPosition,
            Quaternion.Euler(0, Random.Range(0f, 360f), 0)
        );

        fish.transform.localScale =
            new Vector3(size, size, size * 2f);

        FishAI ai = fish.GetComponent<FishAI>();

        if (ai != null)
        {
            ai.swimSpeed = Random.Range(1.5f, 3.5f);
            ai.swimArea = spawnArea;
            ai.minHeight = minHeight;
            ai.maxHeight = maxHeight;
        }
    }
}
