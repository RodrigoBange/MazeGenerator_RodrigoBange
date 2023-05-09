using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private GameObject playerCamera, mapCamera;

    [SerializeField]
    private GameObject player;

    private Vector3 respawnPoint;

    public void ActivatePlayerCamera(bool value)
    {
        playerCamera.SetActive(value);
        mapCamera.SetActive(!value);

        // Set position of camera
        if (!value)
        {
            mapCamera.GetComponent<MapCameraController>().SetCameraPosition();
        }
    }

    public void SetUpPlayer(Vector3 spawnLocation)
    {
        respawnPoint = new Vector3(spawnLocation.x, spawnLocation.y + 5f, spawnLocation.z);
        player.transform.position = respawnPoint;
        player.SetActive(true);
        ActivatePlayerCamera(true);
    }

    public void RespawnPlayer()
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.SetPositionAndRotation(respawnPoint, Quaternion.Euler(0, 180, 0));
    }
}
