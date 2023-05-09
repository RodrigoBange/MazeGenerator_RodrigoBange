using System.Collections;
using UnityEngine;

public interface IMazeGenerator
{
    IEnumerator CreateMaze(int width, int height, GameObject cellPrefab, Transform parent, GameObject startPrefab, GameObject finishPrefab, GameManager manager);
    void ClearMaze();
    bool IsDone { get; }
}
