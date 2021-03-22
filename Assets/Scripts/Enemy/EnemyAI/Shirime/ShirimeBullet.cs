using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirimeBullet : MonoBehaviour
{
    public Player player;
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        /*
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Hit player");
            _player.takeDamage(damage);
        }
        */
    }

}