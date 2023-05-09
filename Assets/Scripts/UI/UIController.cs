using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    /// <summary>
    /// Tells the MapController to generate a new maze
    /// </summary>
    public void OnGenerateNewMazeClick()
    {
        MapController.Instance.GenerateMaze(Random.Range(10, 250), Random.Range(10, 250));
    }
}
