using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Update()
    {
        if (target.position.y > 19f) // If player is above water, follow them.
        {
            transform.position = new Vector3(target.position.x, 21, target.position.z) + offset;
        }
    }
}
