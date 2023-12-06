using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager() { }
    public enum GamemodeType
    {
        none,
        unlimited,
        battleRoyale,
    }
    public enum Location : byte
    {
        menu,
        first,
        second,
        third,
    }
    public GamemodeType gamemodeType;
    public Location location;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetUnlimitedMode() => gamemodeType = GamemodeType.unlimited;
    public void SetBattleRoyaleMode() => gamemodeType = GamemodeType.battleRoyale;

    public void StartGame()
    {
        if (gamemodeType != GamemodeType.none && location != Location.menu)
        {
            SceneManager.LoadScene((int)location);
        }
    }
}
