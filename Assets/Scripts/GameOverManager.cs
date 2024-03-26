using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalLevelText;
    public TextMeshProUGUI finalLinesCleardText;
    private ScoreManager scoreManager;
    public Board board;

    private bool isGameOver = false;

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    private void Awake()
    {
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager == null)
            {
                Debug.LogError("ScoreManager reference is not set in the GameOverManager script.");
            }
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            Time.timeScale = 0f;

            //Compented out because scoreManager keeps crashing during debugging
            //UpdateGameOverScore

            isGameOver = true;
        }
    }

    private void UpdateGameOverScore()
    {
        if (scoreManager != null)
        {
            finalScoreText.text = scoreManager.CurrentScore.ToString("D6");
            finalLevelText.text = scoreManager.Level.ToString();
            finalLinesCleardText.text = scoreManager.TotalLinesCleared.ToString();
        }
        else
        {
            Debug.LogError("ScoreManager reference is not set in the GameOverManager.");
        }

    }

    public void RestartGame()
    {
        board.ResetGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
