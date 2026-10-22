using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    public void Collect(Snake snake)
    {
        snake.ActivateShield();

        Debug.Log("Shield collected!");

        Destroy(gameObject);
    }
}