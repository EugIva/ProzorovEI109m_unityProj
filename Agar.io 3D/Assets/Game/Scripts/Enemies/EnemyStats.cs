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

    [SerializeField] private EnemyMove enemyMove;

    public void UpdateStats(float value, bool increaseMass = false)
    {
        if (increaseMass)
        {
            Mass += value;
            speed -= value / 1000;

            transform.localScale += new Vector3(value, value, value) / 100;
            enemyMove.FOV += value / 500;
            return;
        }
        Mass -= value * 2;
        speed += value / 500;

        transform.localScale -= new Vector3(value, value, value) / 50;
        enemyMove.FOV -= value / 250;
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
        //bot.Mass += Mass / 5;
        //bot.transform.localScale += transform.localScale / 5;
        bot.UpdateStats(Mass / 5, true);
    }
    private void GiveMassFor(CharacterStats character)
    {
        character.UpdateStats(Mass / 5, true);
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
                //Other buffs
                break;
        }
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
            if (GameController.Instance.isUnlimited)
            {
                GameController.Instance.RespawnCharacter();
            }
            else if (GameController.Instance.isBattleRoyale)
            {
                GameController.Instance.CharacterDead();
            }
        }
    }

    private void OnDisable() => EnemySpawner.Instance.EnemiesInMap--;
}
