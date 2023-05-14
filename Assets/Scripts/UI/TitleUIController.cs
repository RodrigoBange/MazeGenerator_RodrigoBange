using UnityEngine;
using UnityEngine.UI;

public class TitleUIController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup titleCanvas;

    [SerializeField]
    private TitleObjectsController titleObjectsController;

    [SerializeField]
    private Button titleButton;

    [SerializeField]
    private GameObject callToAction;

    [SerializeField]
    private UIController uiController;

    private bool fadeTitle;

    private void OnEnable()
    {
        // Set default values.
        titleButton.interactable = false;
        titleCanvas.LeanAlpha(1f, 1f).setOnComplete(EnableTitleFunctionality).setDelay(0.8f);
        fadeTitle = false;
        titleObjectsController.gameObject.SetActive(true);
    }

    /// <summary>
    /// Enables the click functionality and animation.
    /// </summary>
    private void EnableTitleFunctionality()
    {
        titleButton.interactable = true;
        callToAction.LeanScale(new Vector3(1.05f, 1.05f, 1.05f), 1f).setLoopPingPong();
    }

    /// <summary>
    /// Enter the game, leave the title screen.
    /// </summary>
    public void OnTitleClick()
    {
        if (fadeTitle == true) // Disable button spam.
        {
            return;
        }

        fadeTitle = true;
        titleObjectsController.ActivateTitleAnimation();

        titleCanvas.LeanAlpha(0f, 1.5f).setOnComplete(DisableTitleUI);
    }

    /// <summary>
    /// Disables the title screen.
    /// </summary>
    private void DisableTitleUI()
    {
        uiController.ToggleMenuUI(true);
        gameObject.SetActive(false);
    }
}
