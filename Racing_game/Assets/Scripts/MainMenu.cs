using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
}
