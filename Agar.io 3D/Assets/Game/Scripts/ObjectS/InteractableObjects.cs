using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    private enum ObjectType
    {
        none,
        greed_cube,
        red_cube,
        yellow_cube,
    }
    [SerializeField] private ObjectType type;
    [Range(0, 2), SerializeField] private ushort reward;
    [Range(0, 60), SerializeField] private byte durationInSeconds;

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats character = CharacterStats.Instance;

        switch (type)
        {
            case ObjectType.greed_cube:
                character.IncreaseMass(reward);
                break;

            case ObjectType.red_cube:
                character.DecreaseMass(reward);
                break;

            case ObjectType.yellow_cube:
                RandomBuff();
                break;
        }
        Destroy(gameObject);
    }
    private void RandomBuff()
    {
        byte randomEffect = (byte)Random.Range(0, 2);
        switch (randomEffect)
        {
            case 0:
                CharacterStats.Instance.TemporaryChangeSpeed(durationInSeconds, reward);
                break;

            case 1:
                CharacterStats.Instance.TemporaryChangeJumpForce(durationInSeconds, reward);
                break;
        }
    }
    private void OnDestroy()
    {
        if(gameObject != null)
        {
            ObjectSpawner.Instance.ObjectsInMap--;
        }
    }
}
