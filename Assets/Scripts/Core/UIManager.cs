using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loadingPanel;
    public GameObject startPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject endPanel;

    [Header("Loading UI")]
    public Slider loadingBar;
    public TextMeshProUGUI loadingPercentText;
    public TextMeshProUGUI loadingStatusText;

    [Header("HUD Text")]
    public TextMeshProUGUI ringsText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI objectiveText;

    [Header("Pause Panel UI (Optional)")]
    public TextMeshProUGUI pauseTitleText;
    public TextMeshProUGUI pauseRestartButtonText;
    public TextMeshProUGUI pauseSwitchModeButtonText;

    [Header("End Panel Text")]
    public TextMeshProUGUI endTitleText;
    public TextMeshProUGUI endStatsText;

    [Header("Free Flight Buttons (Optional)")]
    public GameObject startFreeFlightButton;
    public GameObject pauseFreeFlightButton;
    public GameObject endFreeFlightButton;

    public IEnumerator PlayBootSequence(Action onComplete)
    {
        if (loadingPanel != null) loadingPanel.SetActive(true);
        startPanel.SetActive(false);
        hudPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        endPanel.SetActive(false);

        string[] statuses = {
            "BOOTING FLIGHT CONTROLLER...",
            "CALIBRATING ROTORS...",
            "LOADING FLIGHT SYSTEMS...",
            "TELEMETRY ONLINE"
        };

        float duration = 1.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            if (loadingBar != null) loadingBar.value = progress;
            if (loadingPercentText != null) loadingPercentText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
            if (loadingStatusText != null)
            {
                int idx = Mathf.Min(Mathf.FloorToInt(progress * statuses.Length), statuses.Length - 1);
                loadingStatusText.text = statuses[idx];
            }
            yield return null;
        }

        if (loadingPanel != null) loadingPanel.SetActive(false);
        onComplete?.Invoke();
    }

    public void ShowStartPanel(bool freeFlightUnlocked = false)
    {
        if (loadingPanel != null) loadingPanel.SetActive(false);
        startPanel.SetActive(true);
        hudPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        endPanel.SetActive(false);

        if (startFreeFlightButton != null)
            startFreeFlightButton.SetActive(freeFlightUnlocked);
    }

    public void ShowHUD()
    {
        if (loadingPanel != null) loadingPanel.SetActive(false);
        startPanel.SetActive(false);
        hudPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public void SetPausePanel(bool isPaused, bool isFreeFlight = false, bool freeFlightUnlocked = false)
    {
        if (pausePanel == null) return;
        pausePanel.SetActive(isPaused);
        if (!isPaused) return;

        if (pauseTitleText != null)
            pauseTitleText.text = isFreeFlight ? "FREE FLIGHT PAUSED" : "TUTORIAL PAUSED";

        if (pauseRestartButtonText != null)
            pauseRestartButtonText.text = isFreeFlight ? "RESET DRONE" : "RESTART";

        if (pauseSwitchModeButtonText != null)
            pauseSwitchModeButtonText.text = isFreeFlight ? "PLAY TUTORIAL" : "FREE FLIGHT";

        if (pauseFreeFlightButton != null)
            pauseFreeFlightButton.SetActive(isFreeFlight || freeFlightUnlocked);
    }

    public void ShowEndPanel(bool won, int rings, int totalRings, float time, float bestTime)
    {
        if (loadingPanel != null) loadingPanel.SetActive(false);
        startPanel.SetActive(false);
        hudPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        endPanel.SetActive(true);

        endTitleText.text = won ? "TRAINING COMPLETE" : "DRONE CRASHED";
        endTitleText.color = won ? Color.green : Color.red;

        if (endFreeFlightButton != null)
            endFreeFlightButton.SetActive(won);

        string bestStr = bestTime < 9000f ? FormatTime(bestTime) : "--:--.-";
        endStatsText.text = $"Gates Cleared: {rings}/{totalRings}\nFlight Time: {FormatTime(time)}\nBest Time: {bestStr}";
    }

    public void UpdateHUD(int rings, int totalRings, float time, bool isFreeFlight = false)
    {
        if (ringsText != null)
            ringsText.text = isFreeFlight ? "FREE FLIGHT" : $"GATES: {rings}/{totalRings}";

        if (timerText != null)
        {
            timerText.gameObject.SetActive(!isFreeFlight);
            timerText.text = $"TIME: {FormatTime(time)}";
        }
    }

    public void SetObjectiveText(string message)
    {
        if (objectiveText != null) objectiveText.text = message;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        float seconds = time % 60f;
        return $"{minutes:00}:{seconds:00.0}";
    }
}