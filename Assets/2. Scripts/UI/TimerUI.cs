using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class TimerUI : MonoBehaviour, ITimerObserver
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        gameTimer.ActionOnTimer += OnUpdateTimer;
    }

    private void OnDestroy()
    {
        gameTimer.ActionOnTimer -= OnUpdateTimer;
    }

    public void OnUpdateTimer(float timer)
    {
        timerText.text = $"{Mathf.Round(timer)}seconds";
    }
}
