using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomOnDeath : MonoBehaviour
{
    Camera deathWalk;
    public bool isDead = false;
    public float baseField = 60;
    public float zoomNum = .1f;
    public float maxZoom = 20f;
    // Start is called before the first frame update
    void Start()
    {
        deathWalk = gameObject.GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            if(deathWalk.fieldOfView > maxZoom)
            {
                deathWalk.fieldOfView -= zoomNum;  
            }
        }
    }
    public void deadZoom()
    {
        isDead = true;
    }
    public void reset()
    {
        isDead = false;
        deathWalk.fieldOfView = 60;
    }
}

