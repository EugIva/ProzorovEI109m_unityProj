using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private void Start()
    {
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 0;
    }
    public void SetUnlimitedMode(Button[] buttons, int clicketButtonIndex)
    {
        GameController.Instance.VisualizeSelectedButton(buttons, clicketButtonIndex, new Vector3(1.05f, 1.05f, 1), Vector3.one, buttons[2], buttons[3]);
        gamemodeType = GamemodeType.unlimited;
    }
    public void SetBattleRoyaleMode(Button[] buttons, int clicketButtonIndex)
    {
        GameController.Instance.VisualizeSelectedButton(buttons, clicketButtonIndex, new Vector3(1.05f, 1.05f, 1), Vector3.one, buttons[2], buttons[3]);
        gamemodeType = GamemodeType.battleRoyale;
    }

    public void StartGame()
    {
        if (gamemodeType != GamemodeType.none && location != Location.menu)
        {
            SceneManager.LoadScene((int)location);
        }
    }
}
