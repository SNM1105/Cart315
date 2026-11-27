
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;

    void Start()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
    }

    public void GameOver(int score)
    {
        finalScoreText.text = "Fish eaten: " + score;

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
