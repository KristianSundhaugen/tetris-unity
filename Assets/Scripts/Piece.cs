using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;
    private float lastMoveTime;
    public float moveInterval = 0.1f;


    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        this.board.Clear(this);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.board.TogglePause();
        }

        this.lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            MoveContinuous(Vector2Int.left);
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            MoveContinuous(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            MoveContinuous(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if (Time.time >= this.stepTime)
        {
            Step();
        }

        this.board.Set(this);
    }

    private void Step()
    {
        ScoreManager scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        if (scoreManager == null)
        {
            Debug.LogError("Missing Score Manager");
        }
        else
        {
            UpdateStepDelay(scoreManager.Level);
        }


        this.stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    public void UpdateStepDelay(int level)
    {
        switch (level)
        {
            case 0:
                stepDelay = 1.0f;
                break;
            case 1:
                stepDelay = 0.95f;
                break;
            case 2:
                stepDelay = 0.90f;
                break;
            case 3:
                stepDelay = 0.85f;
                break;
            case 4:
                stepDelay = 0.75f;
                break;
            case 5:
                stepDelay = 0.65f;
                break;
            case 6:
                stepDelay = 0.45f;
                break;
            case 7:
                stepDelay = 0.30f;
                break;
            case 8:
                stepDelay = 0.20f;
                break;
            case 9:
                stepDelay = 0.15f;
                break;
            case 10:
                stepDelay = 0.12f;
                break;
            case 11:
                stepDelay = 0.10f;
                break;
            case 12:
                stepDelay = 0.08f;
                break;
            case 13:
                stepDelay = 0.05f;
                break;
            case 14:
                stepDelay = 0.03f;
                break;
            default:
                stepDelay = 0.01f;
                break;
        }
    }

    private void Lock()
    {
        int linesCleared;
        this.board.Set(this);
        linesCleared = this.board.ClearLines();
        if (linesCleared > 0)
        {
            this.board.scoreManager.UpdateUI(linesCleared);
        }
        this.board.SpawnPiece();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }

        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0;
        }

        return valid;
    }

    private void MoveContinuous(Vector2Int translation)
    {
        // Continuously move while the key is held down
        if (Time.time - lastMoveTime >= moveInterval)
        {
            if (Move(translation))
            {
                lastMoveTime = Time.time;
            }
        }
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];
            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
