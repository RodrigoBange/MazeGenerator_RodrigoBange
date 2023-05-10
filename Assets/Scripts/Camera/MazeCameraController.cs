using UnityEngine;

public class MazeCameraController : MonoBehaviour
{
    [SerializeField]
    private Camera mazeCamera;

    /// <summary>
    /// Sets the camera position depending on the size of the maze.
    /// </summary>
    public void SetCameraPosition()
    {
        // Set to perspective
        mazeCamera.orthographic = false;
        mazeCamera.nearClipPlane = 0.01f;
        transform.rotation = Quaternion.Euler(90, 0, 0);

        float x = (MazeController.Instance.transform.position.x + MazeController.Instance.Width) / 2 - 0.5f;
        float y = 21.5f + (MazeController.Instance.Width > MazeController.Instance.Height ? MazeController.Instance.Width : MazeController.Instance.Height);
        float z = -(MazeController.Instance.transform.position.z + MazeController.Instance.Height) / 2 + 0.5f;

        transform.position = new Vector3(x, y, z);
    }
}
