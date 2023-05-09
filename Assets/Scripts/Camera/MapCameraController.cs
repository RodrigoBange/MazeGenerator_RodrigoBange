using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public void SetCameraPosition()
    {
        float x = (MapController.Instance.transform.position.x + MapController.Instance.Width / 2) - 0.5f;
        float y = (MapController.Instance.transform.position.y + MapController.Instance.Height / 2) + 0.5f;
        float z = (MapController.Instance.Width > MapController.Instance.Height ? MapController.Instance.Width : MapController.Instance.Height);
        transform.position = new Vector3(x, y, z);
    }
}
