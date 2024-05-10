using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 2f;
    public bool facingRight = true;
    public Vector2 groundCheck;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.01f;
    public Vector2 wallCheck;
    public float wallCheckDistance = 0.01f;
    public Collider2D collider;
    private Rigidbody2D rb;
    public bool isBlocked;
    public bool isGrounded;
    void Start()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
   
        if (!facingRight)
        {
            groundCheck = new Vector2(collider.bounds.min.x, collider.bounds.min.y);   
            isGrounded = Physics2D.Raycast(groundCheck, Vector2.down, groundCheckDistance,~LayerMask.NameToLayer("Player") );
            Debug.DrawLine(groundCheck,groundCheck + Vector2.down*groundCheckDistance ,Color.red);
           wallCheck = new Vector2(collider.bounds.min.x-.2f, collider.bounds.center.y);
            isBlocked = Physics2D.Raycast(wallCheck, Vector2.right, wallCheckDistance,~LayerMask.NameToLayer("Player"));
            Debug.DrawLine(wallCheck, wallCheck + Vector2.right*wallCheckDistance,Color.red, wallCheckDistance);
        }
        else
        {
          
            groundCheck = new Vector2(collider.bounds.max.x, collider.bounds.min.y);   
            isGrounded = Physics2D.Raycast(groundCheck, Vector2.down, groundCheckDistance,~LayerMask.NameToLayer("Player"));
            Debug.DrawLine(groundCheck,groundCheck + Vector2.down*groundCheckDistance ,Color.red);
            wallCheck = new Vector2(collider.bounds.max.x+.2f, collider.bounds.center.y);
            isBlocked = Physics2D.Raycast(wallCheck, Vector2.left, wallCheckDistance,~LayerMask.NameToLayer("Player"));
           Debug.DrawLine(wallCheck, wallCheck + Vector2.left*wallCheckDistance,Color.red, wallCheckDistance);
        }
        // If there is no ground or there is a wall in front, turn around
        if (!isGrounded || isBlocked)
        {
            Flip();
        }

        // Move the enemy in the current direction
        rb.velocity = new Vector2(facingRight ? movementSpeed : -movementSpeed, rb.velocity.y);
    }

    void Flip()
    {
        isBlocked = false;
        // Reverse the direction the enemy is facing
        facingRight = !facingRight;

        // Rotate the enemy to face the new direction
        transform.Rotate(0f, 180f, 0f);
    }
}
    



