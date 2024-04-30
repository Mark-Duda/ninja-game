 using System;
 using System.Collections;
         using System.Collections.Generic;
         using System.Net;
          using UnityEditorInternal;
          using UnityEngine;
          using UnityEngine.Experimental.GlobalIllumination;

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
     public bool candash;

     enum States
     {
         idle,
         walk,
         sprint,
         jump,
         fall,
         wallgrab,
         slide,
         dash,
         crouch
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
         candash = true;


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

     IEnumerator dash()
     {
         candash = false;
         SwitchState(States.dash);
         float startTime = Time.time;
         while (Time.time < startTime + .2f)
         {
             transform.position += new Vector3(Sspeed * Time.deltaTime*playerCollider.transform.localScale.x*2, 0f, 0f);
             yield return null;
         }
         SwitchState(States.idle);
         yield return new WaitForSeconds(1f);
         candash = true;

     }
     


     // Update is called once per frames
     void Update()

     {
         if (Input.GetKeyDown(KeyCode.LeftShift)! & state != States.wallgrab)
         {
             SwitchState(States.crouch);
         }
         
         if (Input.GetKeyDown(KeyCode.Q)&&candash)
         {
             StartCoroutine(dash());
         }

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

         //if (!ismoving)
         {

             //SwitchState(States.idle); 
         }


         switch (state)
         {
             case States.idle:
                // print("idle state");
                 Idle();
                 break;

             case States.walk:
                // print("walk state");
                 break;

             case States.sprint:
                // print("sprint state");
                 Sprint();
                 break;

             case States.jump:
                 print("jump state");
                 Jump();
                 break;

             case States.fall:
                 print("fall state");
                 Fall();
                 break;

             case States.wallgrab:
              //   print("wallgrab state");
                 Wallgrab();
                 break;

             case States.slide:
                 print("slide state");
                 break;
             case States.dash:
                 print("dash state");
                 break;
             case States.crouch:
                 //print("crouch state");
                 crouch();
                 break;

         }
     }

     // Update is called once per frame
     void SwitchState(States newstate)
     {
         StartOfState = true;
         state = newstate;

     }

     void crouch()
     {
         Debug.Log("un");
         Vector3 newScale = playerCollider.transform.localScale;
         newScale.y = .6f;

// Assign the new scale to the GameObject
         playerCollider.transform.localScale = newScale;
         if (!Input.GetKey(KeyCode.LeftShift))
         {
             SwitchState(States.idle);
         }
     }

     void Idle()
     {
         Vector3 newScale = playerCollider.transform.localScale;
         newScale.y = 1f;

// Assign the new scale to the GameObject
         playerCollider.transform.localScale = newScale;
         xdir = Input.GetAxis("Horizontal");

         if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
         {
             Debug.Log("sprint");
             SwitchState(States.sprint);
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

         if (IsMovingDownward)
         {
             Debug.Log("fall");
             SwitchState(States.fall);
         }


         Vector2 rayStartl = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y - .001f);
         Vector2 rayStartr = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y - .001f);

         //Cast a ray downwards to check if the player is grounded
         RaycastHit2D hitr = Physics2D.Raycast(rayStartr, Vector2.down, raycastLength);
         RaycastHit2D hitl = Physics2D.Raycast(rayStartl, Vector2.down, raycastLength);

         // If the raycast hits something with the "Ground" tag, the player is grounded
         if (hitl.collider != null && hitl.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }

         if (hitr.collider != null && hitr.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }
         else
         {
             isGrounded = false;
         }

         if (Input.GetKeyDown(KeyCode.W) && isGrounded)
         {
             Debug.Log("jump");
             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
             SwitchState(States.jump);
         }




     }

     void Fall()
     {
         Vector3 newvScale = playerCollider.transform.localScale;
         newvScale.y = 1f;

// Assign the new scale to the GameObject
         playerCollider.transform.localScale = newvScale;
         rb.gravityScale = 1;
         Vector2 rayStartLeft =
             new Vector2(playerCollider.bounds.min.x - .001f, playerCollider.bounds.center.y - .001f);
         Vector2 rayStartRight =
             new Vector2(playerCollider.bounds.max.x + .001f, playerCollider.bounds.center.y - .001f);

         Debug.DrawLine(rayStartLeft, rayStartLeft + Vector2.left * raycastLength, Color.blue);
         Debug.DrawLine(rayStartRight, rayStartRight + Vector2.right * raycastLength, Color.red);
         // Cast rays horizontally to check if the player is grounded on each side
         RaycastHit2D hitLeft = Physics2D.Raycast(rayStartLeft, Vector2.left, raycastLength);
         RaycastHit2D hitRight = Physics2D.Raycast(rayStartRight, Vector2.right, raycastLength);
         if (isGrounded == false && hitLeft.collider != null && hitLeft.collider.CompareTag("Ground"))
         {

             SwitchState(States.wallgrab);
         }

         if (isGrounded == false && hitRight.collider != null && hitRight.collider.CompareTag("Ground"))
         {

             SwitchState(States.wallgrab);
         }

         Vector2 rayStartl = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y - .001f);
         Vector2 rayStartr = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y - .001f);

         //Cast a ray downwards to check if the player is grounded
         RaycastHit2D hitr = Physics2D.Raycast(rayStartr, Vector2.down, raycastLength);
         RaycastHit2D hitl = Physics2D.Raycast(rayStartl, Vector2.down, raycastLength);

         // If the raycast hits something with the "Ground" tag, the player is grounded
         if (hitl.collider != null && hitl.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }

         if (hitr.collider != null && hitr.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }
         else
         {
             isGrounded = false;
         }

         if (Input.GetKey(KeyCode.D))
         {
             transform.position += new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = 1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;


         }
         else if (Input.GetKey(KeyCode.A))
         {
             transform.position -= new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = -1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;
         }

         if (isGrounded)
         {
             SwitchState(States.idle);
         }


     }


     void Sprint()
     {
         Vector3 newvScale = playerCollider.transform.localScale;
         newvScale.y = 1f;

// Assign the new scale to the GameObject
         playerCollider.transform.localScale = newvScale;
         if (Input.GetKey(KeyCode.D))
         {
             transform.position += new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = 1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;

         }
         else if (Input.GetKey(KeyCode.A))
         {
             transform.position -= new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = -1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;
         }
         else
         {
             SwitchState(States.idle);
         }

         Vector2 rayStartl = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y - .001f);
         Vector2 rayStartr = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y - .001f);

         //Cast a ray downwards to check if the player is grounded
         RaycastHit2D hitr = Physics2D.Raycast(rayStartr, Vector2.down, raycastLength);
         RaycastHit2D hitl = Physics2D.Raycast(rayStartl, Vector2.down, raycastLength);

         // If the raycast hits something with the "Ground" tag, the player is grounded
         if (hitl.collider != null && hitl.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }

         if (hitr.collider != null && hitr.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }
         else
         {
             isGrounded = false;
         }

         if (Input.GetKeyDown(KeyCode.W) && isGrounded)
         {
             Debug.Log("jump");
             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
             SwitchState(States.jump);
         }

     }

     void Jump()
     {
         Vector3 newvScale = playerCollider.transform.localScale;
         newvScale.y = 1f;

// Assign the new scale to the GameObject
         playerCollider.transform.localScale = newvScale;
         rb.gravityScale = 1;

         Vector2 rayStartLeft =
             new Vector2(playerCollider.bounds.min.x - .001f, playerCollider.bounds.center.y - .001f);
         Vector2 rayStartRight =
             new Vector2(playerCollider.bounds.max.x + .001f, playerCollider.bounds.center.y - .001f);

         Debug.DrawLine(rayStartLeft, rayStartLeft + Vector2.left * raycastLength, Color.blue);
         Debug.DrawLine(rayStartRight, rayStartRight + Vector2.right * raycastLength, Color.red);
         // Cast rays horizontally to check if the player is grounded on each side
         RaycastHit2D hitLeft = Physics2D.Raycast(rayStartLeft, Vector2.left, raycastLength);
         RaycastHit2D hitRight = Physics2D.Raycast(rayStartRight, Vector2.right, raycastLength);
         if (isGrounded == false && hitLeft.collider != null && hitLeft.collider.CompareTag("Ground"))
         {

             SwitchState(States.wallgrab);
         }

         if (Input.GetKey(KeyCode.D))
         {
             transform.position += new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = 1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;

         }

         if (Input.GetKey(KeyCode.A))
         {
             transform.position -= new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = -1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;
         }

         float verticalVelocity = rb.velocity.y;

         // Set a threshold value to determine downward movement
         float downwardThreshold = -0.3f;
         if (verticalVelocity < downwardThreshold && verticalVelocity < 0f)
         {
             IsMovingDownward = true; // Player is moving downward
         }
         else
         {
             IsMovingDownward = false;
         }

         if (IsMovingDownward)
         {
             Debug.Log("fall");
             SwitchState(States.fall);
         }

         if (isGrounded == false && hitRight.collider != null && hitRight.collider.CompareTag("Ground"))
         {

             SwitchState(States.wallgrab);
         }

     }

     void Wallgrab()
     {Vector3 newvScale = playerCollider.transform.localScale;
         newvScale.y = 1f;

// Assign the new scale to the GameObject
         playerCollider.transform.localScale = newvScale;
         rb.gravityScale = .3f;
         Vector2 rayStartLeft =
             new Vector2(playerCollider.bounds.min.x - .001f, playerCollider.bounds.center.y - .001f);
         Vector2 rayStartRight =
             new Vector2(playerCollider.bounds.max.x + .001f, playerCollider.bounds.center.y - .001f);

         if (Input.GetKey(KeyCode.D))
         {
             transform.position += new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = 1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;

         }
         else if (Input.GetKey(KeyCode.A))
         {
             transform.position -= new Vector3(Sspeed * Time.deltaTime, 0f, 0f);
             Vector3 newScale = playerCollider.transform.localScale;
             newScale.x = -1;

// Assign the new scale to the GameObject
             playerCollider.transform.localScale = newScale;
         }
         Debug.DrawLine(rayStartLeft, rayStartLeft + Vector2.left * raycastLength, Color.blue);
         Debug.DrawLine(rayStartRight, rayStartRight + Vector2.right * raycastLength, Color.red);
         // Cast rays horizontally to check if the player is grounded on each side
         RaycastHit2D hitLeft = Physics2D.Raycast(rayStartLeft, Vector2.left, raycastLength);
         RaycastHit2D hitRight = Physics2D.Raycast(rayStartRight, Vector2.right, raycastLength);
         if (Input.GetKeyDown(KeyCode.W) && hitRight.collider != null && hitRight.collider.CompareTag("Ground"))
         {
             rb.velocity = new Vector2(rb.velocity.x - .3f * jumpForce, jumpForce * .7f);
             Debug.LogWarning("SUFFER");
             rb.gravityScale = 1f;
             SwitchState(States.jump);
         }

         if (Input.GetKeyDown(KeyCode.W) && hitLeft.collider != null && hitLeft.collider.CompareTag("Ground"))
         {
             rb.velocity = new Vector2(rb.velocity.x + .3f * jumpForce, jumpForce * .7f);
             Debug.Log("SUFFER");
             rb.gravityScale = 1f;
             SwitchState(States.jump);
         }

         if (hitRight.collider == null && hitLeft.collider == null)
         {
             SwitchState(States.fall);
         }
         

         Vector2 rayStartl = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y - .001f);
         Vector2 rayStartr = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.min.y - .001f);

         //Cast a ray downwards to check if the player is grounded
         RaycastHit2D hitr = Physics2D.Raycast(rayStartr, Vector2.down, raycastLength);
         RaycastHit2D hitl = Physics2D.Raycast(rayStartl, Vector2.down, raycastLength);

         // If the raycast hits something with the "Ground" tag, the player is grounded
         if (hitl.collider != null && hitl.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }

         if (hitr.collider != null && hitr.collider.CompareTag("Ground"))
         {
             isGrounded = true;
         }
         else
         {
             isGrounded = false;
         }

         if (isGrounded)
         {
             SwitchState(States.idle);
         }

     }
 }
 

 
