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

    private void Start()
    {
        Board.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        Board.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        GameOver();
    }

    private void GameOver()
    {
        if (!isGameOver)
        {
            Time.timeScale = 0f;

            gameObject.SetActive(true);

            isGameOver = true;
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
