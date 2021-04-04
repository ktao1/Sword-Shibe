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
    int timerHide;
    public Animator animator;
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

        float newX = playerPosition.transform.position.x + Random.Range(-5, 5) - player.transform.position.x + transform.position.x;
        float newY = playerPosition.transform.position.y + Random.Range(-5, 5) - player.transform.position.y + transform.position.y;

        if (newX < 0)
        {
            newX = 0;
        }
        if (newY < 0)
        {
            newY = 0;
        }

        transform.position = new Vector3(newX, newY, transform.position.z);

        Destroy(playerPosition);
    }
}
