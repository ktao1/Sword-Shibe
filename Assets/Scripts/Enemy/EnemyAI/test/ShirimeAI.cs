using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShirimeAI : MonoBehaviour
{

    public Transform player;

    private Rigidbody2D rb;

    float upadteTimer;
    public float updateSpeed = 2f;

    float chargeTimer;
    public float chargeSpeed = 2f;

    public bool canAttack;

    public float detectDistance = 10.0f;
    public float attackRange = 5f;

    private enum State
    {
        Romaing,
        ChaseTarget,

    }

    private State state;

    private void Awake()
    {
        RoamPath();
        state = State.Romaing;
    }


    private Seeker seeker;
    public Path path;
    public float speed = 3;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public bool canMove;

    private Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        // roamPosition.position = GetRoamPosition();
        rb = GetComponent<Rigidbody2D>();

        // Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();

        // Start to calculate a new path to the targetPosition object, return the result to the OnPathComplete method.
        // Path requests are asynchronous, so when the OnPathComplete method is called depends on how long it
        // takes to calculate the path. Usually it is called the next frame.

        RoamPath();
        // InvokeRepeating("UpdatePath", 0f, 0.5f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        switch (state)
        {
            default:
            case State.Romaing:
                RoamPath();
                findTarget();
                break;
            case State.ChaseTarget:
                ChasePlayer();
                attack();
                break;
        }
            Move();

    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

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

    private void RoamPath()
    {
        Vector2 roamPosition = startingPosition + new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        UpdatePath(roamPosition);
    }

    private void ChasePlayer()
    {
        UpdatePath(player.position);
    }

    private void findTarget()
    {
        if (Vector2.Distance(transform.position, player.position) < detectDistance)
        {
            updateSpeed = 0.5f;
            state = State.ChaseTarget;
        }
    }

    private void attack()
    {
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            canMove = false;
            canAttack = true;
        }
        // stop and do attack animation;

        if (canAttack)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > chargeSpeed)
            {
                // doing attack
                chargeTimer = 0;
                canMove = true;
                canAttack = false;
            }
        }
    }

    private void Move()
    {
        if (!canMove) {
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

    
    
}
