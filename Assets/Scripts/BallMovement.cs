using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour, ICollidable
{
    [SerializeField] private float speed = 5f;

    private Vector2 direction;
    private Rigidbody2D rb;

    public float Speed
    {
        get { return speed; }
        set { speed = (value < 0) ? 0 : value; }
    }

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value.normalized; }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        Speed = 0f;
        Direction = Vector2.right;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    void FixedUpdate()
    {
        if (!IsServer) return;
        rb.linearVelocity = Direction * Speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        ICollidable collidable = collision.gameObject.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.OnHit(collision);
        }

        OnHit(collision);
    }

    public void OnHit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            Direction = new Vector2(-Direction.x, Direction.y);
        }
        else if (collision.gameObject.CompareTag("wall"))
        {
            Direction = new Vector2(Direction.x, -Direction.y);
        }
    }

    public void ResetToCenterAndServe(Vector2 serveDir, float serveSpeed)
    {
        if (!IsServer) return;

        rb.position = new Vector2(0f, 0f);
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Direction = serveDir;
        Speed = serveSpeed;

        rb.linearVelocity = Direction * Speed;
    }

    public void StopBall()
    {
        if (!IsServer) return;

        Speed = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}