using Unity.Netcode;
using TMPro; 
using UnityEngine;

public class GameManager:NetworkBehaviour
{
    private NetworkVariable<int> rightScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> leftScore = new NetworkVariable<int>(0);

    public int RightScore => rightScore.Value;
    public int LeftScore => leftScore.Value;

    private NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);
    public TextMeshProUGUI uiTextElement; 

    TextUpdater textUpdater;
    public void StartGame()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening && !IsServer) return;

        Debug.Log($"[GameManager] textUpdater reference at StartGame: {(textUpdater == null ? "NULL" : textUpdater.ToString())}");


        leftScore.Value = 0;
        rightScore.Value = 0;
        
        gameOver.Value = false;

        var ball = Object.FindFirstObjectByType<BallMovement>();
        if (ball != null)
        {
            ball.ResetBall();
            ball.StartBall();
        }
        else
        {
            var allBalls = Object.FindObjectsOfType<BallMovement>();
            for (int i = 0; i < allBalls.Length; i++)
            {
                var bgo = allBalls[i].gameObject;
            }
            var go = GameObject.Find("Ball");
            if (go != null)
            {
                var bm = go.GetComponent<BallMovement>();
                if (bm != null)
                {
                    bm.ResetBall();
                    bm.StartBall();
                }
                else Debug.LogWarning("[GameManager] GameObject 'Ball' found but has no BallMovement component.");
            }
            else
            {
                Debug.LogError("[GameManager] Could not find any BallMovement instance.");
            }
        }

        var startBtn = GameObject.Find("StartButton");
        if (startBtn != null)
        {
            startBtn.SetActive(false);
            Debug.Log("[GameManager] StartButton hidden");
        }
        else
        {
            Debug.LogWarning("[GameManager] StartButton GameObject not found in scene (expected name 'StartButton').");
        }

        Debug.Log("Game Started by Host");
    }

    public void CompleteRightScore()
    {
        if (IsServer)
        {
            rightScore.Value++;
            if (textUpdater != null)
            {
                textUpdater.UpdateText(rightScore.Value + " - " + leftScore.Value);
            }
            else
            {
                Debug.LogError("[GameManager] textUpdater reference not set in Inspector!");
            }
            var ball = Object.FindFirstObjectByType<BallMovement>();
            if (ball != null)
            {
                ball.ResetBall();
                ball.StartBall();
            }
            else
            {
                Debug.LogError("BallMovement instance not found when resetting ball.");
            }
            Debug.Log("Right score is " + rightScore.Value);
        }
    }

    public void CompleteLeftScore()
    {
        if (IsServer)
        {
            leftScore.Value++;
            if (textUpdater != null)
            {
                textUpdater.UpdateText(rightScore.Value + " - " + leftScore.Value);
            }
            else
            {
                Debug.LogError("[GameManager] textUpdater reference not set in Inspector!");
            }
            var ball = Object.FindFirstObjectByType<BallMovement>();
            if (ball != null)
            {
                ball.ResetBall();
                ball.StartBall();
            }
            else
            {
                Debug.LogError("BallMovement instance not found when resetting ball.");
            }
            Debug.Log("Left score is " + leftScore.Value);
        }
    }

    public void CheckGameOver()
    {
        if (rightScore.Value >= 3 || leftScore.Value >= 3)
        {
            gameOver.Value = true;
            Debug.Log("Game Over!");
        }
    }
}