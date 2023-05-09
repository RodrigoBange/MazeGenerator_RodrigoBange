using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleUI, menuUI, gameUI, pauseUI;

    [SerializeField]
    private TextMeshProUGUI timerText, attemptsText;

    [SerializeField]
    private GameObject titleSlime;

    public float timePlayed;
    public bool timerActive;

    public int attemptsCount;

    private void Update()
    {
        if (timerActive)
        {
            timePlayed += Time.deltaTime;
            timerText.text = timePlayed.ToString("00:00");
        }       
    }

    /// <summary>
    /// Tells the MapController to generate a new maze
    /// </summary>
    public void OnGenerateNewMazeClick()
    {
        MazeController.Instance.GenerateMaze(Random.Range(10, 250), Random.Range(10, 250));
    }

    /// <summary>
    /// Enter the game, leave the title screen.
    /// </summary>
    public void OnTitleClick()
    {
        titleSlime.SetActive(false);
        titleUI.SetActive(false);
        menuUI.SetActive(true);
    }

    public void OnStartGameClick()
    {
        //GameManager.Instance.
        attemptsCount = 0;
    }

    public void OnPauseGameClick()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnResumeGameClick()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }    

    public void ActivateTimer()
    {
        timePlayed = 0f;
        timerActive = true;
    }

    public void AddAttempt()
    {
        attemptsCount++;
        attemptsText.text = attemptsCount.ToString();
    }
}
