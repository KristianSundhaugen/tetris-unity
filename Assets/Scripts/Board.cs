using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public ScoreManager scoreManager { get; private set; }
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;
    public GameOverManager gameOverManager;
    private bool isGameOver = false;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.scoreManager = GetComponentInChildren<ScoreManager>();
        if (gameOverManager == null)
        {
            gameOverManager = FindObjectOfType<GameOverManager>();
            if (gameOverManager == null)
            {
                Debug.LogError("GameOverManager not found in the scene.");
            }
        }
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;

        for (int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    private void Start()
    {
        // You might want to set up or initialize your ScoreManager here
        if (this.scoreManager == null)
        {
            Debug.LogError("ScoreManager reference is not set in the Board script.");
        }
        else
        {
            this.scoreManager.ResetUI();
        }

        SpawnPiece();

    }

    public void SpawnPiece()
    {
        if (isGameOver == false)
        {
            int random = Random.Range(0, this.tetrominos.Length);
            TetrominoData data = this.tetrominos[random];

            this.activePiece.Initialize(this, this.spawnPosition, data);

            if (IsValidPosition(this.activePiece, this.spawnPosition))
            {
                Set(activePiece);
            }
            else
            {
                TriggerGameOver();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Freeze time to pause gameplay
        pauseMenuUI.SetActive(true); // Show the pause menu UI
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume normal time to resume gameplay
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        gameOverMenuUI.SetActive(true);
        gameOverManager.GameOver();
    }

    public void ResetGame()
    {
        this.scoreManager.ResetUI();
        this.tilemap.ClearAllTiles();
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGameOver = false;
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }


    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            //Checks if a tile position would be out of bounds 
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            //Checks if the tilemap already has a tile that occupies that space
            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public int ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int countLinesCleard = 0;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                countLinesCleard++;
            }
            else
            {
                row++;
            }
        }
        return countLinesCleard;
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }


    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}