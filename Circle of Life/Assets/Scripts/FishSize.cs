
using UnityEngine;

public class FishSize : MonoBehaviour
{
    public float size = 0.6f;
    public float maxSize = 4f;

    void Start()
    {
        size = transform.localScale.x;
    }

    public void Grow(float amount)
    {
        size = Mathf.Min(size + amount, maxSize);

        transform.localScale = new Vector3(
            size, size, size * 2f
        );
    }
}
