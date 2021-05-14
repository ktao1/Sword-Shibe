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

        GameObject.Find("SoundManager").SendMessage("StopPlay");


    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "EndingVideo 1")
            {   
                if(SceneManager.GetActiveScene().name == "prologue")
                {
                    GameObject.Find("SoundManager").SendMessage("Play", "Theme");
                }
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//the scene that you want to load after the video has ended.
            }
            else
            {
                GameObject.Find("SoundManager").SendMessage("Play", "Theme");
                SceneManager.LoadScene(0);
            }
        }
    }
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        if (SceneManager.GetActiveScene().name != "EndingVideo 1") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//the scene that you want to load after the video has ended.
        }
        else
        {
            GameObject.Find("SoundManager").SendMessage("Play", "Theme");
            SceneManager.LoadScene(0);
        }
        
    }
}
