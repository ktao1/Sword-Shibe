using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public void playAgain()
    {
        GameObject.FindWithTag("World").SendMessage("callToPause");
    }
    public void returnToMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));    
        Destroy(GameObject.FindGameObjectWithTag("pauseMenu"));
        Destroy(GameObject.FindGameObjectWithTag("playerUI"));       
        SceneManager.LoadScene("MainScreen");
    }
}
