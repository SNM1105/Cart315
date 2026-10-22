using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public Transform segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;

    [Header("Shield Settings")]
    public int shieldHits = 0;
    public Color shieldColor = Color.cyan;

    private Color originalHeadColor;
    private Color originalBodyColor;
    private bool colorsSaved = false;

    private readonly List<Transform> segments = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;

    private void Start()
{
    originalHeadColor = GetComponent<SpriteRenderer>().color;
    originalBodyColor = segmentPrefab.GetComponent<SpriteRenderer>().color;

    colorsSaved = true;

    ResetState();
}

    private void Update()
    {
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                input = Vector2Int.up;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                input = Vector2Int.down;
            }
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                input = Vector2Int.right;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                input = Vector2Int.left;
            }
        }
    }

    private void FixedUpdate()
    {
        // Wait until the next update before proceeding
        if (Time.time < nextUpdate) {
            return;
        }

        if (input != Vector2Int.zero) {
            direction = input;
        }

        UpdateHeadRotation();

        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
        }

        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;
        transform.position = new Vector2(x, y);

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            int enemyX = Mathf.RoundToInt(enemy.transform.position.x);
            int enemyY = Mathf.RoundToInt(enemy.transform.position.y);

            if (x == enemyX && y == enemyY)
            {
                enemy.GetEaten();
                break;
            }
        }

        ShieldPickup[] shields =
            FindObjectsByType<ShieldPickup>(FindObjectsSortMode.None);

        foreach (ShieldPickup shield in shields)
        {
            int shieldX = Mathf.RoundToInt(shield.transform.position.x);
            int shieldY = Mathf.RoundToInt(shield.transform.position.y);

            if (x == shieldX && y == shieldY)
            {
                shield.Collect(this);
                break;
            }   
        }



        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);

        UpdateShieldVisuals();
    }

    public void ActivateShield()
    {
        shieldHits = 3;

        Debug.Log("Shield activated! 3 hits remaining.");

        UpdateShieldVisuals();
    }

public bool BlockWithShield()
{
    if (shieldHits <= 0)
    {
        return false;
    }

    shieldHits--;

    Debug.Log("Shield blocked a laser! Hits left: " + shieldHits);

    UpdateShieldVisuals();

    return true;
}

private void UpdateShieldVisuals()
{
    if (!colorsSaved) return;

    bool active = shieldHits > 0;

    // Head
    SpriteRenderer headRenderer = GetComponent<SpriteRenderer>();

    if (headRenderer != null)
    {
        headRenderer.color = active ? shieldColor : originalHeadColor;
    }

    // Body
    foreach (Transform segment in segments)
    {
        if (segment == transform)
            continue;

        SpriteRenderer renderer = segment.GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            renderer.color = active ? shieldColor : originalBodyColor;
        }
    }
}

public void TakeDamage()
{
    if (BlockWithShield())
    {
        return;
    }

    if (segments.Count > 1)
    {
        int lastIndex = segments.Count - 1;

        Destroy(segments[lastIndex].gameObject);
        segments.RemoveAt(lastIndex);

        Debug.Log("Damage taken! Length: " + segments.Count);
    }
    else
    {
        Debug.Log("GAME OVER!");
        ResetState();
    }
}

public void ResetState()
{
    shieldHits = 0;

    direction = Vector2Int.right;
    transform.position = Vector3.zero;

    for (int i = 1; i < segments.Count; i++)
    {
        Destroy(segments[i].gameObject);
    }

    segments.Clear();
    segments.Add(transform);

    for (int i = 0; i < initialSize - 1; i++)
    {
        Grow();
    }

    UpdateShieldVisuals();
}

public bool Occupies(int x, int y)
{
    foreach (Transform segment in segments)
    {
        if (Mathf.RoundToInt(segment.position.x) == x &&
            Mathf.RoundToInt(segment.position.y) == y) {
            return true;
        }
    }

    return false;
}

   private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Food"))
    {
        Grow();
    }
    else if (other.gameObject.CompareTag("Enemy"))
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.GetEaten();
        }
    }
    else if (other.gameObject.CompareTag("Obstacle"))
    {
        ResetState();
    }
    else if (other.gameObject.CompareTag("Wall"))
    {
        if (moveThroughWalls)
        {
            Traverse(other.transform);
        }
        else
        {
            ResetState();
        }
    }
}

    private void Traverse(Transform wall)
    {
        Vector3 position = transform.position;

        if (direction.x != 0f) {
            position.x = Mathf.RoundToInt(-wall.position.x + direction.x);
        } else if (direction.y != 0f) {
            position.y = Mathf.RoundToInt(-wall.position.y + direction.y);
        }

        transform.position = position;
    }

    private void UpdateHeadRotation()
    {
    float angle = 0f;

    if (direction == Vector2Int.right)
    {
        angle = 0f;
    }
    else if (direction == Vector2Int.up)
    {
        angle = 90f;
    }
    else if (direction == Vector2Int.left)
    {
        angle = 180f;
    }
    else if (direction == Vector2Int.down)
    {
        angle = 270f;
    }

    transform.rotation = Quaternion.Euler(0f, 0f, angle);
}

}
