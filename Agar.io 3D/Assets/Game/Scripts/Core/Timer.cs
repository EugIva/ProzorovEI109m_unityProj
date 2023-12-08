using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [Tooltip("time in seconds")]
    [Min(1),SerializeField] private int initialTime = 60;
    private float characterMass;
    private bool colorChanged;

    public static bool isPause = false;

    private void Start()
    {
        ExecuteTimer();
    }
    public void ExecuteTimer() => StartCoroutine(StartTimer(initialTime));
    IEnumerator StartTimer(int seconds)
    {
        while (seconds > 0)
        {
            yield return new WaitUntil(() => !isPause);
            DisplayTime(seconds);
            yield return new WaitForSecondsRealtime(1);
            seconds--;
        }

        DisplayTime(0);
        characterMass = FindObjectOfType<CharacterStats>().Mass;
        GameController.Instance.CharacterDead();
    }
    void DisplayTime(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
        if (seconds < 15 && !colorChanged)
        {
            colorChanged = true;
            timerText.color = Color.red;
        }
    }
}
