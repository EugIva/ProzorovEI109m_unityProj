using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMove : MonoBehaviour
{
    private Rigidbody rb;
    [Min(1), SerializeField] private float speed;
    [Min(1), SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxRotationX = 80f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;
    private float currentRotationX = 0f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        HandleRotation();
        HandleMovement();
        Jump();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ¬ращаем персонаж в соответствии с движением мыши
        transform.Rotate(Vector3.up, mouseX * rotationSpeed);

        // –ассчитываем новый угол вращени€ камеры
        currentRotationX -= mouseY * rotationSpeed;
        currentRotationX = Mathf.Clamp(currentRotationX, -maxRotationX / 2, maxRotationX);

        // ѕримен€ем вращение камеры
        Camera.main.transform.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveInput = new(horizontalInput, 0, verticalInput);

        // ѕреобразуем вектор относительно направлени€ камеры
        Vector3 moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveInput;

        // ѕеремещаем персонаж
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
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
