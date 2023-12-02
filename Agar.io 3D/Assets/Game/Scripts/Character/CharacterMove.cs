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
        //float X = Input.GetAxis("Horizontal");
        //float Y = Input.GetAxis("Vertical");

        //Vector3 moveInput = new(X, 0, Y);

        //Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveInput;

        //transform.Translate(characterStats.Speed * Time.deltaTime * moveDirection, Space.World);

        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(X, 0, Y);

        Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveInput;
        // ѕровер€ем текущую скорость персонажа
        float currentSpeed = rb.velocity.magnitude;

        // ≈сли текуща€ скорость меньше максимальной скорости, примен€ем силу
        if (currentSpeed < characterStats.MaxSpeed)
        {
            // »спользуем метод AddForce дл€ применени€ силы
            rb.AddForce(moveDirection * characterStats.Speed);
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
