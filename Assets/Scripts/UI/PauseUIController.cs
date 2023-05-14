using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    [SerializeField]
    private UIController uiController;

    /// <summary>
    /// Resumes the game after the resume button is clicked.
    /// </summary>
    public void OnResumeGameClick()
    {
        // Activate time.
        Time.timeScale = 1f;

        // Disable pause screen.
        gameObject.SetActive(false);        
    }

    /// <summary>
    /// Quits the current level and returns to menu.
    /// </summary>
    public void OnQuitLevel()
    {
        // Activate time 
        Time.timeScale = 1f;

        GameManager.Instance.QuitLevel();
    }
}
