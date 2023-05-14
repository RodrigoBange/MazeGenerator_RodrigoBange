using UnityEngine;

public class FinishCollisionDetector : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem pSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pSystem.Play();
            PlayerBehaviour pBehaviour = other.GetComponent<PlayerBehaviour>();
            pBehaviour.AllowMovement(false);
            pBehaviour.goalPosition = transform.position;
            pBehaviour.flyUp = true;

            // Notify the manager that the level has been finished.
            GameManager.Instance.LevelFinished();
        }
    }
}
