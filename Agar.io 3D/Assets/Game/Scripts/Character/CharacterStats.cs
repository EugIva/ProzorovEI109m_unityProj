using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public static CharacterStats Instance;
    private CharacterStats() { }
    [SerializeField] private new Camera camera;
    [Min(0), SerializeField] private float mass = 1;
    public float Mass
    {
        get => mass;
        set
        {
            mass = value;
            if(mass <= 0)
            {
                //Respawn logic
            }
        }
    }

    [Min(0), SerializeField] private float speed = 1;
    public float Speed { get => speed; private set { speed = value; } }

    //[Min(0)] private int maxRigidbodySpeed = 20;
    //public int MaxRigidbodySpeed { get => maxRigidbodySpeed; private set { maxRigidbodySpeed = value; } }

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

    public void IncreaseMass(float value)
    {
        Mass += value;
        transform.localScale += new Vector3(value, value, value) / 100;
        camera.fieldOfView += value / 25;
        MassCheck();
    }
    public void DecreaseMass(float value)
    {
        Mass -= value;
        transform.localScale -= new Vector3(value, value, value) / 50;
        camera.fieldOfView -= value / 25;
        MassCheck();
    }
    private void MassCheck()
    {
        var characterMove = GetComponent<CharacterMove>();
        switch (Mass)
        {
            case >= 30 and <= 50:
                characterMove.groundCheckDistance = 0.6f;
                break;

            case > 50 and <= 70:
                break;

            case > 70 and <= 100:
                characterMove.groundCheckDistance = 0.9f;
                break;

            default:
                characterMove.groundCheckDistance = 0.4f;
                break;
        }
    }
    public void TemporaryChangeSpeed(byte seconds, ushort reward) => StartCoroutine(AddSpeedBuff(seconds, reward));
    public void TemporaryChangeJumpForce(byte seconds, ushort reward) => StartCoroutine(AddJumpForceBuff(seconds, reward));

    private IEnumerator AddJumpForceBuff(byte seconds, float reward)
    {
        JumpForce += reward / 2;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
        }
        JumpForce -= reward / 2;
        yield break;
    }
    private IEnumerator AddSpeedBuff(byte seconds, float reward)
    {
        Speed += reward / 3;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
        }
        Speed -= reward / 3;
        yield break;
    }
    public void RandomBuff(byte effectDuration, ushort reward)
    {
        byte randomEffect = (byte)Random.Range(0, 2);
        switch (randomEffect)
        {
            case 0:
                TemporaryChangeSpeed(effectDuration, reward);
                break;

            case 1:
                TemporaryChangeJumpForce(effectDuration, reward);
                break;
        }
    }
}
