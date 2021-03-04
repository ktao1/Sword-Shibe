﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseAndPlay : MonoBehaviour
{
public static bool gameIsPaused;

void Update()
    {
        if (Input.GetKey("p"))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
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