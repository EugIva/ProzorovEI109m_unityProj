using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public static CharacterStats Instance;
    private CharacterStats() { }
    [Min(0), SerializeField] private int mass = 1;
    public int Mass { get => mass; private set { mass = value; } }
    [Min(0), SerializeField] private int speed = 1;
    public int Speed { get => speed; private set { speed = value; } }
    [Min(0), SerializeField] private float jumpForce = 1;
    public float JumpForce { get => jumpForce; private set { jumpForce = value; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
    public void AddMass(ushort value) => Mass += value;
    public void AddSpeed(ushort value) => Speed += value;
    public void AddJumpForce(ushort value) => JumpForce += value;
}
