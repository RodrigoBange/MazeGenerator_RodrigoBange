using UnityEngine;

public class TitleCollisionPlatform : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem pSystem;

    private void OnTriggerEnter(Collider other)
    {
        pSystem.Play();
        gameObject.SetActive(false);
    }
}
