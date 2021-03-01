using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class setNextScene : MonoBehaviour
{
    bool hasMessaged = false;
	public GameObject Dead_Message;
    public GameObject Portal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject a =  GameObject.FindWithTag("Enemy");
        if(a == null)
        {
            if(!hasMessaged)
            {
            Instantiate(Dead_Message);
            Instantiate(Portal);
            hasMessaged = true;
            }
        }
    }
    void nextStage()
    {
        SceneManager.LoadScene("DeadScreen");
    }
}
