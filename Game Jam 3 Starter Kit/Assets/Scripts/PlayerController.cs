 using System.Collections;
using System.Collections.Generic;
using System.Net;
 using UnityEditorInternal;
 using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public GameObject player;
    public float Wspeed = 5f;
    public float Sspeed = 10f;
    public bool StartOfState; 
    public Rigidbody2D rb;
    public float jumpForce = 10f;
    public float raycastLength = .1f;
    public bool isGrounded;
    public Animator m_Animator;
    public Vector3 previousPosition;
   public bool ismoving;
    enum States
    {
        idle,
        walk,
        sprint,
        jump,
        fall,
        wallgrab,
        slide
    }

    States state;

    float xdir;
    public Collider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delayforfun());
        previousPosition = playerCollider.bounds.center;


        player = GameObject.FindWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        SwitchState(States.idle);
        
        playerCollider = player.GetComponent<Collider2D>();


    }

    public bool IsMovingDownward;
    
    IEnumerator delayforfun()
    {
        yield return new WaitForSeconds(.2f);
        Debug.Log("pain");
        previousPosition = playerCollider.bounds.center;
        player = GameObject.FindWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        SwitchState(States.idle);
        
        playerCollider = player.GetComponent<Collider2D>();
    }
   
    
    // Update is called once per frames
    void Update()
    {
        Vector3 currentPosition = playerCollider.bounds.center;
        if (currentPosition != previousPosition)
        {
            
            ismoving = true;
            
        }
        else
        {
            ismoving = false;
        }
        previousPosition = currentPosition;
        if (!ismoving&& isGrounded)
        {
            Debug.Log("idle");
            SwitchState(States.idle); 
        }
            
        if (IsMovingDownward)
        {
            Debug.Log("fall");
            SwitchState(States.fall);
        }
        
    
        // Get the vertical velocity of the player's Rigidbody2D component
        float verticalVelocity = rb.velocity.y;

        // Set a threshold value to determine downward movement
        float downwardThreshold = -0.3f; // Adjust this value as needed

        // Check if the vertical velocity is greater than the downward threshold
        if (verticalVelocity < downwardThreshold && verticalVelocity < 0f)
        {
            IsMovingDownward = true; // Player is moving downward
        }
        else
        {
            IsMovingDownward = false;
        }
        
        Vector2 rayStartLeft = new Vector2(playerCollider.bounds.min.x-.001f, playerCollider.bounds.center.y-.001f);
        Vector2 rayStartRight = new Vector2(playerCollider.bounds.max.x+.001f, playerCollider.bounds.center.y-.001f);

        Debug.DrawLine(rayStartLeft, rayStartLeft + Vector2.left * raycastLength, Color.blue);
        Debug.DrawLine(rayStartRight, rayStartRight + Vector2.right * raycastLength, Color.red);
        // Cast rays horizontally to check if the player is grounded on each side
        RaycastHit2D hitLeft = Physics2D.Raycast(rayStartLeft, Vector2.left, raycastLength);
        RaycastHit2D hitRight = Physics2D.Raycast(rayStartRight, Vector2.right, raycastLength);
        Vector2 rayStartl = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y-.001f);
        Vector2 rayStartr = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y-.001f );
        
        //Cast a ray downwards to check if the player is grounded
        RaycastHit2D hitr = Physics2D.Raycast(rayStartr, Vector2.down, raycastLength);
        RaycastHit2D hitl = Physics2D.Raycast(rayStartl, Vector2.down, raycastLength);

            // If the raycast hits something with the "Ground" tag, the player is grounded
       if (hitl.collider != null && hitl.collider.CompareTag("Ground"))
        {
           isGrounded = true;
        }
       if(hitr.collider != null && hitr.collider.CompareTag("Ground"))
       {
           isGrounded = true;
       }
       else
       {
           isGrounded = false;
       }

       if (isGrounded == false && hitLeft.collider != null && hitLeft.collider.CompareTag("Ground"))
       {
           
           SwitchState(States.wallgrab);
       }
       if (isGrounded == false && hitRight.collider != null && hitRight.collider.CompareTag("Ground"))
       {
           
           SwitchState(States.wallgrab);
       }
      
       xdir = Input.GetAxis("Horizontal"); 
        
       if (Input.GetKeyDown(KeyCode.W) && isGrounded)
       {
           Debug.Log("jump");
           Jump();
           SwitchState(States.jump);
       }

       if (xdir != 0&& !Input.GetKey(KeyCode.LeftShift))
       {
           SwitchState(States.walk);
       }
       if (xdir != 0 && Input.GetKey(KeyCode.LeftShift))
       {
           Debug.Log("sprint");
           SwitchState(States.sprint);
       }
       switch (state)
       {
           case States.idle:
               //print("idle state");
               Idle();
               break;

           case States.walk:
               //print("walk state");
               Walk();
               break;

           case States.sprint:
               print("sprint state");
               Sprint();
               break;

           case States.jump:
               //print("jump state");
               break;

           case States.fall:
               print("fall state");
               break;

           case States.wallgrab:
               print("wallgrab state");
               Wallgrab();
               break;

           case States.slide:
               print("slide state");
               break;

       }
    }

    // Update is called once per frame
    void SwitchState(States newstate)
    {
        StartOfState = true;
        state = newstate;

    }

    void Idle()
    {
        //idle code goes here
        


        //one of the conditions for exiting idle state
        if (xdir != 0)
        {
            SwitchState(States.walk);
        }
    }

    void Walk()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(Wspeed * Time.deltaTime, 0f, 0f);
            
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(Wspeed * Time.deltaTime, 0f, 0f);
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
            
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
        }
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void Wallgrab()
    {
        rb.gravityScale = .3f;
        if (Input.GetKeyDown(KeyCode.W)&&Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = new Vector2(rb.velocity.x + .3f * jumpForce, jumpForce * .7f);
        }
        if (Input.GetKeyDown(KeyCode.W)&&Input.GetKeyDown(KeyCode.D))
        {
            rb.velocity = new Vector2(rb.velocity.x -.3f * jumpForce, jumpForce * .7f);
        }
    }
    
}
