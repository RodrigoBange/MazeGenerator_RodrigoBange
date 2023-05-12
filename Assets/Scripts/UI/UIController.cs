using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleUI, menuUI, gameUI, pauseUI;

    [SerializeField]
    private TextMeshProUGUI timerText, attemptsText;

    [SerializeField]
    private Slider widthSlider, heightSlider;

    [SerializeField]
    private ToggleGroup mazeToggles;

    [SerializeField]
    private TextMeshProUGUI widthText, heightText;

    [SerializeField]
    private TitleController titleController;

    [SerializeField]
    private CanvasGroup titleUIGroup;

    private bool fadeTitle;

    public float timePlayed;
    public bool timerActive;

    public int attemptsCount;

    private void Update()
    {
        if (timerActive)
        {
            timePlayed += Time.deltaTime;
            timerText.text = timePlayed.ToString("00:00");
        }
    }

    /// <summary>
    /// Tells the MazeController to generate a new maze
    /// </summary>
    public void OnGenerateNewMazeClick()
    {
        int width = (int)widthSlider.value;
        int height = (int)heightSlider.value;

        Toggle activeToggle = mazeToggles.ActiveToggles().FirstOrDefault();
        
        if (activeToggle.gameObject.name == "Wilson")
        {
            MazeController.Instance.GenerateMaze(width, height, MazeAlgo.WilsonAlgo);
        }
        else if (activeToggle.gameObject.name == "Prim")
        {
            MazeController.Instance.GenerateMaze(width, height, MazeAlgo.PrimAlgo);
        }
    }

    /// <summary>
    /// Enables the Maze Generation Menu UI
    /// </summary>
    private void EnableMenuUI()
    {
        titleUI.SetActive(false);
        menuUI.SetActive(true);
    }

    /// <summary>
    /// Enter the game, leave the title screen.
    /// </summary>
    public void OnTitleClick()
    {
        if (fadeTitle == true) // Disable button spam
        {
            return;
        }

        fadeTitle = true;
        titleController.ActivateTitleAnimation();

        StartCoroutine(nameof(FadeTitleScreen));
        
    }

    /// <summary>
    /// Fades out the title screen and activates the Maze Menu UI.
    /// </summary>
    IEnumerator FadeTitleScreen()
    {
        while (titleUIGroup.alpha >= 0)
        {
            titleUIGroup.alpha -= 0.0025f;

            if (titleUIGroup.alpha < 0.0025f)
            {
                EnableMenuUI();
            }
            yield return new WaitForSeconds(0.0025f);
        }
    }

    /// <summary>
    /// Starts the game after the start game button is clicked.
    /// </summary>
    public void OnStartGameClick()
    {
        attemptsCount = 0;
        GameManager.Instance.SetUpPlayer();
        menuUI.SetActive(false);
        gameUI.SetActive(true);
    }

    /// <summary>
    /// Pauses the game after the pause button is clicked.
    /// </summary>
    public void OnPauseGameClick()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Resumes the game after the resume button is clicked.
    /// </summary>
    public void OnResumeGameClick()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }    

    /// <summary>
    /// Activates the timer.
    /// </summary>
    public void ActivateTimer()
    {
        timePlayed = 0f;
        timerActive = true;
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
    /// Updates the maze width text value on the Maze Menu UI.
    /// </summary>
    public void OnSliderWidthChange()
    {
        widthText.text = widthSlider.value.ToString();
    }

    /// <summary>
    /// Updates the maze height text value on the Maze Menu UI.
    /// </summary>
    public void OnSliderHeightChange()
    {
        heightText.text = heightSlider.value.ToString();
    }
}
