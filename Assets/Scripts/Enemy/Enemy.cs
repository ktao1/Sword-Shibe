using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    public bool spinShot = false;
    public bool followShot = false;
    public int maxDistance = 50;

    int curShot = 0;
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
        }
    }

    void attackPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);;

        if( distanceFromPlayer < maxDistance)
        {
            if(timerShot == monster.shotDelay)
            {
                if(followShot)
                {
                    followShotFire();
                }
                else if(spinShot)
                {
                    spinShotFire();
                }
                else
                {
                    baseShotFire();
                }

                timerShot = 0;
            }
            else{
                timerShot += 1;
            }
        }
    }

    void followShotFire()
    {
                            if(transform.GetChild(0).childCount > 1)
                    {
                        Transform ShotPattern = transform.GetChild(0);                    

                        GameObject playerPosition = new GameObject(); 
                        playerPosition.transform.position = transform.position;

                        Vector3 difference = player.transform.position - playerPosition.transform.position;
                        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                        for (int i = 0; i < ShotPattern.childCount; i++)    {
                            Transform shotPosition = ShotPattern.GetChild(i);
                            shotPosition.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ- 90f);
                            monster.Shot (shotPosition);
                        }

                        Destroy(playerPosition);        

                    }
                    else
                    {
                        GameObject playerPosition = new GameObject(); 
                        playerPosition.transform.position = transform.position;

                        Vector3 difference = player.transform.position - playerPosition.transform.position;
                        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                        playerPosition.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ- 90f) ;

                        monster.Shot (playerPosition.transform);             

                        Destroy(playerPosition);        
                    }
    }

    void spinShotFire()
    {
        Transform ShotPattern = transform.GetChild(0);
        Transform shotPosition = ShotPattern.GetChild(curShot);
        monster.Shot (shotPosition);
        curShot ++;
        if(curShot == ShotPattern.childCount)   {
            curShot = 0;
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

}

