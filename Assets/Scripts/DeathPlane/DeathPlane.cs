using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().AllowMovement(false);
            gameManager.RespawnPlayer();            
        }
    }
}
