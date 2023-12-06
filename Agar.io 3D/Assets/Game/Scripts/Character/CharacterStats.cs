using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private new Camera camera;
    [Min(0), SerializeField] private float mass = 1;

    public float Mass
    {
        get => mass;
        set
        {
            mass = value;
            if(mass <= 0)
            {
                if (GameController.Instance.isUnlimited)
                {
                    GameController.Instance.RespawnCharacter();
                    return;
                }
                else if (GameController.Instance.isBattleRoyale)
                {
                    GameController.Instance.CharacterDead();
                    Destroy(gameObject);
                }
            }
        }
    }

    [Min(0), SerializeField] private float speed = 1;
    public float Speed { get => speed; private set { speed = value; } }

    [Min(0), SerializeField] private float jumpForce = 1;
    public float JumpForce { get => jumpForce; private set { jumpForce = value; } }


    [Tooltip("The lower the coefficient, the higher the speed")]
    [Min(0.01f), SerializeField] private float speedCoefficient = 0.2f;
    public float SpeedCoefficient { get => speedCoefficient; private set {} }
    private CharacterMove charachterMove;

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
        charachterMove = GetComponent<CharacterMove>();
    }
    public void UpdateStats(float value , bool increaseMass = false)
    {
        if (increaseMass)
        {
            Mass += value;
            Speed -= value / 1000;
            charachterMove.groundCheckDistance += value / 200;

            camera.fieldOfView += value / 25;

            transform.localScale += new Vector3(value, value, value) / 100;
            return;
        }
        Mass -= value * 2;
        Speed += value / 500;
        charachterMove.groundCheckDistance -= value / 100;

        camera.fieldOfView -= value / 12.5f;

        transform.localScale -= new Vector3(value, value, value) / 50;
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