using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Timer timerObject;
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
            timerObject.ExecuteTimer();
        }
    }
}
