using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanuki : MonoBehaviour
{
GameObject player;
    public int maxDistance = 50;
    public int maxHide = 50;
    int attackTimer;
    public int maxAttack = 50;
    bool visible = true;
    public int XP = 50;
    int timerHide;
    public int health = 3;
    public Animator animator;
    int timer = 0;
    int maxTimer = 10000;
    bool seenPlayer = false;
    Monster monster;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        monster = GetComponent<Monster>();
        timerHide = maxHide;
        attackTimer = maxAttack;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if(health < 1)
            {
                player.SendMessage("AddXP", XP);
                Destroy(gameObject);
            }
            else if(maxTimer < timer)
            {
                Destroy(gameObject);
            }
            if(seenPlayer)
            {
            timer++;                
            }
            attackPlayer();
        }
    }

    void attackPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        attackTimer++;
        if (timerHide >= maxHide)
        {
                if (!visible)
                {
                    followPlayer();
                    animator.SetTrigger("Appear");
                    visible = true;
                }
            if (distanceFromPlayer < maxDistance)
            {
                seenPlayer = true;
                if (visible && attackTimer >= maxAttack)
                {
                    animator.SetTrigger("Attack");
                    followShotFire();
                    timerHide = 0;
                    attackTimer = 0;
                }
            }
        }
        else
        {
            visible = false;
            timerHide++;
        }
    }

    void followShotFire()
    {
        GameObject playerPosition = new GameObject();
        playerPosition.transform.position = transform.position;

        Vector3 difference = player.transform.position - playerPosition.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (transform.GetChild(0).childCount > 1)
        {
            Transform ShotPattern = transform.GetChild(0);
            transform.GetChild(0).rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 180f);

            for (int i = 0; i < ShotPattern.childCount; i++)
            {
                monster.Shot(ShotPattern.GetChild(i));
            }
        }
        else
        {
            playerPosition.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 180f);
            monster.Shot(playerPosition.transform);
        }
        Destroy(playerPosition);
    }
    void followPlayer()
    {
        GameObject playerPosition = new GameObject();
        playerPosition.transform.position = transform.position;

        float xMod = Random.Range(-1,1);
        float yMod = Random.Range(-1,1);

        if(xMod>0)  xMod = 1;
        if(xMod<0)  xMod = -1;
        if(yMod>0)  yMod = 1;
        if(yMod<0)  yMod = -1;

        float newX = Random.Range(-5, 5) + xMod * 5 + player.transform.position.x;
        float newY = Random.Range(-5, 5) + yMod * 5 + player.transform.position.y;

        transform.position = new Vector3(newX, newY, transform.position.z);

        Destroy(playerPosition);
    }

    void takeDamage(int damage)
    {
        health -= damage;
    }

}
