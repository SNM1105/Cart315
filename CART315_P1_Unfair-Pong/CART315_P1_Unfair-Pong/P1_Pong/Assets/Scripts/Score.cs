using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private static readonly string[] enemyScoreMessages =
    {
        "Your death is inevitable",
        "It's only a matter of time",
        "Struggling is useless",
        "There is no escape",
        "You cannot win"
    };

    public int scorePlayerOne;
    public int scorePlayerTwo;

    public TextMeshProUGUI scorePlayerOneText;
    public TextMeshProUGUI scorePlayerTwoText;

    private int lastScoringPlayer = -1;
    private int nextEnemyScoreMessage;
    
    public void IncreaseScore(int playerId)
    {
        switch (playerId)
        {
            case 0:
                scorePlayerOne++;
                break;
            case 1:
                scorePlayerTwo++;
                break;
        }

        lastScoringPlayer = playerId;
        
        UpdateScore();

        if (playerId == 1)
        {
            nextEnemyScoreMessage = (nextEnemyScoreMessage + 1) % enemyScoreMessages.Length;
        }
    }

    public void ResetScore()
    {
        scorePlayerOne = 0;
        scorePlayerTwo = 0;
        lastScoringPlayer = -1;
        nextEnemyScoreMessage = 0;
        UpdateScore();
    }

    // Update is called once per frame
    void UpdateScore()
    {
        scorePlayerOneText.text = lastScoringPlayer == 1
            ? enemyScoreMessages[nextEnemyScoreMessage]
            : scorePlayerOne.ToString();
        scorePlayerTwoText.text = scorePlayerTwo.ToString();
    }
}