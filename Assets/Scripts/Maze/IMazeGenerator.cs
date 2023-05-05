using System.Collections;
using UnityEngine;

public interface IMazeGenerator
{
    IEnumerator CreateMaze(int width, int height, GameObject cellPrefab, Transform parent);
    void ClearMaze();
    bool IsDone { get; }
}
