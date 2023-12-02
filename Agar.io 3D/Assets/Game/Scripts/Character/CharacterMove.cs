using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CharacterStats))]
public class CharacterMove : MonoBehaviour
{
    private Rigidbody rb;
    private CharacterStats characterStats;

    [Header("Other Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;
    private float currentRotationX = 0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterStats = GetComponent<CharacterStats>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        HandleMovement();
        Jump();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveInput = new(horizontalInput, 0, verticalInput);

        Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveInput;

        transform.Translate(characterStats.Speed * Time.deltaTime * moveDirection, Space.World);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
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
