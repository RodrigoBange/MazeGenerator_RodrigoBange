using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject playerCamera, mazeCamera;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private PlayerBehaviour playerBehaviour;

    [SerializeField]
    private GameObject startPlatform;

    private Vector3 respawnPoint;

    [SerializeField]
    private UIController uiController;

    [SerializeField]
    private GameUIController gameUIController;

    [SerializeField]
    private GenerationMenuUIController generationMenuUIController;

    public bool joystickEnabled;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Toggles between the player camera and map camera.
    /// </summary>
    /// <param name="value">Value to activate the player camera or not.</param>
    public void ActivatePlayerCamera(bool value)
    {
        playerCamera.SetActive(value);
        mazeCamera.SetActive(!value);

        // Set position of camera.
        if (!value)
        {
            mazeCamera.GetComponent<MazeCameraController>().SetCameraPosition();
        }
    }

    /// <summary>
    /// Sets up the player. Called after the maze has been loaded.
    /// </summary>
    public void SetUpPlayer()
    {
        if (player.activeSelf) // If player was still active, reset.
        { 
            player.SetActive(false); 
        }

        playerBehaviour.joystickEnabled = joystickEnabled;
        gameUIController.joystickEnabled = joystickEnabled;
        player.transform.SetPositionAndRotation(respawnPoint, Quaternion.Euler(0, 180, 0));
        respawnPoint = new Vector3(startPlatform.transform.position.x, startPlatform.transform.position.y + 5f, startPlatform.transform.position.z);
        player.transform.position = respawnPoint;

        uiController.ToggleGameUI(true);
        player.SetActive(true);
        ActivatePlayerCamera(true);
    }

    /// <summary>
    /// Respawns the player to the start platform. Called after the player has reached the death plane.
    /// </summary>
    public void RespawnPlayer()
    {
        gameUIController.AddAttempt();
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.SetPositionAndRotation(respawnPoint, Quaternion.Euler(0, 180, 0));
    }

    /// <summary>
    /// Sets the Maze Camera position
    /// </summary>
    public void SetCameraPosition()
    {
        mazeCamera.GetComponent<MazeCameraController>().SetCameraPosition();
    }

    /// <summary>
    /// Updates the UI that the maze has been generated and can be played.
    /// </summary>
    public void MazeGenerated()
    {
        generationMenuUIController.DisplayPlayButton();
    }

    /// <summary>
    /// Quits the current level and opens the maze menu.
    /// </summary>
    public void QuitLevel()
    {
        player.SetActive(false);
        ActivatePlayerCamera(false);
        uiController.ToggleMenuUI(true);
    }

    /// <summary>
    /// Displays the level completion screen and disables the player.
    /// </summary>
    public void LevelFinished()
    {
        // Display UI.
        gameUIController.StopTimer();
        uiController.ToggleLevelCompleteUI();
    }
}
