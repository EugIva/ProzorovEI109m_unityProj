using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementnew : MonoBehaviour
{
    [HideInInspector]
    public CharacterController mCharacterController;

    public float mWalkSpeed = 1.5f;
    public float mRunSpeed = 3.0f;
    public float mRotationSpeed = 50.0f;


    public Rigidbody rb;
    public float runSpeed = 500f;
    public float strafeSpeed = 500f;
    public float jumpForce = 15f;
    protected bool strafeleft = false;
    protected bool strafeRight = false;
    protected bool strafeForward = false;
    protected bool strafeBack = false;
    protected bool doJump = false;


    [Tooltip("Only useful with Follow and Independent " +
      "Rotation - third - person camera control")]
    public bool mFollowCameraForward = false;
    public float mTurnRate = 200.0f;
    private Vector3 mVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        mCharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        
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

    public void Move()
    {


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



       // float h = Input.GetAxis("Horizontal");
       // float v = Input.GetAxis("Vertical");

        float speed = mWalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = mRunSpeed;
        }

        if (mFollowCameraForward)
        {
            // Only allow aligning of player's direction 
            // when there is a movement.
            
            
                // rotate player towards the camera forward.
                Vector3 eu = Camera.main.transform.rotation.eulerAngles;
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.Euler(0.0f, eu.y, 0.0f),
                    mTurnRate * Time.deltaTime);
            
        }
        else
        {
          //  transform.Rotate(0.0f, h * mRotationSpeed * Time.deltaTime, 0.0f);
        }
        //transform.Rotate(0.0f, h * mRotationSpeed * Time.deltaTime, 0.0f);
      //  mCharacterController.Move(transform.forward * v * speed * Time.deltaTime);

    }
}