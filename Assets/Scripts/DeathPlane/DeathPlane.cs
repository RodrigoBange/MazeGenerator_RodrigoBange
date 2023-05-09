using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    /// <summary>
    /// If touched, respawn the player to the start.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().AllowMovement(false);
            gameManager.RespawnPlayer();            
        }
    }
}
