using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int pointsToWin = 5;
    [SerializeField] private float serveSpeed = 5f;

    [Header("References")]
    [SerializeField] private BallMovement ball;

    private NetworkVariable<int> leftScore = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<int> rightScore = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<bool> gameOver = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<int> winner = new NetworkVariable<int>(
        -1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public int LeftScore => leftScore.Value;
    public int RightScore => rightScore.Value;
    public bool GameOver => gameOver.Value;
    public bool GameStarted => gameStarted.Value;
    public int Winner => winner.Value;

    public void StartGame()
    {
        if (!IsServer) return;

        leftScore.Value = 0;
        rightScore.Value = 0;
        gameOver.Value = false;
        winner.Value = -1;
        gameStarted.Value = true;

        if (ball == null) ball = FindObjectOfType<BallMovement>();

        float y = Random.Range(-0.5f, 0.5f);
        if (Mathf.Abs(y) < 0.2f) y = (y >= 0 ? 0.2f : -0.2f);

        Vector2 dir = new Vector2(1f, y).normalized;
        ball.ResetToCenterAndServe(dir, serveSpeed);
    }

    public void ScoreRightPoint()
    {
        if (!IsServer) return;
        if (!gameStarted.Value) return;
        if (gameOver.Value) return;

        rightScore.Value++;
        CheckWinCondition();

        if (!gameOver.Value)
        {
            ServeTowardRightScorer();
        }
    }

    public void ScoreLeftPoint()
    {
        if (!IsServer) return;
        if (!gameStarted.Value) return;
        if (gameOver.Value) return;

        leftScore.Value++;
        CheckWinCondition();

        if (!gameOver.Value)
        {
            ServeTowardLeftScorer();
        }
    }

    private void ServeTowardLeftScorer()
    {
        if (ball == null) ball = FindObjectOfType<BallMovement>();

        float y = Random.Range(-0.5f, 0.5f);
        if (Mathf.Abs(y) < 0.2f) y = (y >= 0 ? 0.2f : -0.2f);

        Vector2 dir = new Vector2(-1f, y).normalized; 
        ball.ResetToCenterAndServe(dir, serveSpeed);
    }

    private void ServeTowardRightScorer()
    {
        if (ball == null) ball = FindObjectOfType<BallMovement>();

        float y = Random.Range(-0.5f, 0.5f);
        if (Mathf.Abs(y) < 0.2f) y = (y >= 0 ? 0.2f : -0.2f);

        Vector2 dir = new Vector2(1f, y).normalized; 
        ball.ResetToCenterAndServe(dir, serveSpeed);
    }

    private void CheckWinCondition()
    {
        if (!IsServer) return;
        if (gameOver.Value) return;

        if (leftScore.Value >= pointsToWin)
        {
            gameOver.Value = true;
            winner.Value = 0;
            ball.StopBall();
        }
        else if (rightScore.Value >= pointsToWin)
        {
            gameOver.Value = true;
            winner.Value = 1;
            ball.StopBall();
        }
    }
}