using System;
using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public static CharacterStats Instance;
    private CharacterStats() { }
    [Min(0), SerializeField] private int mass = 1;
    public int Mass { get => mass; private set { mass = value; } }

    [Min(0), SerializeField] private int speed = 1;
    public int Speed { get => speed; private set { speed = value; } }

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

    public void IncreaseMass(ushort value)
    {
        Mass += value;
        transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
    }
    public void DecreaseMass(ushort value)
    {
        Mass -= value;
        transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
    }
    public void TemporaryChangeSpeed(ushort value) => StartCoroutine(AddSpeedBuff(5, value));
    public void TemporaryChangeJumpForce(ushort value) => StartCoroutine(AddJumpForceBuff(5, value));

    private IEnumerator AddJumpForceBuff(byte seconds, ushort value)
    {
        JumpForce += value;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
            Debug.Log("Time left: " + seconds);
        }
        JumpForce -= value;
        yield break;
    }
    private IEnumerator AddSpeedBuff(byte seconds, ushort value)
    {
        Speed += value;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
            Debug.Log("Time left: " + seconds);
        }
        Speed -= value;
        yield break;
    }

    /// <param name="baseValue">Set MaxSpeed to base value: 5</param>
    public void SetMaxSpeed(byte value, bool baseValue = false)
    {
        if (baseValue)
        {
            MaxSpeed = 5;
            return;
        }
        MaxSpeed = value;
    }
}
