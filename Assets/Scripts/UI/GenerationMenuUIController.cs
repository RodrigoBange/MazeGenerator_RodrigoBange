using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenerationMenuUIController : MonoBehaviour
{
    [SerializeField]
    private RectTransform generationPanelRect;

    private Vector3 generationPanelPosition;

    [SerializeField]
    private Slider widthSlider, heightSlider;

    [SerializeField]
    private ToggleGroup mazeToggles;

    [SerializeField]
    private TextMeshProUGUI widthText, heightText;

    [SerializeField]
    private TextMeshProUGUI generatingText;

    [SerializeField]
    private Button generateButton, cancelButton, playButton;

    public bool generatingMap;

    private readonly WaitForSeconds mapDelay = new(0.3f);

    private void Awake()
    {
        // Save position.
        generationPanelPosition = generationPanelRect.anchoredPosition;
    }

    private void OnEnable()
    {
        // Set default values.
        generationPanelRect.anchoredPosition = generationPanelPosition;
        generationPanelRect.LeanMoveX(-200f, 0.5f);
    }

    /// <summary>
    /// Tells the MazeController to generate a new maze.
    /// </summary>
    public void OnGenerateMazeClick()
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

        generatingMap = true;
        generatingText.gameObject.SetActive(true);
        generateButton.interactable = false;
        cancelButton.interactable = true;
        playButton.interactable = false;
        StartCoroutine(GenerationProcess());
    }

    /// <summary>
    /// Animates the generation text during a maze generation.
    /// </summary>
    private IEnumerator GenerationProcess()
    {
        while (generatingMap)
        {
            generatingText.text += ".";
            if (generatingText.text == "GENERATING....")
            {
                generatingText.text = "GENERATING";
            }
            yield return mapDelay;
        }
    }

    /// <summary>
    /// Cancels the maze generation.
    /// </summary>
    public void OnCancelClick()
    {
        MazeController.Instance.CancelGeneration();
        GenerationCancel();
    }

    /// <summary>
    /// Aborts the process of the current maze generation.
    /// </summary>
    private void GenerationCancel()
    {
        generatingMap = false;
        generatingText.gameObject.SetActive(false);
        generateButton.interactable = true;
        cancelButton.interactable = false;
        playButton.interactable = false;
    }

    /// <summary>
    /// Enables the play button, called after a maze has been generated.
    /// </summary>
    public void DisplayPlayButton()
    {
        generatingMap = false;
        generatingText.gameObject.SetActive(false);
        generateButton.interactable = true;
        cancelButton.interactable = false;
        playButton.interactable = true;
    }

    /// <summary>
    /// Starts the game after the start game button is clicked.
    /// </summary>
    public void OnPlayLevelClick()
    {
        GameManager.Instance.SetUpPlayer();
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
