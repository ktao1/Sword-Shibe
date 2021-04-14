using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseAndPlay : MonoBehaviour
{
int counter = 0;
public static bool gameIsPaused;
void Update()
    {
        if (Input.GetKey("p") && counter == 0)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
            counter ++;
        }
        else if(Input.GetKey("p"))
        {
            counter++;            
        }
        else
        {
            counter = 0;
        }
    }

    void PauseGame ()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }

}
