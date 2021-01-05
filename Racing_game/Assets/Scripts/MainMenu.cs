﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject raceSetupPanel, trackSelectPanel, racerSelectPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(RaceInfoManager.instance.trackToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("OH COME OOON... game quit i guess");
    }

    public void OpenRaceSetup()
    {
        raceSetupPanel.SetActive(true);
    }

    public void CloseRaceSetup()
    {
        raceSetupPanel.SetActive(false);
    }

    public void OpenTrackSelect()
    {
        trackSelectPanel.SetActive(true);
        CloseRaceSetup();
    }

    public void CloseTrackSelect()
    {
        trackSelectPanel.SetActive(false);
        OpenRaceSetup();
    }

    public void OpenRacerSelect()
    {
        racerSelectPanel.SetActive(true);
        CloseRaceSetup();
    }
    public void CloseRacerSelect()
    {
        racerSelectPanel.SetActive(false);
        OpenRaceSetup();
    }
}
