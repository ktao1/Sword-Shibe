using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class buttonControl : MonoBehaviour
{
    public void playGame() {
        SceneManager.LoadScene("Room1");
    }
 
    public void options() {
        
    }
 
    public void exitGame() {
        Application.Quit();
    }
}
