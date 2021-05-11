using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public void playAgain()
    {
        Debug.Log("Resuming Play");
        GameObject.Find("World").SendMessage("callToPause");
    }
    public void returnToMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        Destroy(GameObject.FindGameObjectWithTag("Player"));    
        Destroy(GameObject.FindGameObjectWithTag("pauseMenu"));
        Destroy(GameObject.FindGameObjectWithTag("playerUI"));
        Destroy(GameObject.Find("Cursor"));
        Destroy(GameObject.Find("Canvas"));
        SceneManager.LoadScene("MainScreen");
    }
}
