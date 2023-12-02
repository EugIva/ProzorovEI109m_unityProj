using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    private enum objectType
    {
        none,
        greed_cube,
        red_cube,
        yellow_cube,
    }
    [SerializeField] private objectType type;
    [Min(0), SerializeField] private ushort reward;

    private void OnTriggerEnter(Collider other)
    {
        CharacterStats character = CharacterStats.Instance;
        if(other.TryGetComponent(out InteractableObjects _object))
        {
            Destroy(gameObject);
            print("LOL");
            return;
        }
        switch (type)
        {
            case objectType.greed_cube:
                character.IncreaseMass(reward);
                break;

            case objectType.red_cube:
                character.DecreaseMass(reward);
                break;

            case objectType.yellow_cube:
                int randomEffect = Random.Range(0, 2);
                if(randomEffect == 1)
                {
                    character.TemporaryChangeSpeed(reward);
                }
                else
                {
                    character.TemporaryChangeJumpForce(reward);
                }
                break;
        }
        Destroy(gameObject);
    }
}
