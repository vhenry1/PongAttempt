using Unity.Netcode;
using UnityEngine;

public class Score : NetworkBehaviour
{
  void OnTriggerEnter2D(Collider2D other)
  {
    
    if (!other.CompareTag("Ball")) {
      Debug.Log("[ScoreZone] Ignored: entering object is not tagged 'Ball'.");
      return;
    }
    var ball = Object.FindFirstObjectByType<BallMovement>();

    GameManager manager = Object.FindFirstObjectByType<GameManager>();
    if (manager == null)
    {
      Debug.LogError("[ScoreZone] GameManager not found when scoring.");
      return;
    }

    if (this.CompareTag("RightScoreZone"))
    {
      Debug.Log("[ScoreZone] Scoring for RIGHT zone");
      manager.CompleteRightScore();
      ball.ResetBall();
      ball.StartBall();
    }
    else if (this.CompareTag("LeftScoreZone"))
    {
      Debug.Log("[ScoreZone] Scoring for LEFT zone");
      manager.CompleteLeftScore();
      ball.ResetBall();
      ball.StartBall();
    }
    else
    {
      Debug.LogWarning($"[ScoreZone] This trigger's tag is not 'RightScoreZone' or 'LeftScoreZone': {this.tag}");
    }
  }
}