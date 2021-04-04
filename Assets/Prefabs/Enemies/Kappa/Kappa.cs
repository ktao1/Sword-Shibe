using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kappa : MonoBehaviour
{
    GameObject player;

    public int health = 3;

    public bool spinShot = false;
    public bool followShot = false;
    public bool randomDir = false;
    public int maxDistance = 10;

    public Animator animator;


    int timerShot = 0;
    
    Monster monster;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        monster = GetComponent<Monster> ();
    }

    void Update()
    {
        if(Time.timeScale != 0)
        {
            attackPlayer();
            if(health < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void attackPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        timerShot += 1;
        if( distanceFromPlayer < maxDistance)
        {
            if(timerShot >= monster.shotDelay)
            {    
                animator.SetTrigger("attack");
                timerShot = 0;
            }
        }
    }
    void baseShotFire()
    {
        Transform ShotPattern = transform.GetChild(0);
        for (int i = 0; i < ShotPattern.childCount; i++)    {
            Transform shotPosition = ShotPattern.GetChild(i);
            monster.Shot (shotPosition);
        }
    }

    void takeDamage(int amount)
    {
        health -= amount;
    }
}
