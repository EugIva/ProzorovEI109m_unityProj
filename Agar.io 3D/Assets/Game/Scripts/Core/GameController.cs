using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private GameController() { }

    private new CameraFollow camera;
    [SerializeField] private TMP_Text finalMassText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Transform character;
    [SerializeField] private GameObject characterPrefab;

    [SerializeField] private Vector3 characterSpawnPosition;

    public TMP_Text speedBuffCount, jumpBuffCount;
    public Button[] buttons;
    public Button[] locationButtons;
    public bool isUnlimited;
    public bool isBattleRoyale;
    public bool characterDead;
    private void Awake()
    {
        camera = FindObjectOfType<CameraFollow>();
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Time.timeScale = 2;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            buttons[0].onClick.AddListener(GameManager.Instance.SetUnlimitedMode);
            buttons[1].onClick.AddListener(GameManager.Instance.SetBattleRoyaleMode);
            buttons[2].onClick.AddListener(GameManager.Instance.StartGame);
            buttons[3].onClick.AddListener(CloseGame);
            locationButtons[0].onClick.AddListener(() => SetLocation(1));
            locationButtons[1].onClick.AddListener(() => SetLocation(2));
            locationButtons[2].onClick.AddListener(() => SetLocation(3));
            return;
        }
        switch (GameManager.Instance.gamemodeType)
        {
            case GameManager.GamemodeType.unlimited:
                Debug.Log("Unlimited mode");
                isUnlimited = true;
                break;

            case GameManager.GamemodeType.battleRoyale:
                Debug.Log("battleRoyale mode");
                isBattleRoyale = true;
                break;
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0 && Input.GetKeyDown(KeyCode.Escape) && !characterDead)
        {
            Pause();
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Time.timeScale = 3;
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Time.timeScale = 2;
        }
#endif
    }
    public void Pause()
    {
        if (!pausePanel.activeInHierarchy)
        {
            Time.timeScale = 0;
            Timer.isPause = true;
            pausePanel.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
            return;
        }
        Time.timeScale = 2;
        Timer.isPause = false;
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void CloseGame() => Application.Quit();
    public void CharacterDead()
    {
        var characterMass = character.GetComponent<CharacterStats>().Mass;
        Destroy(character.gameObject);
        characterDead = true;
        pausePanel.SetActive(true);
        finalMassText.gameObject.SetActive(true);
        finalMassText.text = $"Финальная масса: {characterMass}";
        continueButton.interactable = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }
    public void SetLocation(int index)
    {
        GameManager.Instance.location = (GameManager.Location)index;
    }
    public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
    public void RespawnCharacter()
    {
        character = FindObjectOfType<CharacterStats>().transform;
        Destroy(character.gameObject);
        var player = Instantiate(characterPrefab, characterSpawnPosition, Quaternion.identity);
        camera.SetNewTarget(player.transform);
    }
}
