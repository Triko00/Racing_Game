using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text lapCounterText, bestLapTimeText, currentLapTimeText, positionText, countdownText, goText, raceResultText;

    public GameObject resultsScreen, pauseScreen, trackUnlockedMessage;

    public bool isPaused;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        isPaused = !isPaused;

        pauseScreen.SetActive(isPaused);

        if(isPaused)
        {
            Time.timeScale = 0f;

            AudioListener.pause = true;
            AudioListener.volume = 0;
        } else
        {
            Time.timeScale = 1f;

            AudioListener.pause = false;
            AudioListener.volume = 1;
        }
    }

    public void ExitRace()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        AudioListener.volume = 1;

        RaceManager.instance.ExitRace();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("*Gunclick* quit game *gunshot*");
    }
}
