using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollisionDetector : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem pSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pSystem.Play();
            other.GetComponent<PlayerBehaviour>().AllowMovement(true);
        }
    }
}
