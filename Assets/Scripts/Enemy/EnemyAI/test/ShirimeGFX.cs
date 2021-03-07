using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirimeGFX : MonoBehaviour
{

    private float oldPosition;
    private void Start()
    {
        oldPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // if monster move to the right
        if (transform.position.x > (oldPosition + .3f))
        {
            oldPosition = transform.position.x;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        // if monster move to the left 
        else if (transform.position.x < (oldPosition - .3f))
        {
            oldPosition = transform.position.x;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
