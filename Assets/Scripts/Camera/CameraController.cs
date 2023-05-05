using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraHeight;

    // TODO: Implement camera functionality
    void Start()
    {
        
    }

    void Update()
    {
        float x = (MapController.Instance.transform.position.x + MapController.Instance.Width / 2) - 0.5f;
        float y = (MapController.Instance.transform.position.y - MapController.Instance.Height / 2) + 0.5f;
        float z = (MapController.Instance.Width > MapController.Instance.Height ? MapController.Instance.Width : MapController.Instance.Height) + cameraHeight;
        transform.position = new Vector3(x, y, -z);
    }
}
