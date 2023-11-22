using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float runSpeed = 500f;
    public float strafeSpeed = 500f;
    public float jumpForce = 15f;
    protected bool strafeleft = false;
    protected bool strafeRight = false;
    protected bool strafeForward = false;
    protected bool strafeBack = false;
    protected bool doJump = false;



    void Update()
    {
        if (Input.GetKey("a"))
        {
            strafeleft = true;
        }
        else
        {
            strafeleft = false;
        }
        if (Input.GetKey("d"))
        {
            strafeRight = true;

        }
        else
        {
            strafeRight = false;
        }

        if (Input.GetKey("w"))
        {
            strafeForward = true;

        }
        else
        {
            strafeForward = false;
        }

        if (Input.GetKey("s"))
        {
            strafeBack = true;

        }
        else
        {
            strafeBack = false;
        }

        if (Input.GetKeyDown("space"))
        {
            doJump = true;
        }

        if (transform.position.y < -5f)
        {
            Debug.Log(" Конец игры ! ");
        }


    }


    void FixedUpdate()
    {
        // rb.AddForce ( 0 , 0 , runSpeed * Time.deltaTime ) ;
        if (strafeForward)
        {
            rb.AddForce(0, 0, strafeSpeed * Time.deltaTime, ForceMode.VelocityChange);
            //rb.MovePosition(transform.position + Vector3.forward * runSpeed * Time.deltaTime);
        }
        if (strafeBack)
        {
            rb.AddForce(0, 0, -strafeSpeed * Time.deltaTime, ForceMode.VelocityChange);
            //rb.MovePosition(transform.position + Vector3.forward * runSpeed * Time.deltaTime);
        }
        if (strafeleft)
        {
            rb.AddForce(-strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (strafeRight)
        {
            rb.AddForce(strafeSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (doJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doJump = false;
        }
    }
}
