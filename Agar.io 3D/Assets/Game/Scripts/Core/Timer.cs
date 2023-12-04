using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    [Tooltip("time in seconds")]
    [SerializeField] private int initialTime = 60;
    private bool colorChanged;

    public void ExecuteTimer() => StartCoroutine(StartTimer(initialTime));
    IEnumerator StartTimer(int seconds)
    {
        while (seconds > 0)
        {
            DisplayTime(seconds);
            yield return new WaitForSeconds(1);
            seconds--;
        }

        DisplayTime(0);
        Debug.Log("Время вышло!");
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
