using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CellController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] walls;

    [SerializeField]
    private List<CellController> neighbors = new();

    private Vector3 position = Vector3.zero;

    [SerializeField]
    private int pathIndex = -1;

    [SerializeField]
    private bool isActive;

    public int PathIndex
    {
        get
        {
            return pathIndex;
        }
        set
        {
            pathIndex = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            var pos = value;
            pos.z *= -1;
            transform.localPosition = pos;
        }
    }

    /// <summary>
    /// Sets the position of the cell in the maze.
    /// </summary>
    /// <param name="x">X position of cell.</param>
    /// <param name="z">Z position of cell.</param>
    public void SetPosition(int x, int z)
    {
        Position = new Vector3(x, 0, z);
    }

    /// <summary>
    /// Opens a path of closes a path from the cell.
    /// </summary>
    /// <param name="side">The side of the cell to open/close.</param>
    /// <param name="active">Boolean to open(true) the path or close(false)</param>
    public void TogglePath(Side side, bool active)
    {
        walls[(int)side].GetComponent<MeshRenderer>().enabled = active;
        walls[(int)side].layer = active ? 0 : 1;
        walls[(int)side].GetComponent<BoxCollider>().enabled = active;
    }

    /// <summary>
    /// Sets the cell variables to active or unactive.
    /// </summary>
    /// <param name="active">Boolean to activate the cell values</param>
    public void SetCellActive(bool active)
    {
        isActive = active;

        if (!active)
        {
            isConnectable = true;
            pathIndex = -1;
        }
    }

    /// <summary>
    /// Checks if the cell has neighbors.
    /// </summary>
    /// <param name="cell">The cell to inspect.</param>
    /// <returns>Boolean if cell has the given neighbor.</returns>
    public bool HasNeighbor(CellController cell)
    {
        return neighbors.Contains(cell);
    }

    /// <summary>
    /// Adds a neighbor to the given cell.
    /// </summary>
    /// <param name="cell">The cell to inspect.</param>
    public void AddNeighbor(CellController cell)
    {
        neighbors.Add(cell);
    }

    /// <summary>
    /// Removes a neighbor from the given cell.
    /// </summary>
    /// <param name="cell">The cell to inspect.</param>
    public void RemoveNeighbor(CellController cell)
    {
        neighbors.Remove(cell);
    }

    /// <summary>
    /// Checks if a side from the given cell is open or not.
    /// </summary>
    /// <param name="side">The side to inspect of the cell.</param>
    /// <returns>Boolean if the side is open or not.</returns>
    public bool IsSideOpen(Side side)
    {
        return walls[(int)side].GetComponent<MeshRenderer>().enabled;
    }

    public List<CellController> Neighbors
    {
        get
        {
            return neighbors;
        }
    }

    public bool IsActive => isActive;

    [SerializeField]
    private bool isConnectable = true;
    public bool IsConnectable { get => isConnectable; set => isConnectable = value; }

    /// <summary>
    /// Checks the amount of paths that are currently open.
    /// </summary>
    /// <returns>Returns the amount of paths that are open.</returns>
    public int GetOpenPathsCount()
    {
        int count = 0;
        if (IsSideOpen(Side.Top))
        {
            count++;
        }
        if (IsSideOpen(Side.Right))
        {
            count++;
        }
        if (IsSideOpen(Side.Bottom))
        {
            count++;
        }
        if (IsSideOpen(Side.Left))
        {
            count++;
        }
        return count;
    }

    /// <summary>
    /// Reset all values to default when cell is deactivated.
    /// </summary>
    private void OnDisable()
    {
        pathIndex = -1;
        isConnectable = true;
        position = Vector3.zero;
        neighbors.Clear();
        TogglePath(Side.Top, false);
        TogglePath(Side.Right, false);
        TogglePath(Side.Bottom, false);
        TogglePath(Side.Left, false);
    }
}
