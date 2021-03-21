using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirimeBulletGFXTurning : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if monster move to the left
        if (rb.velocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x > 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
