using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MazeGenerator : MonoBehaviour, IMazeGenerator
{
    private bool isDone = true;

    private CellController[,] cells;

    public IEnumerator CreateMaze(int width, int height, GameObject cellPrefab, Transform parent)
    {
        isDone = false;

        cells = new CellController[width, height];
        List<CellController> unvisitedCells = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObject = Instantiate(cellPrefab, parent);
                cells[x, y] = cellObject.GetComponent<CellController>();
                cells[x, y].SetPosition(x, y);
                cells[x, y].SetCellActive(false);

                unvisitedCells.Add(cells[x, y]);

                // Add yourself to neighbor to left and above.
                if (x > 0)
                {
                    cells[x - 1, y].AddNeighbor(cells[x, y]);
                    cells[x, y].AddNeighbor(cells[x - 1, y]);
                }
                if (y > 0)
                {
                    cells[x, y - 1].AddNeighbor(cells[x, y]);
                    cells[x, y].AddNeighbor(cells[x, y - 1]);
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
        float baseSpeed = maxCells / 100;
        float speed = baseSpeed;
        float i = 0;

        while (true) // While coroutine is active
        {
            // Load a chunk of the maze at a time.
            i++;
            if (i >= speed)
            {
                speed += baseSpeed;
                yield return new WaitForSeconds(0.004f);
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
                    // Prefer inactive cells, or cells with only two open walls.
                    if (!neighbor.IsActive || neighbor.GetOpenWallsCount() <= 2)
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
                currentCell.ToggleWall(currentCell.GetSide(badCell), true);

                continue;
            }

            // Pick random neighbor from the neighbors.
            CellController nextCell = neighbors[Random.Range(0, neighbors.Count)];

            // If the next cell is connectable (aka, path has more than one cell), connect the cells.
            //if (nextCell.IsConnectable)
            //{
            // TODO: Test this part
                currentCell.ToggleWall(currentCell.GetSide(nextCell), false);
                nextCell.ToggleWall(nextCell.GetSide(currentCell), false);
            //}

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

        // Open the top wall of the first cell, and the bottom wall of the last cell.
        cells[Random.Range(0, cells.GetLength(0)), 0].ToggleWall(Side.Top, false);
        cells[Random.Range(0, cells.GetLength(0)), height - 1].ToggleWall(Side.Bottom, false);

        isDone = true;
        Debug.Log("Maze loaded");
    }

    /// <summary>
    /// Returns true if the maze generation is done.
    /// </summary>
    public bool IsDone => isDone;

    /// <summary>
    /// Returns the entire grid of cells.
    /// </summary>
    public CellController[,] Cells => cells;

    public void ClearMaze()
    {
        if (cells == null)
        {
            return;
        }

        foreach (CellController cell in cells)
        {
            Destroy(cell.gameObject);
        }

        cells = new CellController[0, 0];
    }
}
