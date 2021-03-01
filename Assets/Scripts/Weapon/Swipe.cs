using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public int damage = 1;
    public float lifelength = .3f;


    void Update()
    {
      Destroy(gameObject, lifelength);
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            Destroy (c.gameObject);
            Destroy(gameObject);
        }
    }
}
