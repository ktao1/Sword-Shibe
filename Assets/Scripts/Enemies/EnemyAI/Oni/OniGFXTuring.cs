using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniGFXTuring : MonoBehaviour
{
    public float offSet = .3f;
    private float oldPosition;
    private void Start()
    {
        oldPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // if monster move to the right
        if (transform.position.x > (oldPosition + offSet))
        {
            oldPosition = transform.position.x;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        // if monster move to the left 
        else if (transform.position.x < (oldPosition - offSet))
        {
            oldPosition = transform.position.x;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
