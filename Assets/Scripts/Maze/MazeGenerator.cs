using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour, IMazeGenerator
{
    private CellController[,] cells;

    [SerializeField]
    private GameObject startPlatform, finishPlatform;

    [SerializeField]
    private CellPooler cellPooler;

    private bool isDone = true;

    /// <summary>
    /// Generates a maze of cells.
    /// </summary>
    /// <param name="width">The horizontal size of the maze.</param>
    /// <param name="height">The vertical size of the maze.</param>
    /// <param name="parent">The parent of the maze to generate the cells in.</param>
    public IEnumerator CreateMaze(int width, int height, Transform parent)
    {
        isDone = false;

        cells = new CellController[width, height];
        List<CellController> unvisitedCells = new();

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = cellPooler.pool.Get();
                cells[x, z] = cell;
                cells[x, z].SetPosition(x, z);
                cells[x, z].SetCellActive(false);

                unvisitedCells.Add(cells[x, z]);

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

        int pathIndex = 0;

        // First we pick a random start and goal cell for the first path.
        CellController currentCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];
        currentCell.SetCellActive(true);
        currentCell.PathIndex = pathIndex;
        unvisitedCells.Remove(currentCell);
        CellController goalCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];
        goalCell.SetCellActive(true);

        Stack<CellController> path = new();
        path.Push(currentCell);
        List<CellController> wrongCells = new();

        // Values to present parts of the maze step-by-step instead of instant display.
        float maxCells = width * height;
        float chunks = maxCells / 100;
        WaitForSeconds delay;

        // Limit amount of steps and set delay, helps prevent freezes.
        if (chunks > 60) // In case of a big maze, slow down the amount loaded.
        {
            chunks = 60;
            delay = new(0.125f);
        } else
        {
            delay = new(0.01f);
        }

        float progressChunks = chunks;
        float i = 0;

        while (true) // While coroutine is active
        {
            // Load a chunk of the maze at a time.
            i++;
            if (i >= progressChunks)
            {
                progressChunks += chunks;

                yield return delay;
            }

            // Fixes a problem of path incorrectly being left as correct, if it had more than one possible neighbor,
            // and they turned out to be both wrong.
            int possibleNeighbours = currentCell.Neighbors.Count + 1;

            // Filter out neighbors that are already part of the path, or are in the wrongCells list.
            List<CellController> neighbors = new(currentCell.Neighbors);
            // Priority list for inactive cells, or cells with only two open walls.
            List<CellController> priorityList = new();
            foreach (CellController neighbor in currentCell.Neighbors)
            {
                if (neighbor.PathIndex == pathIndex || wrongCells.Contains(neighbor) && neighbor.IsConnectable)
                {
                    neighbors.Remove(neighbor);
                    possibleNeighbours--;
                }
                else
                {
                    // Prefer inactive cells, or cells with only two open walls or less.
                    if (!neighbor.IsActive || neighbor.GetOpenPathsCount() <= 2)
                    {
                        priorityList.Add(neighbor);
                    }
                }

                // Biggest priority is to find the goal cell, if pathindex is 0.
                if (neighbor == goalCell && pathIndex == 0)
                {
                    priorityList.Clear();
                    neighbors.Clear();
                    neighbors.Add(neighbor);
                    break;
                }
            }

            // If there are more important cells, use them instead.
            if (priorityList.Count > 0)
            {
                neighbors = priorityList;
            }

            // If the current cell is not part of the path, and it has more than one possible neighbor, add it to the path.
            // This avoids issue with the path being left as correct, if it had more than one possible neighbor,
            // and they turned out to be both wrong.
            if (possibleNeighbours > 1 && !path.Contains(currentCell))
            {
                path.Push(currentCell);
            }

            if (neighbors.Count == 0)
            {
                // We are at a dead end. Go back.
                CellController badCell = currentCell;
                badCell.SetCellActive(false);
                wrongCells.Add(badCell);

                // Add the cell to unvisited cells, so we can try to find a new path to it.
                if (!unvisitedCells.Contains(badCell))
                {
                    unvisitedCells.Add(badCell);
                }

                currentCell = path.Pop();
                currentCell.TogglePath(currentCell.GetSide(badCell), false);
                badCell.TogglePath(badCell.GetSide(currentCell), false);

                continue;
            }

            // Pick random neighbor from the neighbors.
            CellController nextCell = neighbors[Random.Range(0, neighbors.Count)];

            // Connect the cells.
            currentCell.TogglePath(currentCell.GetSide(nextCell), true);
            nextCell.TogglePath(nextCell.GetSide(currentCell), true);

            unvisitedCells.Remove(nextCell);

            if (nextCell.IsActive)
            {
                if (pathIndex == 0 && nextCell == goalCell)
                {
                    nextCell.PathIndex = pathIndex;
                }

                // We found a path with another index. We can stop generating that path.
                pathIndex++;

                // If path is only 2 tile long, make it unconnectable.
                if (path.Count <= 1)
                {
                    currentCell.IsConnectable = false;
                }

                if (unvisitedCells.Count == 0)
                {
                    // We are done.
                    break;
                }

                // We found a path to a cell that is already part of a path. We can stop generating that path.
                currentCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];
                currentCell.PathIndex = pathIndex;
                currentCell.SetCellActive(true);
                unvisitedCells.Remove(currentCell);

                path.Clear();
                path.Push(currentCell);
                wrongCells.Clear();
            }
            else
            {
                nextCell.SetCellActive(true);
                // We found an unoccupied cell. Continue path.
                path.Push(nextCell);
                nextCell.PathIndex = pathIndex;
                currentCell = nextCell;
            }
        }

        // Open the top path of the first cell, and the bottom path of the last cell.
        CellController start = cells[Random.Range(0, cells.GetLength(0)), 0];
        CellController finish = cells[Random.Range(0, cells.GetLength(0)), height - 1];
        start.TogglePath(Side.Top, true);
        finish.TogglePath(Side.Bottom, true);

        // Activate spawn and finish
        startPlatform.SetActive(true);
        startPlatform.transform.position = new Vector3(start.Position.x, parent.position.y - 0.1f, start.Position.z + 1.175f);
        finishPlatform.SetActive(true);
        finishPlatform.transform.position = new Vector3(finish.Position.x, parent.position.y - 0.2f, -finish.Position.z - 1.175f);

        // Maze has been loaded
        isDone = true;
        Debug.Log("Maze loaded");
        GameManager.Instance.SetUpPlayer(startPlatform.transform.position);
        //gameManager.SetUpPlayer(startPlatform.transform.position);
    }

    /// <summary>
    /// Returns true if the maze generation is done.
    /// </summary>
    public bool IsDone => isDone;

    /// <summary>
    /// Returns the entire grid of cells.
    /// </summary>
    public CellController[,] Cells => cells;

    /// <summary>
    /// Clears the maze of all cells and stores them back in a pool. Returns if no cell has been generated.
    /// </summary>
    public void ClearMaze()
    {
        if (cells == null)
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
