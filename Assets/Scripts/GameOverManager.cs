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

    private bool isGameOver = false;

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    private void Start()
    {
        // Hide the game over screen initially
        gameObject.SetActive(false);

        // Subscribe to the game over event
        Board.OnGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the game over event to avoid memory leaks
        Board.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        // Call the GameOver method when the game over event is triggered
        GameOver();
    }

    private void GameOver()
    {
        if (!isGameOver)
        {
            Time.timeScale = 0f;

            gameObject.SetActive(true);

            // Set UI

            isGameOver = true;
        }
    }

    public void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
