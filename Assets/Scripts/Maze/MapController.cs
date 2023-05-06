using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private IMazeGenerator mazeGenerator;

    [SerializeField]
    private GameObject cellPrefab;

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
        if (!mazeGenerator.IsDone)
        {
            return;
        }

        mazeGenerator.ClearMaze();

        this.width = width;
        this.height = height;

        coroutine = mazeGenerator.CreateMaze(width, height, cellPrefab, transform);
        StartCoroutine(coroutine);
    }

    public int Width => width;
    public int Height => height;
}
