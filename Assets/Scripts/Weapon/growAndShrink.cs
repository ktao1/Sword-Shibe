using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growAndShrink : MonoBehaviour
{
    public int speed = 7;
    public int damage = 1;
    public float lifelength = 1.5f;
    public float growValue = .025f;
    bool grow = true;
    public float growMax = 80;
    int curGrow = 0;

    void Start ()
    {
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
    }

    void Update()
    {
      if(grow)
      {
        curGrow++;
        transform.localScale+= new Vector3 (growValue,growValue,0);
        if(curGrow == growMax)
        {
          grow = false;
        }
      }
      else
      {
        curGrow--;
        transform.localScale-= new Vector3 (growValue,growValue,0);
        if(curGrow == 0)
        {
          grow = true;
        }
      }

      Destroy(gameObject, lifelength);
    }
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player" && c.gameObject.layer != 13)
        {
            c.gameObject.SendMessage("takeDamage", damage);
            Destroy(gameObject);
        }
        else if(c.gameObject.tag == "Decorative")
        {
            Destroy(gameObject);
        }
    }
}
