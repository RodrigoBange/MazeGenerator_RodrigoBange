using System.Collections;
using UnityEngine;

public interface IMazeGenerator
{
    IEnumerator CreateMaze(int width, int height, Transform parent);
    void ClearMaze();
    bool IsDone { get; }
}
