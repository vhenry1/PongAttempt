using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BallMovement : MonoBehaviour, ICollidable
{
   private float speed = 5f;
   private Vector2 direction;
   private Rigidbody2D rb;

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
      Direction = new Vector2(1f, 1f);
      Speed = 5f;
      rb.linearVelocity = Direction * Speed;
   }

   void FixedUpdate()
   {
      rb.linearVelocity = Direction * Speed;
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