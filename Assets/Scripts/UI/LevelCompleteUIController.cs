using TMPro;
using UnityEngine;

public class LevelCompleteUIController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup levelCompleteCanvas;

    [SerializeField]
    private TextMeshProUGUI timeText, attemptsText;

    public float timePlayed;
    public int attempts;

    private string TimePlayedText 
    { 
        get 
        { 
            return Mathf.Floor(timePlayed / 60).ToString("00") + ":" + Mathf.FloorToInt(timePlayed % 60).ToString("00");
        }
    }

    private void OnEnable()
    {
        // Set default value.
        levelCompleteCanvas.alpha = 0f;

        // Display finish time and attempts.
        timeText.text = TimePlayedText;
        attemptsText.text = attempts.ToString();

        // Fade in screen.
        levelCompleteCanvas.LeanAlpha(1f, 0.5f).setDelay(0.5f);
    }

    /// <summary>
    /// Leaves the current level and goes back to the generation menu.
    /// </summary>
    public void OnContinueClick()
    {
        GameManager.Instance.QuitLevel();
    }

    /// <summary>
    /// Sets up the player again to retry the level.
    /// </summary>
    public void OnRetryClick()
    {
        GameManager.Instance.SetUpPlayer();
    }
}
