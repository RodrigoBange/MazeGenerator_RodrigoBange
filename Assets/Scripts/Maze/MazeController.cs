using System.Collections;
using UnityEngine;

public class MazeController : MonoBehaviour
{
    [SerializeField]
    private WilsonMazeGenerator wilsonMazeGenerator;

    [SerializeField]
    private PrimMazeGenerator primMazeGenerator;

    private IEnumerator coroutine;

    private int width, height;

    public int Width => width;
    public int Height => height;

    private static MazeController instance;

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
    public void GenerateMaze(int width, int height, MazeAlgo algorithm)
    {
        // Check to avoid creating a maze while the current is in progress.
        if (!wilsonMazeGenerator.IsDone || 
            !primMazeGenerator.IsDone)
        {
            return;
        }

        this.width = width;
        this.height = height;

        // Clear maze
        wilsonMazeGenerator.ClearMaze();
        primMazeGenerator.ClearMaze();

        // Enable map camera
        GameManager.Instance.ActivatePlayerCamera(false);
        GameManager.Instance.SetCameraPosition();

        if (algorithm == MazeAlgo.WilsonAlgo)
        {
            coroutine = wilsonMazeGenerator.CreateMaze(width, height, transform);
        } 
        else if (algorithm == MazeAlgo.PrimAlgo)
        {
            coroutine = primMazeGenerator.CreateMaze(width, height, transform);
        }

        StartCoroutine(coroutine);
    }

    public void GeneratePrimMaze(int width, int height)
    {
        // Check to avoid creating a maze while the previous is in progress.
        if (!primMazeGenerator.IsDone)
        {
            return;
        }

        primMazeGenerator.ClearMaze();

        // Enable map camera
        GameManager.Instance.ActivatePlayerCamera(false);
        GameManager.Instance.SetCameraPosition();

        coroutine = primMazeGenerator.CreateMaze(width, height, transform);
        StartCoroutine(coroutine);
    }
}
