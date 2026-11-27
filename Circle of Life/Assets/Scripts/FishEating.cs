
using UnityEngine;

[RequireComponent(typeof(FishSize))]
public class FishEating : MonoBehaviour
{
    public int score = 0;
    public float growthMultiplier = 0.15f;
    public float requiredSizeRatio = 1.1f;

    public GameManager gameManager;

    private FishSize mySize;
    private bool isDead = false;

    void Awake()
    {
        mySize = GetComponent<FishSize>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        FishSize otherFish = other.GetComponent<FishSize>();

        if (otherFish == null || otherFish == mySize)
            return;

        // Eat smaller fish
        if (mySize.size >= otherFish.size * requiredSizeRatio)
        {
            float growth = otherFish.size * growthMultiplier;

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            mySize.Grow(growth);
            score++;

            Debug.Log("Fish eaten! Score: " + score);
        }

        // Get eaten by bigger fish
        else if (otherFish.size >= mySize.size * requiredSizeRatio)
        {
            isDead = true;
            gameManager.GameOver(score);
            gameObject.SetActive(false);
        }
    }
}
