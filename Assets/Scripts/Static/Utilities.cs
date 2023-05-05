using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    /// <summary>
    /// Returns the opposite side of the given side
    /// </summary>
    public static Side Opposite(this Side side)
    {
        return (Side)(((int)side + 2) % 4);
    }

    public static Vector2 TranslateToVector(this Side side)
    {
        switch (side)
        {
            case Side.Top:
                return Vector2.up;
            case Side.Right:
                return Vector2.right;
            case Side.Bottom:
                return Vector2.down;
            case Side.Left:
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }

    /// <summary>
    /// Returns the side on which the neighbouring wall is.
    /// </summary>
    public static Side GetSide(this CellController cell, CellController neighbour)
    {
        if (neighbour.Position.x == cell.Position.x)
        {
            if (neighbour.Position.y < cell.Position.y)
            {
                return Side.Top;
            }
            else
            {
                return Side.Bottom;
            }
        }
        else
        {
            if (neighbour.Position.x > cell.Position.x)
            {
                return Side.Right;
            }
            else
            {
                return Side.Left;
            }
        }
    }

    public static List<CellController> SubtractLists(this List<CellController> list1, List<CellController> list2)
    {
        List<CellController> result = new();
        foreach (var item in list1)
        {
            if (!list2.Contains(item))
            {
                result.Add(item);
            }
        }
        return result;
    }
}
