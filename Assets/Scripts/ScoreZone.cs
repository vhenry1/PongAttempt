using Unity.Netcode;
using UnityEngine;

public class ScoreZone : NetworkBehaviour
{
    public enum ZoneType { LeftZone, RightZone }
    [SerializeField] private ZoneType zoneType;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;
        if (!other.CompareTag("Ball")) return;

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene!");
            return;
        }

        if (gameManager.GameOver) return;

        if (zoneType == ZoneType.LeftZone)
            gameManager.ScoreRightPoint();
        else
            gameManager.ScoreLeftPoint();
    }
}