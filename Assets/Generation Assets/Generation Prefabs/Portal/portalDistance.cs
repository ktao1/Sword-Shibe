using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalDistance : MonoBehaviour
{
GameObject player;
public Animator animator;
public double viewDistance = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, transform.position)<viewDistance)
        {
            animator.SetBool("playerNear", true);
        }
        else
        {
            animator.SetBool("playerNear", false);
        }
    }
}
