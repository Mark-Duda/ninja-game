using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public GameObject player;
    public GameObject Camera = GameObject.Find("Main Camera");
    public float Wspeed = 5f;
    public float Sspeed = 10f;
    private bool StartOfState; 
    private Rigidbody2D rb;
    public float jumpForce = 10f;
    public float raycastLength = 0.1f;
    private bool isGrounded;
   
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

    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        player = gameObject;
        SwitchState(States.idle);
        player.transform.SetParent(Camera.transform);
        isGrounded = IsGrounded();

    }
    bool IsGrounded()
    {
        // Cast a ray downwards to check if the player is grounded
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength);

        // If the raycast hits something with the "Ground" tag, the player is grounded
        return hit.collider != null && hit.collider.CompareTag("Ground");
    }
   
    
    // Update is called once per frame
    void Update()
    {
        xdir = Input.GetAxis("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
            SwitchState(States.jump);
        }

        if (xdir != 0&& !Input.GetKey(KeyCode.LeftShift))
        {
            SwitchState(States.walk);
        }
        if (xdir != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            SwitchState(States.sprint);
        }
        switch (state)
        {
            case States.idle:
                print("idle state");
                Idle();
                break;

            case States.walk:
                print("walk state");
                Walk();
                break;

            case States.sprint:
                print("sprint state");
                Sprint();
                break;

            case States.jump:
                print("jump state");
                break;

            case States.fall:
                print("fall state");
                break;

            case States.wallgrab:
                print("wallgrab state");
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
}
