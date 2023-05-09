using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private MazeGenerator mazeGenerator;

    [SerializeField]
    private GameManager gameManager;

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

    public void GenerateMaze(int width, int height)
    {
        // Check to avoid createMaze spam
        if (!mazeGenerator.IsDone)
        {
            return;
        }

        // Enable map camera
        gameManager.ActivatePlayerCamera(false);

        mazeGenerator.ClearMaze();

        this.width = width;
        this.height = height;

        coroutine = mazeGenerator.CreateMaze(width, height, transform);
        StartCoroutine(coroutine);    
    }

    public int Width => width;
    public int Height => height;
}
