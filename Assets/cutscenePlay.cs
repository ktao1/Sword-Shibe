using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class cutscenePlay : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer cutscene;

    // Start is called before the first frame update
    void Start()
    {
        cutscene = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        cutscene.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
