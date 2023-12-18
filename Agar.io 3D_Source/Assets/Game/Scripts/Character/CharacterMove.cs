using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CharacterStats))]
public class CharacterMove : MonoBehaviour
{
    private Rigidbody rb;
    private CharacterStats characterStats;
    private new Transform camera;
    [Min(0), SerializeField] private float lerpFactor = 0.04f;
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
    private void FixedUpdate()
    {
        HandleMovement();
        Jump();
    }
    void HandleMovement()
    {
        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        Vector3 moveInput = new(X, 0, Y);
        moveInput.Normalize();

        Vector3 moveDirection = Quaternion.Euler(0, camera.eulerAngles.y, 0) * moveInput;

        moveDirection = Vector3.Scale(moveDirection, transform.localScale);

        if (moveInput.magnitude > 0.1f)
        {
            Vector3 targetVelocity = moveDirection * characterStats.Speed / (characterStats.Mass * characterStats.SpeedCoefficient);
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, lerpFactor);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, lerpFactor);
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0f, characterStats.JumpForce, 0f, ForceMode.Impulse);
        }
    }
    private bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.yellow);
    }
#endif
}
