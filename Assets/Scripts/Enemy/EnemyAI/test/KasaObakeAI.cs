using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class KasaObakeAI : MonoBehaviour
{
    [SerializeField]
    AIPath aipath;

    [SerializeField]
    float speed = 0.05f;

    [SerializeField]
    float CD = 2f;

    public Transform player;
    private Rigidbody2D rb;

    bool inCD = false;
    float timer = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        attack();

    }


    private void attack()
    {
        // check distance between enemy 
        float distance = Vector2.Distance((Vector2)player.position, rb.position);

        // if in CD don't do anything
        
        if (inCD)
        {
            if (timer < CD)
            {
                timer += Time.deltaTime;
                return;
            }
            else {
                aipath.canMove = true;
                inCD = false;
            }
        }
        

        // if not in CD and distance <= 4 stop A* move and attack the player.

        if (distance <= 4)
        {
            aipath.canMove = false;
            inCD = true;
            timer = 0.0f;
            Vector2 direction = ((Vector2)player.position - rb.position);
            Vector2 velocity = (direction).normalized * speed * distance * Time.deltaTime;
            rb.velocity = velocity;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }

}
