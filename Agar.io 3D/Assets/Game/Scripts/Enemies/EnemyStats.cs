using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [Min(0), SerializeField] private float mass;
    public float Mass
    {
        get => mass;
        private set
        {
            mass = value;
            if(mass <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    [Min(0), SerializeField] private float speed = 1;
    public float Speed { get => speed; private set { speed = value; } }

    public void IncreaseMass(float value)
    {
        Mass += value;
        transform.localScale += new Vector3(value, value, value) / 100;
    }
    public void DecreaseMass(float value)
    {
        Mass -= value * 2;
        transform.localScale -= new Vector3(value, value, value) / 50;
    }
    public void ChangeSpeed(float value, bool baseValue = false)
    {
        if (baseValue)
        {
            Speed = 1;
            return;
        }
        Speed *= value;
    }
    public void TemporaryChangeSpeed(byte seconds, ushort reward) => StartCoroutine(AddSpeedBuff2(seconds, reward));

    private IEnumerator AddSpeedBuff2(byte seconds, float reward)
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
    private void GiveMassFor(EnemyStats bot)
    {
        bot.Mass += Mass / 5;
        bot.transform.localScale += transform.localScale / 5;
    }
    private void GiveMassFor(CharacterStats character)
    {
        character.Mass += Mass / 5;
        character.transform.localScale += transform.localScale / 5;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out EnemyStats bot))
        {
            if (Mass < bot.Mass)
            {
                GiveMassFor(bot);
                Destroy(gameObject);
                return;
            }
            Mass += bot.Mass / 5;
            transform.localScale += bot.transform.localScale / 5;
            Destroy(bot.gameObject);
        }
        else if (other.gameObject.TryGetComponent(out CharacterStats character))
        {
            if (Mass < character.Mass)
            {
                GiveMassFor(character);
                Destroy(gameObject);
                return;
            }
            //Respawn logic
        }
    }
}
