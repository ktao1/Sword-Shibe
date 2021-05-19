using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToEpilogue : MonoBehaviour
{
    private GameObject finalBoss;

    // Start is called before the first frame update
    void Start()
    {
        finalBoss = GameObject.Find("Komainu");
    }

    // Update is called once per frame
    void Update()
    {
        if(finalBoss == null)
        {
            Destroy(GameObject.Find("Player"));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}
