using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurikabe : MonoBehaviour
{
        GameObject player;
        public Animator animator;
        public int maxDistance = 50;

        bool isAwake = false;
        public int XP = 50;

        public int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
            if(!isAwake)
            {
                if(health < 1)
                {
                    player.GetComponent<Player>().levelSystem.AddXP(XP);
                    Destroy(gameObject);
                }
                float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
                if( distanceFromPlayer < maxDistance)
                {
                animator.SetBool("awake", true);
                isAwake = true;
                }
            }
        }

        void takeDamage(int damage)
        {
            health -= damage;
        }
}
