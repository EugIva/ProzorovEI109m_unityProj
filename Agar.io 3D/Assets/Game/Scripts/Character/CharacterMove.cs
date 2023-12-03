using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CharacterStats))]
public class CharacterMove : MonoBehaviour
{
    private Rigidbody rb;
    private CharacterStats characterStats;
    private new Transform camera;

    [Header("Other Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] internal float groundCheckDistance = 0.1f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterStats = GetComponent<CharacterStats>();
        camera = FindObjectOfType<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        Jump();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        //None physic
        //float X = Input.GetAxis("Horizontal");
        //float Y = Input.GetAxis("Vertical");

        //Vector3 moveInput = new(X, 0, Y);

        //Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveInput;

        //transform.Translate(characterStats.Speed * Time.deltaTime * moveDirection, Space.World);

        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        Vector3 moveInput = new(X, 0, Y);

        Vector3 moveDirection = Quaternion.Euler(0, camera.eulerAngles.y, 0) * moveInput;

        moveDirection = Vector3.Scale(moveDirection, transform.localScale);

        float currentSpeed = rb.velocity.magnitude;

        float lerpFactor = 0.1f;
        if (moveInput.magnitude > 0.1f)
        {
            if(currentSpeed < characterStats.MaxSpeed)
            {
                rb.AddForce(moveDirection * characterStats.Speed);
            }
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, lerpFactor);
        }
    }
    private void Jump()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) )
        {
            rb.AddForce(0f, characterStats.JumpForce, 0f, ForceMode.Impulse);
        }
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
    }
}
