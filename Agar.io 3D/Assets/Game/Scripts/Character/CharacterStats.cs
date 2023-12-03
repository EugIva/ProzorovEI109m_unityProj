using System;
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
        private set
        {
            mass = value;
            if(mass < 0)
            {
                mass = 0;
            }
        }
    }

    [Min(0), SerializeField] private float speed = 1;
    public float Speed { get => speed; private set { speed = value; } }

    [Min(0)] private int maxSpeed = 5;
    public int MaxSpeed { get => maxSpeed; private set { maxSpeed = value; } }

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
                SetMaxSpeed(5);
                characterMove.groundCheckDistance = 0.6f;
                break;

            case > 50 and <= 70:
                SetMaxSpeed(4);
                break;

            case > 70 and <= 100:
                SetMaxSpeed(4);
                characterMove.groundCheckDistance = 0.9f;
                break;

            default:
                SetMaxSpeed(0, true);
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

    /// <param name="baseValue">Set MaxSpeed to base value: 5</param>
    private void SetMaxSpeed(byte value, bool baseValue = false)
    {
        if (baseValue)
        {
            MaxSpeed = 6;
            return;
        }
        MaxSpeed = value;
    }
}
