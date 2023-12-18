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
            if(mass <= 8)
            {
                Destroy(gameObject);
            }
        }
    }

    [Min(0), SerializeField] private float speed = 1;
    public float Speed { get => speed; private set { speed = value; } }
    [Tooltip("Recomended base value = 0.02")]
    [Min(0), SerializeField] private float speedCoeficient = 0.02f;
    public float SpeedCoeficient { get => speedCoeficient; private set {  } }

    private float lastSpeed;
    private int speedCounter;

    [SerializeField] private EnemyMove enemyMove;

    private void Start() => lastSpeed = Speed;
    public void UpdateStats(float value, bool increaseMass = false)
    {
        if (increaseMass)
        {
            Mass += value;
            speed += value / 60;

            transform.localScale += new Vector3(value, value, value) / 100;
            enemyMove.FOV += value / 100;
            return;
        }
        //CancelAllBuffs();
        Mass -= value * 2;
        speed -= value / 30;

        transform.localScale -= new Vector3(value, value, value) / 50;
        enemyMove.FOV -= value / 50;
    }
    //private void CancelAllBuffs()
    //{
    //    lastSpeed = Speed;
    //    StopAllCoroutines();
    //    speedCounter = 0;
    //    Speed = lastSpeed;
    //}

    public void TemporaryChangeSpeed(byte seconds, ushort reward) => StartCoroutine(AddSpeedBuff2(seconds, reward));
    private IEnumerator AddSpeedBuff2(byte seconds, float reward)
    {
        Speed += reward / 12;
        while (seconds > 0)
        {
            if (!Timer.isPause)
            {
                yield return new WaitForSecondsRealtime(1);
                seconds--;
            }
            yield return new WaitForSecondsRealtime(0.3f);
        }
        Speed -= reward / 12;
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
        byte randomEffect = (byte)Random.Range(0, 1);
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
            Mass += character.Mass / 5;
            transform.localScale += character.transform.localScale / 5;
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
