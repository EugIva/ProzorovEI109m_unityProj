using UnityEngine;

public class GravitationController : MonoBehaviour
{
    internal const float baseGravity = -9.81f;
    [Min(-10), SerializeField] private float customGravity = -9.81f;

    void Start() => Physics.gravity = new Vector3(0, customGravity, 0);
}
