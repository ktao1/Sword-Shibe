using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(Input.GetKeyDown("p") && Time.timeScale == 1)
        {
            callToPause();
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
            pause.SetActive(false);
            AudioListener.pause = false;
            curTime = Time.timeScale = 1f;
        }
    }

}
