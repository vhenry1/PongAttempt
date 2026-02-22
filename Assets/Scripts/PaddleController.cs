using UnityEngine;
using Unity.Netcode;

public abstract class PaddleController : NetworkBehaviour, ICollidable
{
    [SerializeField] protected float speed = 8f;
    protected Rigidbody2D rb;

    private SpriteRenderer sr;
    private Color originalColor;
    public NetworkVariable<float> NetworkedYPosition = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Owner
    );

    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }
    public override void OnNetworkSpawn()
{
    if (IsOwner)
    {
        float xPosition = (OwnerClientId == 0) ? -7f : 7f;
        transform.position = new Vector3(xPosition, 0, 0);
    }
}

    
    void Update()
    {
        if (HasLocalControl())
        {
            float input = GetMovementInput();
            rb.linearVelocity = new Vector2(0f, input * speed);
            // Only write the NetworkVariable if Netcode is active and this NetworkObject is spawned.
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            {
                if (NetworkObject != null && NetworkObject.IsSpawned)
                {
                    NetworkedYPosition.Value = transform.position.y;
                }
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, NetworkedYPosition.Value, transform.position.z);
        }
        
    }

    protected bool HasLocalControl()
    {
        // If Netcode isn't running, allow local control for single-player/testing.
        if (NetworkManager.Singleton == null) return true;
        // If Netcode is running but not listening (not started), allow local control.
        if (!NetworkManager.Singleton.IsListening) return true;
        // Otherwise require ownership.
        return IsOwner;
    }

   protected abstract float GetMovementInput();

   public void OnHit(Collision2D collision)
    {
        if (collision.otherCollider != null && collision.otherCollider.CompareTag("Ball"))
        {
            if (sr != null)
            {
                sr.color = Color.pink;
                CancelInvoke(nameof(ResetColor));
                Invoke(nameof(ResetColor), 0.1f);
            }
        }
    }

    private void ResetColor()
    {
        if (sr != null) sr.color = originalColor;
    }
}