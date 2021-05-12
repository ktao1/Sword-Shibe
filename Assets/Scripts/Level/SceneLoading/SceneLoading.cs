using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    /*
    [SerializeField]
    private Image progressBar;
    */
    [SerializeField]
    public VideoPlayer videoPLayer;
    // Start is called before the first frame update
    void Start()
    {
        //Start Async Scene Load
        //StartCoroutine(LoadAsyncOperation());
        videoPLayer.loopPointReached += EndReached;

    }
    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator LoadAsyncOperation()
    {
        /*
        //Begin loading next scene
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while(gameLevel.progress < 1)
        {
            progressBar.fillAmount = gameLevel.progress;
            wait();
            yield return new WaitForEndOfFrame();

        }
        
        */
        wait();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForEndOfFrame();
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
    }
}
