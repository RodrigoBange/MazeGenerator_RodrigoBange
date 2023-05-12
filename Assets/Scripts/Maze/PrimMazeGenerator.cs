using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimMazeGenerator : MonoBehaviour, IMazeGenerator
{
    [SerializeField]
    private GameObject startPlatform, finishPlatform;

    [SerializeField]
    private CellPooler cellPooler;

    private CellController[,] cells;

    private List<CellController> frontierCellNeighbors;
    private List<CellController> mazeNeighbors;

    private CellController neighborCell;
    private CellController frontierCell;

    [SerializeField]
    private List<CellController> frontier;

    private WaitForSeconds delayGeneration;
    private float chunks, progressChunks;

    private int width, height;

    private bool isDone = true;

    /// <summary>
    /// Returns true if the maze generation is done.
    /// </summary>
    public bool IsDone => isDone;

    /// <summary>
    /// Generates a maze of cells.
    /// </summary>
    /// <param name="width">The horizontal size of the maze.</param>
    /// <param name="height">The vertical size of the maze.</param>
    /// <param name="parent">The parent of the maze to generate the cells in.</param>
    public IEnumerator CreateMaze(int width, int height, Transform parent)
    {
        // Set default values
        isDone = false;
        this.width = width;
        this.height = height;
        cells = new CellController[width, height];
        frontier = new();

        // Create the cells
        yield return StartCoroutine(CreateMazeCells());

        // Pick a starting point
        PickRandomStartingPoint();

        chunks = SetGenerationDelay(); // Reference to save for incremental usage.
        progressChunks = chunks; // Set start amount.
        float i = 0;

        // While there are still frontiers left open, create paths
        while (frontier.Count > 0)
        {
            PickRandomFrontierCell();

            // Load a chunk at a time
            i++;
            if (i >= progressChunks)
            {
                progressChunks += chunks;
                yield return delayGeneration;
            }
            
        }
        
        // Generates the start and finish platforms.
        CreateStartAndFinish(parent);

        // Maze has been generated
        isDone = true;
    }

    /// <summary>
    /// Generates the cells of the maze.
    /// </summary>
    private IEnumerator CreateMazeCells()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = cellPooler.pool.Get();
                cells[x, z] = cell;
                cells[x, z].SetPosition(x, z);
                cells[x, z].SetCellActive(false);

                // Add yourself to neighbor to left and above.
                if (x > 0)
                {
                    cells[x - 1, z].AddNeighbor(cells[x, z]);
                    cells[x, z].AddNeighbor(cells[x - 1, z]);
                }
                if (z > 0)
                {
                    cells[x, z - 1].AddNeighbor(cells[x, z]);
                    cells[x, z].AddNeighbor(cells[x, z - 1]);
                }
            }
            // Display a row at a time.
            yield return null;
        }
    }

    /// <summary>
    /// Pick a random start point within the maze
    /// </summary>
    private void PickRandomStartingPoint()
    {
        // First pick a random start point 
        // Mark the cell as part of the maze and set frontier neighbors
        MarkCell(cells[Random.Range(0, width), Random.Range(0, height)]);
    }

    /// <summary>
    /// Picks a random frontier cell to work a path from
    /// </summary>
    private void PickRandomFrontierCell()
    {
        // Pick a random frontier cell and remove from frontier list
        frontierCell = frontier[Random.Range(0, frontier.Count)];
        frontier.Remove(frontierCell);

        // Get neighbor cells that are part of the maze
        frontierCellNeighbors = GetInMazeNeighbors(frontierCell);

        //Select a random neighbor
        neighborCell = frontierCellNeighbors[Random.Range(0, frontierCellNeighbors.Count)];

        // Open path from neighbor cell to
        neighborCell.TogglePath(neighborCell.GetSide(frontierCell), true);
        frontierCell.TogglePath(frontierCell.GetSide(neighborCell), true);

        // Mark a new cell
        MarkCell(frontierCell);
    }

    /// <summary>
    /// Marks a cell as part of the maze and it's out of maze neighbors as frontiers.
    /// </summary>
    /// <param name="cell"></param>
    private void MarkCell(CellController cell)
    {
        cell.IsVisited = true;
        foreach (var neighbor in cell.Neighbors)
        {
            if (!neighbor.IsVisited)
            {
                if (!frontier.Contains(neighbor)) // Avoid duplicates
                {
                    frontier.Add(neighbor);
                }                
            }
        }
    }

    /// <summary>
    /// Retrieve all neighbors that are part of the maze from the frontierCell.
    /// </summary>
    /// <param name="frontierCell"></param>
    /// <returns>A list of all neighbors part of the maze</returns>
    private List<CellController> GetInMazeNeighbors(CellController frontierCell)
    {
        mazeNeighbors = new();

        // Can't be any neighbor, must be a connection. So basically requires an open wall
        foreach (var neighbor in frontierCell.Neighbors)
        {
            if (neighbor.IsVisited)
            {
                mazeNeighbors.Add(neighbor);
            }            
        }

        return mazeNeighbors;
    }

    /// <summary>
    /// Sets a delay by indicating how many chunks of the maze can be loaded at a specific time.
    /// </summary>
    /// <returns>Returns the amount of chunks.</returns>
    private float SetGenerationDelay()
    {
        // Values to present parts of the maze for step-by-step display.
        float maxCells = width * height;
        float chunks = maxCells / 100;

        // Limit amount of steps and set delay, helps prevent freezes.
        if (chunks > 60) // In case of a big maze, reduce the amount loaded.
        {
            chunks = 60;
            delayGeneration = new(0.125f);
        }
        else
        {
            delayGeneration = new(0.01f);
        }

        return chunks;
    }


    /// <summary>
    /// Opens a path at the top and bottom of the maze and creates the start and finish platforms.
    /// </summary>
    /// <param name="parent">The parent to attach the parts onto.</param>
    private void CreateStartAndFinish(Transform parent)
    {
        // Open the top path of the first cell, and the bottom path of the last cell.
        CellController start = cells[Random.Range(0, cells.GetLength(0)), 0];
        CellController finish = cells[Random.Range(0, cells.GetLength(0)), height - 1];
        start.TogglePath(Side.Top, true);
        finish.TogglePath(Side.Bottom, true);

        // Create the start and finish platforms.
        startPlatform.SetActive(true);
        startPlatform.transform.position = new Vector3(start.Position.x, parent.position.y - 0.1f, start.Position.z + 1.175f);
        finishPlatform.SetActive(true);
        finishPlatform.transform.position = new Vector3(finish.Position.x, parent.position.y - 0.2f, -finish.Position.z - 1.175f);
    }

    /// <summary>
    /// Clears the maze of all cells and stores them back in a pool. Returns if no cell has been generated.
    /// </summary>
    public void ClearMaze()
    {
        if (cells == null) // Return is no maze has been generated.
        {
            return;
        }

        foreach (CellController cell in cells)
        {
            cellPooler.pool.Release(cell);
        }

        startPlatform.SetActive(false);
        finishPlatform.SetActive(false);

        cells = new CellController[0, 0];
    }
}
