using System.Collections.Generic;
using UnityEngine;

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

    public void SetPosition(int x, int z)
    {
        Position = new Vector3(x, 0, z);
    }

    public void TogglePath(Side side, bool active)
    {
        walls[(int)side].GetComponent<MeshRenderer>().enabled = active;
        walls[(int)side].layer = active ? 0 : 1;
    }

    public void SetCellActive(bool active)
    {
        isActive = active;

        if (!active)
        {
            isConnectable = true;
            pathIndex = -1;
        }
    }

    public bool HasNeighbor(CellController cell)
    {
        return neighbors.Contains(cell);
    }

    public void AddNeighbor(CellController cell)
    {
        neighbors.Add(cell);
    }

    public void RemoveNeighbor(CellController cell)
    {
        neighbors.Remove(cell);
    }

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

    public int GetOpenWallsCount()
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
}
