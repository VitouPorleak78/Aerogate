using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Loading, Start, Playing, Paused, GameOver, Win }
    public GameState CurrentState { get; private set; } = GameState.Loading;

    [Header("Course Settings")]
    public string courseTitle = "TRAINING COURSE 01";
    public int totalRings = 8;
    public int ringsCollected { get; private set; } = 0;
    public float elapsedTime { get; private set; } = 0f;
    public bool isFreeFlight { get; private set; } = false;

    public bool IsFreeFlightUnlocked => PlayerPrefs.GetInt("FreeFlightUnlocked", 0) == 1 || GetBestTime() < 9000f;

    [Header("Tutorial Prompts (Per Ring)")]
    [TextArea]
    public string[] tutorialSteps = new string[]
    {
        "STEP 1: HOLD [SPACE] TO LIFT OFF & [W] TO FLY FORWARD",
        "STEP 2: USE [A / D] TO BANK & [Q / E] TO STEER",
        "STEP 3: MAINTAIN THROTTLE THROUGH THE CANYON",
        "STEP 4: CLIMB HIGH — APPROACH THE UPPER LOOP",
        "STEP 5: PITCH DOWN GENTLY THROUGH THE DESCENT",
        "FINAL STEP: ALL GATES CLEARED — LAND ON THE HELIPAD"
    };

    [Header("Audio (Optional)")]
    public AudioSource sfxSource;
    public AudioClip ringSFX;
    public AudioClip crashSFX;
    public AudioClip winSFX;

    [Header("References")]
    public UIManager uiManager;
    public GameObject tutorialRingsParent; // Optional: Drag parent folder of rings here to hide them in Free Flight

    private static bool hasBooted = false;
    private static bool launchDirectlyIntoFreeFlight = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0f;

        GameObject[] rings = GameObject.FindGameObjectsWithTag("Ring");
        if (rings.Length > 0) totalRings = rings.Length;

        if (uiManager != null)
        {
            uiManager.UpdateHUD(ringsCollected, totalRings, elapsedTime, isFreeFlight);

            if (launchDirectlyIntoFreeFlight)
            {
                launchDirectlyIntoFreeFlight = false;
                EnterFreeFlightImmediate();
                return;
            }

            if (!hasBooted && uiManager.loadingPanel != null)
            {
                hasBooted = true;
                StartCoroutine(uiManager.PlayBootSequence(() => {
                    CurrentState = GameState.Start;
                    uiManager.ShowStartPanel(IsFreeFlightUnlocked);
                }));
            }
            else
            {
                CurrentState = GameState.Start;
                uiManager.ShowStartPanel(IsFreeFlightUnlocked);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && CurrentState != GameState.Start && CurrentState != GameState.Loading)
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && (CurrentState == GameState.Playing || CurrentState == GameState.Paused))
        {
            TogglePause();
        }

        if (CurrentState == GameState.Playing)
        {
            elapsedTime += Time.deltaTime;
            if (uiManager != null)
                uiManager.UpdateHUD(ringsCollected, totalRings, elapsedTime, isFreeFlight);
        }
    }

    public void StartGame()
    {
        isFreeFlight = false;
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        if (tutorialRingsParent != null)
            tutorialRingsParent.SetActive(true);

        if (uiManager != null)
        {
            uiManager.ShowHUD();
            uiManager.UpdateHUD(ringsCollected, totalRings, elapsedTime, false);
            UpdateTutorialPrompt();
        }
    }

    public void StartFreeFlight()
    {
        if (CurrentState == GameState.Start)
        {
            EnterFreeFlightImmediate();
        }
        else
        {
            launchDirectlyIntoFreeFlight = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void EnterFreeFlightImmediate()
    {
        isFreeFlight = true;
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;

        if (tutorialRingsParent != null)
            tutorialRingsParent.SetActive(false);

        if (uiManager != null)
        {
            uiManager.ShowHUD();
            uiManager.UpdateHUD(ringsCollected, totalRings, elapsedTime, true);
            uiManager.SetObjectiveText("FREE FLIGHT // ALL AIRSPACE UNLOCKED");
        }
    }

    public void SwitchModeFromPause()
    {
        if (isFreeFlight)
            RestartTutorial();
        else
            StartFreeFlight();
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f;
            if (uiManager != null) uiManager.SetPausePanel(true, isFreeFlight, IsFreeFlightUnlocked);
        }
        else if (CurrentState == GameState.Paused)
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
            if (uiManager != null) uiManager.SetPausePanel(false, isFreeFlight, IsFreeFlightUnlocked);
        }
    }

    public void AddRing()
    {
        if (CurrentState != GameState.Playing || isFreeFlight) return;

        ringsCollected++;
        if (sfxSource != null && ringSFX != null) sfxSource.PlayOneShot(ringSFX);

        if (uiManager != null)
        {
            uiManager.UpdateHUD(ringsCollected, totalRings, elapsedTime, false);
            UpdateTutorialPrompt();
        }
    }

    private void UpdateTutorialPrompt()
    {
        if (uiManager == null || tutorialSteps.Length == 0) return;

        if (ringsCollected >= totalRings)
        {
            uiManager.SetObjectiveText(tutorialSteps[tutorialSteps.Length - 1]);
        }
        else
        {
            int index = Mathf.Clamp(ringsCollected, 0, tutorialSteps.Length - 2);
            uiManager.SetObjectiveText(tutorialSteps[index]);
        }
    }

    public void TriggerGameOver()
    {
        if (CurrentState != GameState.Playing || isFreeFlight) return;

        CurrentState = GameState.GameOver;
        Time.timeScale = 0f;
        if (sfxSource != null && crashSFX != null) sfxSource.PlayOneShot(crashSFX);

        if (uiManager != null)
            uiManager.ShowEndPanel(false, ringsCollected, totalRings, elapsedTime, GetBestTime());
    }

    public void TriggerWin()
    {
        if (CurrentState != GameState.Playing || isFreeFlight) return;

        if (ringsCollected < totalRings)
        {
            if (uiManager != null)
                uiManager.SetObjectiveText($"CLEAR ALL GATES FIRST ({ringsCollected}/{totalRings})");
            return;
        }

        CurrentState = GameState.Win;
        Time.timeScale = 0f;
        if (sfxSource != null && winSFX != null) sfxSource.PlayOneShot(winSFX);

        PlayerPrefs.SetInt("FreeFlightUnlocked", 1);
        PlayerPrefs.Save();

        float best = SaveAndGetBestTime(elapsedTime);
        if (uiManager != null)
            uiManager.ShowEndPanel(true, ringsCollected, totalRings, elapsedTime, best);
    }

    private float SaveAndGetBestTime(float newTime)
    {
        float best = PlayerPrefs.GetFloat("BestTime_Tutorial", 9999f);
        if (newTime < best)
        {
            best = newTime;
            PlayerPrefs.SetFloat("BestTime_Tutorial", best);
            PlayerPrefs.Save();
        }
        return best;
    }

    private float GetBestTime()
    {
        return PlayerPrefs.GetFloat("BestTime_Tutorial", 9999f);
    }

    public void RestartGame()
    {
        launchDirectlyIntoFreeFlight = isFreeFlight;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartTutorial()
    {
        launchDirectlyIntoFreeFlight = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}