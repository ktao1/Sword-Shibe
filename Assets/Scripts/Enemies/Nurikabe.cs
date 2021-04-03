using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurikabe : MonoBehaviour
{
        GameObject player;
        public Animator animator;
        public int maxDistance = 50;

        bool isAwake = false;

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
                float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
                if( distanceFromPlayer < maxDistance)
                {
                animator.SetBool("awake", true);
                isAwake = true;
                }
            }
        }
}
