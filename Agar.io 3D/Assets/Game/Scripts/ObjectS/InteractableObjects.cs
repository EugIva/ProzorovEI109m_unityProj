using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    private enum ObjectType
    {
        none,
        green_cube,
        red_cube,
        yellow_cube,
    }
    [SerializeField] private ObjectType type;
    [Range(0, 2), SerializeField] private ushort reward;
    [Range(0, 60), SerializeField] private byte durationInSeconds;

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats character = CharacterStats.Instance;
        other.TryGetComponent(out EnemyStats enemy);
        if(enemy != null)
        {
            EnemyAdded(enemy);
            return;
        }
        CharacterAdded(character);
    }
    private void EnemyAdded(EnemyStats enemy)
    {
        switch (type)
        {
            case ObjectType.green_cube:
                enemy.IncreaseMass(reward);
                break;

            case ObjectType.red_cube:
                enemy.DecreaseMass(reward);
                break;

            case ObjectType.yellow_cube:
                RandomBuff(enemy);
                break;
        }
        Destroy(gameObject);
    }
    private void CharacterAdded(CharacterStats character)
    {
        switch (type)
        {
            case ObjectType.green_cube:
                character.IncreaseMass(reward);
                break;

            case ObjectType.red_cube:
                character.DecreaseMass(reward);
                break;

            case ObjectType.yellow_cube:
                RandomBuff(character);
                break;
        }
        Destroy(gameObject);
    }
    private void RandomBuff<T>(T type)
    {
        switch (type)
        {
            case EnemyStats enemy:
                enemy.TemporaryChangeSpeed(durationInSeconds, reward);
                break;

            case CharacterStats character:
                byte randomEffect = (byte)Random.Range(0, 2);
                switch (randomEffect)
                {
                    case 0:
                        character.TemporaryChangeSpeed(durationInSeconds, reward);
                        break;

                    case 1:
                        character.TemporaryChangeJumpForce(durationInSeconds, reward);
                        break;
                }
                break;
        }
    }
    private void OnDestroy() => ObjectSpawner.Instance.ObjectsInMap--;
}
