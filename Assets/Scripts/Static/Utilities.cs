public static class Utilities
{
    /// <summary>
    /// Returns the opposite side of the given side.
    /// </summary>
    public static Side Opposite(this Side side)
    {
        return (Side)(((int)side + 2) % 4);
    }

    /// <summary>
    /// Returns the side on which the neighbouring wall is.
    /// </summary>
    public static Side GetSide(this CellController cell, CellController neighbour)
    {
        if (neighbour.Position.x == cell.Position.x)
        {
            if (neighbour.Position.z < cell.Position.z)
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
}
