using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BallMovement : MonoBehaviour, ICollidable
{
   private float speed = 5f;
   private Vector2 direction;
   private Rigidbody2D rb;
   private SpriteRenderer sr;

   public float Speed
   {
      get { return speed; }
      set
      {
         if (value < 0)
            speed = 0;
         else
            speed = value;
      }
   }

   public Vector2 Direction
   {
      get { return direction; }
      set
      {
         direction = value.normalized;
      }
   }
   
   void Start()
   {
      rb = GetComponent<Rigidbody2D>();
      sr = GetComponent<SpriteRenderer>();
      Direction = new Vector2(1f, 1f);
      Speed = 5f;
      rb.linearVelocity = Vector2.zero; // stay still until StartBall() is called
   }

   void FixedUpdate()
   {
      if (rb == null) return;
      if (isActive)
      {
         rb.linearVelocity = Direction * Speed;
      }
      else
      {
         rb.linearVelocity = Vector2.zero;
      }
   }

   void OnCollisionEnter2D(Collision2D collision)
   {
      ICollidable collidable = 
         collision.gameObject.GetComponent<ICollidable>();

      if (collidable != null)
      {
         collidable.OnHit(collision);
      }

      OnHit(collision);
   }
   private bool isActive = false;

   public void ResetBall()
   {
      transform.position = new Vector3(-63f, 0f, 0f);
      Debug.Log($"[BallMovement] ResetBall position set to {transform.position}");
      rb = GetComponent<Rigidbody2D>();
      sr = GetComponent<SpriteRenderer>();
      Direction = new Vector2(1f, 1f);
      Speed = 5f;
      rb.linearVelocity = Vector2.zero;
      isActive = false;
      if (rb != null) rb.linearVelocity = Vector2.zero;
      if (sr != null) sr.enabled = true; // ensure sprite is visible
      Debug.Log($"[BallMovement] ResetBall called. activeSelf={gameObject.activeSelf}, srEnabled={sr?.enabled}, srColor={sr?.color}, pos={transform.position}, scale={transform.localScale}, isActive={isActive}");
   }

   public void StartBall()
   {
      isActive = true;
      if (rb != null) rb.linearVelocity = Direction * Speed;
      if (sr != null) sr.enabled = true;
      Debug.Log($"[BallMovement] StartBall called. activeSelf={gameObject.activeSelf}, srEnabled={sr?.enabled}, pos={transform.position}, velocity={rb?.linearVelocity}");
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
}