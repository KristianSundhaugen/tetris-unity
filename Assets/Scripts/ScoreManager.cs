using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI topScoreText;
    public TextMeshProUGUI levelText;

    private int currentScore;
    private int topScore;
    private int level;
    private int totalLinesCleared;
    private int linesRequiredToLevel;

    private const int SingleLineScore = 40;
    private const int DoubleLineScore = 100;
    private const int TripleLineScore = 300;
    private const int TetrisScore = 1200;

    private const int LinesToLevelBaseValue = 10;

    private const int MaxLevel = 15;

    private const string TopScoreKey = "TopScore";

    public int Level
    {
        get { return level; }
    }

    private void Awake()
    {
        topScore = PlayerPrefs.GetInt(TopScoreKey, 0);

        currentScore = 0;
        level = 0;
        UpdateUI(0);
    }

    public void UpdateUI(int linesCleared)
    {
        int score = RowToScoreCalculation(linesCleared);
        totalLinesCleared += linesCleared;
        currentScore += score;

        TopScoreHandling();
        CalculateLevel();

        scoreText.text = currentScore.ToString("D6");
        topScoreText.text = topScore.ToString("D6");
        levelText.text = level.ToString();
    }

    public void ResetUI()
    {
        currentScore = 0;
        level = 0;
        totalLinesCleared = 0;
        linesRequiredToLevel = LinesToLevelBaseValue;

        scoreText.text = currentScore.ToString("D6");
        topScoreText.text = topScore.ToString("D6");
        levelText.text = level.ToString();
    }

    private int RowToScoreCalculation(int linesCleared)
    {
        int score = 0;

        switch (linesCleared)
        {
            case 1:
                score = SingleLineScore * (level + 1);
                break;
            case 2:
                score = DoubleLineScore * (level + 1);
                break;
            case 3:
                score = TripleLineScore * (level + 1);
                break;
            case 4:
                score = TetrisScore * (level + 1);
                break;
            default:
                break;
        }

        return score;
    }

    private void TopScoreHandling()
    {
        if (this.currentScore > this.topScore)
        {
            this.topScore = this.currentScore;
            // Save the top score to PlayerPrefs
            PlayerPrefs.SetInt(TopScoreKey, this.topScore);
            PlayerPrefs.Save(); // Ensure changes are saved immediately
        }
    }

    private void CalculateLevel()
    {
        if (linesRequiredToLevel <= totalLinesCleared && this.level < MaxLevel)
        {
            this.level += 1;
            linesRequiredToLevel = LinesToLevelBaseValue * this.level;
        }
    }
}
