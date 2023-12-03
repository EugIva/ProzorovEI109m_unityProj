using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GamemodeType
    {
        none,
        unlimited,
        oneLife,
    }
    [SerializeField] private GamemodeType gamemodeType;
    private void Start()
    {
        if(gamemodeType == GamemodeType.unlimited)
        {
            GetComponent<Timer>().ExecuteTimer();
        }
    }
}
