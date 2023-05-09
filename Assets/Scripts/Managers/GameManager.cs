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
    /// <param name="spawnLocation">Location of the players respawn.</param>
    public void SetUpPlayer(Vector3 spawnLocation)
    {
        respawnPoint = new Vector3(spawnLocation.x, spawnLocation.y + 5f, spawnLocation.z);
        player.transform.position = respawnPoint;
        player.SetActive(true);
        ActivatePlayerCamera(true);
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

    public void SetCameraPosition()
    {
        mazeCamera.GetComponent<MazeCameraController>().SetCameraPosition();
    }
}
