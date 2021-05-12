using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    
    [SerializeField]
    private Slider progressBar;
    
    [SerializeField]
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
        
    }

    private void Update()
    {
        progressBar.value = (float)videoPlayer.time / (float)videoPlayer.length;
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
   
}
