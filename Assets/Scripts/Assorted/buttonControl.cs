using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class buttonControl : MonoBehaviour
{

public AudioSource source;
public AudioClip clip;

    public void playGame() {
        source.PlayOneShot(clip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
 
    public void options() {
        
    }
 
    public void exitGame() {
        source.PlayOneShot(clip);
        Application.Quit();
    }
}
