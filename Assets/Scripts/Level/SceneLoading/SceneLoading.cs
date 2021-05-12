using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        //Start Async Scene Load
        StartCoroutine(LoadAsyncOperation());
    }
    
    IEnumerator LoadAsyncOperation()
    {
        //Begin loading next scene
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while(gameLevel.progress < 1)
        {
            progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
