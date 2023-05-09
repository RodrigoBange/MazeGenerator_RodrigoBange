using System.Collections;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    [SerializeField]
    private MazeGenerator mazeGenerator;

    private IEnumerator coroutine;

    private static MazeController instance;

    private int width;
    private int height;

    public static MazeController Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Generates a maze with the given sizes.
    /// </summary>
    /// <param name="width">The horizontal size of the maze.</param>
    /// <param name="height">The vertical size of the maze.</param>
    public void GenerateMaze(int width, int height)
    {
        // Check to avoid creating a maze while the previous is in progress.
        if (!mazeGenerator.IsDone)
        {
            return;
        }

        mazeGenerator.ClearMaze();

        this.width = width;
        this.height = height;

        // Enable map camera
        GameManager.Instance.ActivatePlayerCamera(false);
        GameManager.Instance.SetCameraPosition();

        coroutine = mazeGenerator.CreateMaze(width, height, transform);
        StartCoroutine(coroutine);    
    }

    public int Width => width;
    public int Height => height;
}
