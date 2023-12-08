using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private new Camera camera;
    [Min(0), SerializeField] private float mass = 1;
    private int speedCounter;
    private int jumpCounter;
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
    private float lastSpeed;

    [Min(0), SerializeField] private float jumpForce = 1;
    public float JumpForce { get => jumpForce; private set { jumpForce = value; } }
    private float lastJumpForce;


    [Tooltip("The lower the coefficient, the higher the speed")]
    [Min(0.01f), SerializeField] private float speedCoefficient = 0.2f;
    public float SpeedCoefficient { get => speedCoefficient; private set {} }
    private CharacterMove charachterMove;

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
        charachterMove = GetComponent<CharacterMove>();
    }
    private void Start()
    {
        lastJumpForce = JumpForce;
        lastSpeed = Speed;
    }
    public void UpdateStats(float value, bool increaseMass = false)
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
        CancelAllBuffs();
        Mass -= value * 2;
        lastSpeed = Speed;
        Speed += value / 500;
        charachterMove.groundCheckDistance -= value / 100;

        camera.fieldOfView -= value / 12.5f;

        transform.localScale -= new Vector3(value, value, value) / 50;
    }
    private void CancelAllBuffs()
    {
        StopAllCoroutines();
        jumpCounter = 0;
        speedCounter = 0;
        GameController.Instance.jumpBuffCount.text = "x0";
        GameController.Instance.speedBuffCount.text = "x0";
        Speed = lastSpeed;
        JumpForce = lastJumpForce;
    }
    public void TemporaryChangeSpeed(byte seconds, ushort reward) => StartCoroutine(AddSpeedBuff(seconds, reward));
    public void TemporaryChangeJumpForce(byte seconds, ushort reward) => StartCoroutine(AddJumpForceBuff(seconds, reward));

    private IEnumerator AddJumpForceBuff(byte seconds, float reward)
    {
        GameController.Instance.jumpBuffCount.text = $"x{++jumpCounter}";
        JumpForce += reward / 4;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
        }
        JumpForce -= reward / 4;
        GameController.Instance.jumpBuffCount.text = $"x{--jumpCounter}";
        yield break;
    }
    private IEnumerator AddSpeedBuff(byte seconds, float reward)
    {
        GameController.Instance.speedBuffCount.text = $"x{++speedCounter}";
        Speed += reward;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
        }
        Speed -= reward;
        GameController.Instance.speedBuffCount.text = $"x{--speedCounter}";
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