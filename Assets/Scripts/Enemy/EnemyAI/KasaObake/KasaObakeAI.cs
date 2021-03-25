using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class KasaObakeAI : MonoBehaviour
{
    // get player transform
    Transform player;
    // get Player Class use for doing damage
    Player _player;
    // get animator
    public Animator animator;
    private Rigidbody2D rb;

    // upadteTimer and updateSpees: how often should upadte the path
    float upadteTimer;
    public float updateSpeed = 2f;

    // chargeTimer and ChargeSpeed: how often should enemy attack
    float chargeTimer;
    public float chargeSpeed = 2f;
    public bool canAttack;

    public float attackSpeed = .5f;
    public float attackTimer;

    public float attackCD = 1f;
    public float attackCDTimer;

    // enemry attack's damge
    public int damage = 1;

    // how far enemy can see the player
    public float detectDistance = 5.0f;
    public float attackRange = 3.0f;

    // Enemy AI 
    private enum State
    {
        Romaing,
        ChaseTarget,
        Attack,
    }
    private State state;

    //A* path finding asset
    private Seeker seeker;
    public Path path;
    // Enemy Movement speed
    public float speed = 3;
    // Enemy movmen determines the distance to the point the AI will move to
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public bool canMove = true;

    private Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        startingPosition = transform.position;
        // roamPosition.position = GetRoamPosition();
        rb = GetComponent<Rigidbody2D>();

        // Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();

        RoamPath();

    }

    // Update is called once per frame
    // Use state to do the enemy AI
    void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.Romaing:
                // enemy start roaming and try to find the player.
                RoamPath();
                findTarget();
                break;
            // if enemy find the player, start chase player.
            case State.ChaseTarget:
                ChasePlayer();
                findTarget();
                break;
            case State.Attack:
                attack();
                ChasePlayer();
                break;
        }
        Move();
    }

    // Callback function for UpdatePath(Vector2 position)
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    // find the path that where AI should go.
    // Parameter vector position: where AI should go
    void UpdatePath(Vector2 position)
    {
        upadteTimer += Time.deltaTime;
        if (upadteTimer > updateSpeed)
        {
            upadteTimer = 0;
            if (seeker.IsDone())
                seeker.StartPath(transform.position, position, OnPathComplete);
        }
    }

    // update Roaming path
    private void RoamPath()
    {
        Vector2 roamPosition = startingPosition + new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        UpdatePath(roamPosition);
    }

    // update Chasing path
    private void ChasePlayer()
    {
        UpdatePath(player.position);
    }

    // Try to find the target
    private void findTarget()
    {
        if (Vector2.Distance(transform.position, player.position) < detectDistance)
        {
            updateSpeed = 0.5f;
            state = State.ChaseTarget;
        }

        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            ChasePlayer();
            state = State.Attack;
        }
    }

    private void attack()
    {
        if(attackTimer < attackSpeed)
        {
            speed = 7f;
            animator.SetBool("attack", true);
            attackTimer += Time.deltaTime;
            // attacking
        }
        else
        {
            animator.SetBool("attack", false);
            attackCDTimer += Time.deltaTime;
            canMove = false;
            if(attackCDTimer > attackCD)
            {
                speed = 3f;
                attackTimer = 0;
                attackCDTimer = 0;
                canMove = true;
                state = State.ChaseTarget;
            }
        }
    }

    // after find the path, Move() actually move the Enemy base on the path
    private void Move()
    {
        if (!canMove)
        {
            return;
        }

        if (path == null)
            return;

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;

        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime

        // If you are writing a 2D game you should remove the CharacterController code above and instead move the transform directly by uncommenting the next line
        transform.position += velocity * Time.deltaTime;
    }


    // Use collider to do the attack or be attacked. 
    void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Hit player");
            _player.takeDamage(damage);
        }
        /*
        if (col.gameObject.name == "Swpie")
        {
            Destroy(gameObject);
        }
        */
    }

}
