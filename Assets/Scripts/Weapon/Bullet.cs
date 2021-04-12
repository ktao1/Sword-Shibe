using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 10;
    public int damage = 1;
    public float lifelength = .5f;

    void Start ()
    {
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
    }
    void Update()
    {
      
      Destroy(gameObject, lifelength);
    }
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player" && c.gameObject.layer != 13)
        {
            c.gameObject.SendMessage("takeDamage", damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
