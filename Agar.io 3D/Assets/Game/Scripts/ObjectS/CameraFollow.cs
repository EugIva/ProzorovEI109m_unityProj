using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Tooltip("Recomended base value = 1")]
    [SerializeField] private float rotationSpeed = 1.0f;
    [Tooltip("Recomended base value = 45")]
    [SerializeField] private float maxLookUpAngle = 45.0f;
    private float currentRotationX = 0f;

    void Update() => Move();
    private void Move()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        target.Rotate(0, mouseX, 0);

        currentRotationX -= mouseY;
        currentRotationX = Mathf.Clamp(currentRotationX, -maxLookUpAngle / 100, maxLookUpAngle);
        transform.rotation = Quaternion.Euler(currentRotationX, transform.eulerAngles.y + mouseX, 0);

        transform.position = target.position - transform.forward * 5.0f;
    }
}

