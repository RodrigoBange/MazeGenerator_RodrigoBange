using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;

    [SerializeField]
    private TextMeshProUGUI timerText, attemptsText;

    [SerializeField]
    private GameObject joystick, dpad;

    public bool joystickEnabled;

    public float timePlayed;

    private string TimePlayedText
    {
        get
        {
            return Mathf.Floor(timePlayed / 60).ToString("00") + ":" + Mathf.FloorToInt(timePlayed % 60).ToString("00");
        }
    }

    public bool timerActive;

    public int attemptsCount;

    private void OnEnable()
    {
        // After game started, start timer and set attempts to default.
        timePlayed = 0f;
        timerText.text = TimePlayedText;
        Invoke(nameof(ActivateTimer), 0.9f);
        attemptsCount = 0;
        attemptsText.text = attemptsCount.ToString();
        EnableMobileInput();
    }

    private void Update()
    {
        if (timerActive)
        {
            timePlayed += Time.deltaTime;
            timerText.text = TimePlayedText;
        }
    }

    /// <summary>
    /// Checks if platform is of mobile and enables the requested input.
    /// </summary>
    private void EnableMobileInput()
    {
        joystick.SetActive(Application.isMobilePlatform && joystickEnabled);
        dpad.SetActive(Application.isMobilePlatform && !joystickEnabled);
    }

    /// <summary>
    /// Activates the timer.
    /// </summary>
    public void ActivateTimer()
    {
        timerActive = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        timerActive = false;
    }

    /// <summary>
    /// Adds +1 to the attempts counter.
    /// </summary>
    public void AddAttempt()
    {
        attemptsCount++;
        attemptsText.text = attemptsCount.ToString();
    }

    /// <summary>
    /// Pauses the game after the pause button is clicked.
    /// </summary>
    public void OnPauseGameClick()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
