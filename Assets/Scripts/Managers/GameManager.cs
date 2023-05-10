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
    private GameObject startPlatform;

    private Vector3 respawnPoint;

    [SerializeField]
    private UIController uiController;

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

        // Set position of camera
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
        respawnPoint = new Vector3(startPlatform.transform.position.x, startPlatform.transform.position.y + 5f, startPlatform.transform.position.z);
        player.transform.position = respawnPoint;
        player.SetActive(true);
        ActivatePlayerCamera(true);

        Invoke(nameof(ActivateTimer), 1.2f);
    }

    /// <summary>
    /// Activate the timer in the UI
    /// </summary>
    private void ActivateTimer()
    {
        uiController.timerActive = true;
    }

    /// <summary>
    /// Respawns the player to the start platform. Called after the player has reached the death plane.
    /// </summary>
    public void RespawnPlayer()
    {
        uiController.AddAttempt();
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
}
