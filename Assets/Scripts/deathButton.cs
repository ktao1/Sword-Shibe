using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathButton : MonoBehaviour
{
    
    public void Retry()
    {
        GameObject player = GameObject.Find("Player");
        Destroy(player);
        SceneManager.LoadScene("MainScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
