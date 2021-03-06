﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ShirimeAI : MonoBehaviour
{
    // get gameobject
    #region GameObject
    // get player transform
    Transform player;
    // get Player Class use for doing damage
    Player _player;
    // get animator
    public Animator animator;
    public GameObject shirimeBullet;
    #endregion

    // get spirte shader
    #region Material
    SpriteRenderer spriteRenderer;
    Material originalMaterial;
    [SerializeField]
    Material flashMaterial;
    Coroutine flashRoutine;
    #endregion

    // Enemy Stateus
    #region enemyStatus

    // enemry attack's damge
    public int damage;
    // enemy health
    public int health;
    // enemy XP
    public int XP;
    public int soul;


    bool canAttack = true;
    bool canHurt = true;
    bool canCharging = false;
    bool canTransformed = true;
    bool death = false;
    public float attackCD;
    public float recoveryCD;
    public float flashDuration;
    // how far enemy can see the player
    public float detectDistance;
    public float attackRange;
    public float FireRate = 1f;
    public float bulletForce = 10f;

    #endregion

    // AI State
    #region AI State
    // Enemy AI 
    private enum State
    {
        Romaing,
        ChaseTarget,
        Transform,
        Attack,
        Hurt,
    }
    private State state;
    #endregion

    // Animation State
    #region Animation State
    string currentState = "Shirime Walk NORMAL";
    const string WALK = "Shirime Walk NORMAL";
    const string TRANSFORM = "Shirime Transform";
    const string CHARGING = "Shirime Charging";
    const string ATTACK = "Shirime Firing";
    const string CRAWL = "Shirime Crawl";
    const string HURT = "hurt";
    #endregion 

    //A* path finding asset
    #region Astar Asset
    // upadteTimer and updateSpees: how often should upadte the path
    float upadteTimer;
    public float updateSpeed;
    private Seeker seeker;
    public Path path;
    // Enemy Movement speed
    public float speed = 1;
    // Enemy movmen determines the distance to the point the AI will move to
    public float nextWaypointDistance;
    private int currentWaypoint;
    public bool reachedEndOfPath;
    public bool canMove = true;
    Vector2 startingPosition;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        defaultSetting();
    }

    // Update is called once per frame
    // Use state to do the enemy AI
    void Update()
    {
        if (!death)
        {
            switch (state)
            {
                default:
                case State.Romaing:
                    // enemy start roaming and try to find the player.
                    RoamPath();
                    break;
                // if enemy find the player, start chase player.
                case State.Transform:
                    ShirimeTransform();
                    break;
                case State.ChaseTarget:
                    ChasePlayer();
                    break;
                case State.Attack:
                    attackLogic();
                    break;
                case State.Hurt:
                    hurt();
                    break;
            }
            findTarget();
            Move();
        }
    }

    // Default setting
    void defaultSetting()
    {
        // find player gameObject
        player = GameObject.Find("Player").transform;
        _player = player.GetComponent<Player>();
        startingPosition = transform.position;

        // Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();

        // set material
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        // start the AI;
        RoamPath();
    }

    // Astart path movement
    #region Astart Path Movement
    // Callback function for UpdatePath(Vector2 position)
    void OnPathComplete(Path p)
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
    void RoamPath()
    {
        Vector2 roamPosition = startingPosition + new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        UpdatePath(roamPosition);
    }

    // update Chasing path
    void ChasePlayer()
    {
        UpdatePath(player.position);
    }

    // Try to find the target
    void findTarget()
    {
        if (canHurt)
        {
            if (Vector2.Distance(transform.position, player.position) < attackRange)
            {
                state = State.Attack;
            }
            else if (Vector2.Distance(transform.position, player.position) < detectDistance)
            {
                updateSpeed = 0.5f;
                if (canTransformed)
                    state = State.Transform;
                else
                    state = State.ChaseTarget;
            }
            else
            {
                updateSpeed = 2f;
                state = State.Romaing;
            }
        }
    }

    // after find the path, Move() actually move the Enemy base on the path
    void Move()
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
    #endregion

    // Shirime transform animation
    #region Transform animation
    void ShirimeTransform()
    {
        canTransformed = false;
        canMove = false;
        ChangeAnimationState(TRANSFORM);
        Invoke("OnTransformFinished", animator.GetCurrentAnimatorStateInfo(0).length);
    }

    void OnTransformFinished()
    {
        ChangeAnimationState(CRAWL);
        canMove = true;
        canCharging = true;
        speed = 2f;
    }

    #endregion

    // Shirime Attack animation
    #region Attack
    // attack state machine
    void attackLogic()
    {
        // stop move and attack
        canMove = false;
        // check does it need to charging;
        if (canCharging)
        {
            canCharging = false;
            // do charging animation
            ChangeAnimationState(CHARGING);
            float charingTime = animator.GetCurrentAnimatorStateInfo(0).length;
            // When charge animation finished
            Invoke("OnChargingFinished", charingTime*2);
        }
    }

    void OnChargingFinished()
    {
        if (canAttack)
        {
            canAttack = false;
            ChangeAnimationState(ATTACK);
            shootBullet();
            Invoke("OnAttackFinished", animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
    void shootBullet()
    {
        Vector3 shootDir = (player.position - transform.position).normalized;
        GameObject bullet = Instantiate(shirimeBullet, transform.position, Quaternion.identity);
        bullet.GetComponent<ShirimeBullet>().setDir(shootDir);
    }

    void OnAttackFinished()
    {
        canMove = true;
        canCharging = true;
        canAttack = true;
        ChangeAnimationState(CRAWL);
    }

    #endregion

    // enemyAI take damage animation
    #region Take Damage
    public void takeDamage(int damage)
    {
        if (canHurt)
        {
            health -= damage;
            state = State.Hurt;
        }
    }

    // function that set hurt animaion
    void hurt()
    {
        if (canHurt)
        {
            CancelInvoke();
            canHurt = false;
            canMove = false;
            ChangeAnimationState(HURT);
            flash();
            Invoke("recovery", recoveryCD);
        }
    }

    // hurt animation CD
    void recovery()
    {
        canMove = true;
        canHurt = true;
        canAttack = true;
        canCharging = true;
        ChangeAnimationState(CRAWL);
        isDeath();
        findTarget();
    }

    // function that makes sprite flash
    void flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    // function detect enemy death
    IEnumerator FlashRoutine()
    {
        // Swap to the fflashMaterial
        spriteRenderer.material = flashMaterial;
        // pause the execution of this function for "flashDuration" seconds
        yield return new WaitForSeconds(flashDuration);

        // after pause, swap back to original material
        spriteRenderer.material = originalMaterial;

        // set the routine to null, signaling that it's finished
        flashRoutine = null;
    }
    void isDeath()
    {
        // if enemy health drop to 0 destory the enemy
        if (health < 1)
        {
            death = true;
            _player.SendMessage("AddXP", XP);
            _player.SendMessage("increaseSoul");
            ChangeAnimationState("POOF");
            this.GetComponent<BoxCollider2D>().enabled = false;
            Invoke("onDeathComplete", 1f);
        }
    }

    void onDeathComplete()
    {
        Destroy(gameObject);
    }
    #endregion


    // function that changes animation
    void ChangeAnimationState(string newState)
    {
        // stop same animation interuuping each other
        if (currentState == newState)
            return;
        // play new animation 
        animator.Play(newState);
        // set the current state
        currentState = newState;
    }

    // Use collider to do the attack
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player" && c.gameObject.layer != 13)
        {
            // c.gameObject.SendMessage("takeDamage", damage);
        }
    }

}
