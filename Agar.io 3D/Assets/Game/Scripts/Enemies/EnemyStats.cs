using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Min(0), SerializeField] private float mass;
    public float Mass { get => mass; private set { } }
}
