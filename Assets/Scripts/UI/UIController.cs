using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleUI, menuUI, gameUI, pauseUI, levelCompleteUI;

    [SerializeField]
    private GameUIController gameUIController;

    [SerializeField]
    private LevelCompleteUIController levelCompleteController;

    /// <summary>
    /// Enables the Maze Generation Menu UI.
    /// </summary>
    public void ToggleMenuUI(bool value)
    {
        DisableAllUI();
        menuUI.SetActive(value);
    }

    /// <summary>
    /// Enables the Game UI.
    /// </summary>
    public void ToggleGameUI(bool value)
    {
        DisableAllUI();
        gameUI.SetActive(value);
    }

    /// <summary>
    /// Enables the completion UI
    /// </summary>
    public void ToggleLevelCompleteUI()
    {
        DisableAllUI();

        // Get variable and set to levelComplete.
        levelCompleteController.timePlayed = gameUIController.timePlayed;
        levelCompleteController.attempts = gameUIController.attemptsCount;
        levelCompleteUI.SetActive(true);
    }

    /// <summary>
    /// Sets all menu's to inactive, used before activing one. Avoids duplicate code.
    /// </summary>
    private void DisableAllUI()
    {
        titleUI.SetActive(false);
        menuUI.SetActive(false);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        levelCompleteUI.SetActive(false);
    }
}
