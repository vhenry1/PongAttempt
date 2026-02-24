using Unity.Netcode;
using UnityEngine;

public class NetworkPaddleController : NetworkBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float leftX = -7f;
    [SerializeField] private float rightX = 8f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    private Rigidbody2D rb;

    private NetworkVariable<float> syncedXPosition = new NetworkVariable<float>();
    
    private NetworkVariable<float> syncedYPosition = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();

        if (IsServer)
        {
            // Calculate and set the X position once on the server
            float targetX = (OwnerClientId == 0) ? leftX : rightX;
            syncedXPosition.Value = targetX;
            
            // Apply immediately to the server's version
            transform.position = new Vector3(targetX, 0f, 0f);
        }
    }

    void FixedUpdate()
    {
        // Safety: If not spawned or RB is missing, stop to avoid NullReferenceException
        if (!IsSpawned || rb == null) return;

        if (IsOwner)
        {
            string axis = (OwnerClientId == 0) ? "LeftPaddle" : "RightPaddle";
            float input = Input.GetAxisRaw(axis);

            float newY = transform.position.y + (input * speed * Time.fixedDeltaTime);
            newY = Mathf.Clamp(newY, minY, maxY);

            // Use the syncedXPosition to ensure we stay at -69 or -56
            rb.MovePosition(new Vector2(syncedXPosition.Value, newY));
            syncedYPosition.Value = newY;
        }
        else
        {
            // Non-owners stay at the synced X and follow the synced Y
            rb.MovePosition(new Vector2(syncedXPosition.Value, syncedYPosition.Value));
        }
    }
}