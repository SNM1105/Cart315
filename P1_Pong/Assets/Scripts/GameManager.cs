using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int scoreP1 = 0;
    public int scoreP2 = 0;

    //player id = 0 -> left player, 1 -> right player
    public void IncreaseScore(int playerId)
    {
        switch (playerId)
        {
            case 0:
                scoreP1++;
                break;
            case 1:
                scoreP2++;
                break;
        }
    }
}

