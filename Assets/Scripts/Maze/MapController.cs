using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private MazeGenerator mazeGenerator;

    private IEnumerator coroutine;

    private static MapController instance;

    private int width;
    private int height;

    public static MapController Instance
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

    void Start()
    {
        GenerateMaze(10, 10);
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

        // Enable map camera
        GameManager.Instance.ActivatePlayerCamera(false);
        //gameManager.ActivatePlayerCamera(false);

        mazeGenerator.ClearMaze();

        this.width = width;
        this.height = height;

        coroutine = mazeGenerator.CreateMaze(width, height, transform);
        StartCoroutine(coroutine);    
    }

    public int Width => width;
    public int Height => height;
}
