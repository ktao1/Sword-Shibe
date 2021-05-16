using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class pauseAndPlay : MonoBehaviour
{
    public static bool gameIsPaused;
    [SerializeField] private GameObject pause;
    float curTime;

    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("pauseMenu");
        pause.SetActive(false);
        curTime = Time.timeScale;
    }


    void Update()
    {
        if(Input.GetKeyDown("p") || Input.GetButtonDown("Pause"))
        {
            callToPause();
            if (pause.activeSelf)
            {
                EventSystem.current.firstSelectedGameObject = GameObject.Find("Resume");
                EventSystem.current.SetSelectedGameObject(GameObject.Find("Resume"));
            }
        }
    }

    void callToPause()
    {
        gameIsPaused = !gameIsPaused;
        PauseGame();
    }

    void PauseGame ()
    {
        if(gameIsPaused)
        {
            pause.SetActive(true);
            AudioListener.pause = true;
            curTime = Time.timeScale = 0f;
        }
        else 
        {
            curTime = Time.timeScale = 1f;
            pause.SetActive(false);
            AudioListener.pause = false;
            
        }
    }

}
