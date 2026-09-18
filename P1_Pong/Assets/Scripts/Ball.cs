using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    public float speed = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Start()
    {
        float x = 0.0f;
        float y = 0.0f;

        if (Random.value < 0.5f) x = -1.0f;
        else x = 1.0f;

        if (Random.value < 0.5f) y = -1.0f;
        else y = 1.0f;

        y = y * Random.Range(0.5f, 0.9f); //Picks a random number from the range of the 2 numbers i put.

        Vector2 direction = new Vector2(x, y);

        _rigidBody.AddForce(direction*speed); //This adds a force to the ball
    }
}
