using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SkipVideo : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer video;
    public int index = 0;
    void Awake()
    {
        video.loopPointReached += CheckOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "EndingVideo")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//the scene that you want to load after the video has ended.
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        if(SceneManager.GetActiveScene().name != "EndingVideo") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//the scene that you want to load after the video has ended.
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        
    }
}
