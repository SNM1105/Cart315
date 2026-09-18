using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    public float speed = 10.0f;

    public Vector2 direction;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>(); //meow
        
    }

    //update is called once per frame

    private void Update()
    {
        direction = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            direction = Vector2.up;
        else if (Keyboard.current.sKey.isPressed)
            direction = Vector2.down;
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude == 0) return;

        _rigidBody.AddForce(direction * speed);

    }
}
