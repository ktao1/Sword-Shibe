using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseAndPlay : MonoBehaviour
{
public static bool gameIsPaused;
[SerializeField] private GameObject pause;

void Start()
{
                pause = GameObject.FindGameObjectWithTag("pauseMenu");
                pause.SetActive(false);
}
void Update()
    {
        if(Input.GetKeyDown("p"))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    void PauseGame ()
    {
        if(gameIsPaused)
        {
                pause.SetActive(true);
                AudioListener.pause = true;
            Time.timeScale = 0f;
        }
        else 
        {
                pause.SetActive(false);
                AudioListener.pause = false;
            Time.timeScale = 1;
        }
    }

}
