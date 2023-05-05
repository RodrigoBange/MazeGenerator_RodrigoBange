using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] wallRenderers;


    [SerializeField]
    private GameObject corners;

    [SerializeField]
    private List<CellController> neighbors = new();

    private Vector2 position = Vector2.zero;

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

    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
            var pos = value;
            pos.y *= -1;
            transform.localPosition = pos;
        }
    }

    public void SetPosition(int x, int y)
    {
        Position = new Vector2(x, y);
    }

    public void ToggleWall(Side side, bool active)
    {
        wallRenderers[(int)side].sortingOrder = active ? 100 : -100;
        wallRenderers[(int)side].gameObject.SetActive(active);
    }

    public void SetCellActive(bool active)
    {
        isActive = active;
        corners.SetActive(active);

        if (!active)
        {
            isConnectable = true;
            pathIndex = -1;
            ToggleWall(Side.Top, true);
            ToggleWall(Side.Right, true);
            ToggleWall(Side.Bottom, true);
            ToggleWall(Side.Left, true);

            foreach (var renderer in wallRenderers)
            {
                renderer.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var renderer in wallRenderers)
            {
                renderer.gameObject.SetActive(true);
            }
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
        return wallRenderers[(int)side].sortingOrder == -100;
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
