using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sounds[] sounds;

    private bool isPlay = true; 

    public static AudioManager instance;

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "EndingVideo" || SceneManager.GetActiveScene().name == "EndingVideo 1" || SceneManager.GetActiveScene().name == "prologue")
        {
            foreach (Sounds s in sounds)
            {
                if (isPlay)
                {
                    s.source.Stop();
                    isPlay = false;
                }
                
            }
        }
        else
        {
            foreach (Sounds s in sounds)
            {
                if (!isPlay)
                {
                    s.source.Play();
                    isPlay = true;
                }
                
            }
        }
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play (string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
