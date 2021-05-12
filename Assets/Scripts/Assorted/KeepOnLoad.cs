 using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepOnLoad : MonoBehaviour 
 {
     void Awake () {
         DontDestroyOnLoad(this);
     }
    private void Start()
    {
        GameObject test = GameObject.Find("DontDestroyOnLoad");
        //Debug.Log(test);
    }
}

