using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private IMazeGenerator mazeGenerator;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField]
    private GameObject startPrefab;

    [SerializeField]
    private GameObject finishPrefab;

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


    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = gameObject.AddComponent<MazeGenerator>();

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

        coroutine = mazeGenerator.CreateMaze(width, height, cellPrefab, transform, startPrefab, finishPrefab, gameManager);
        StartCoroutine(coroutine);    
    }

    public int Width => width;
    public int Height => height;
}
