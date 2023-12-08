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

        Quaternion newRotation = Quaternion.Euler(currentRotationX, transform.eulerAngles.y + mouseX, 0);
        transform.rotation = newRotation;

        RaycastHit hit;
        Vector3 raycastOrigin = target.position + Vector3.up * 0.5f;
        if (Physics.Raycast(raycastOrigin, -transform.forward, out hit, 5.0f))
        {
            transform.position = hit.point + transform.forward * 0.1f; 
        }
        else
        {
            // If no collision, set the position based on the original logic
            transform.position = target.position - transform.forward * 5.0f;
        }
    }
    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
    }
}

