using UnityEngine;

public abstract class PaddleController : MonoBehaviour, ICollidable
{
    [SerializeField] protected float speed = 8f;
    protected Rigidbody2D rb;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    
    void FixedUpdate()
    {
        float input = GetMovementInput();
        rb.linearVelocity = new Vector2(0f, input * speed);
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