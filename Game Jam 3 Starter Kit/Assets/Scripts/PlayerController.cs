using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        state = States.idle;

    }

    // Update is called once per frame
    void Update()
    {
        xdir = Input.GetAxis("Horizontal");
    }

    // Update is called once per frame
    void SwitchState(States state)
    {
        switch (state)
        {
            case States.idle:
                print("idle state");
                Idle();
                break;

            case States.walk:
                print("walk state");
                break;

            case States.sprint:
                print("sprint state");
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

    void Idle()
    {
        //idle code goes here



        //one of the conditions for exiting idle state
        if (xdir != 0)
        {
            SwitchState(States.walk);
        }
    }


}
