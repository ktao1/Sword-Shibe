using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Komainu : MonoBehaviour
{
    // get gameobject
    #region GameObject
    // get player transform
    Transform player;
    // get Player Class use for doing damage
    Player _player;
    // get animator
    public Animator animator;
    public Collider2D col2d;
    private Collider2D attackCol;
    public Animator effect;
    public Collider2D effectCol;

    #endregion

    // get spirte shader
    #region Material
    SpriteRenderer spriteRenderer;
    Material originalMaterial;
    [SerializeField]
    Material flashMaterial;
    Coroutine flashRoutine;
    #endregion

    // Enemy Status
    #region EnemyStatus
    // enemy health
    public int health;
    // enemy XP
    public int XP;
    // enemy Soul;
    public int soul;
    // enemy attack's damge
    public int damage;

    bool canAttack = true;
    bool canHurt = true;
    bool canDamage = true;
    bool isAttacking = false;
    bool death = false;


    public float attackCD;
    public float flashDuration;

    public float idleCD = 2f;
    private float idleTimer = 0f;
    public float jumpCD = 1f;
    private float jumpTimer = 0f;
    public float moveCD = 3f;
    private float moveTimer = 0f;
    public float landCD = 1f;
    private float landTimer = 0f;
    public float chargeCD = 1f;
    private float chargeTimer = 0f;
    public float scratchCD = 1f;
    private float scratchTimer = 0f;
    public float roarCD = 2f;
    private float roarTimer = 0f;
    public float recoveryCD = 2f;
    private float recoveryTimer = 0f;
    

    // how far enemy can see the player
    public float detectDistance;
    public float attackRange;
    float attackDelay;
    #endregion

    // AI State 
    #region AI State
    private enum State
    {
        Idle,
        Jump,
        Move,
        Land,
        Charge,
        Attack,
        Scratch,
        Roar,
        Hurt,
    }
    private State state;
    #endregion

    // Animation State
    #region Animation State
    string currentState = "komainu_idle";
    const string IDLE = "komainu_idle";
    const string JUMP = "komainu_jump";
    const string MOVE = "move";
    const string LAND = "komainu_land";
    const string ROAR = "komainu roar";
    const string SCARTCH = "Komainu Scratch";

    const string HOPPING = "hopping";
    const string ATTACKING = "attacking";
    const string HURT = "hurt";
    #endregion

    //A* path finding asset
    #region Astar Asset
    // upadteTimer and updateSpees: how often should upadte the path
    float upadteTimer;
    public float updateSpeed = .5f;

    Seeker seeker;
    public Path path;
    // Enemy Movement speed
    public float speed = 3;
    // Enemy movmen determines the distance to the point the AI will move to
    public float nextWaypointDistance;
    int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public bool canMove = false;
    private Vector2 startingPosition;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        defaultSetting();
        attackCol = this.GetComponent<BoxCollider2D>();
    }

    /*
       Update is called once per frame
       Use state to do the enemy AI
    */
    void Update()
    {
        if (!death)
        {
            switch (state)
            {
                default:
                case State.Idle:
                    idle ();
                    break;
                case State.Jump:
                    jump();
                    break;
                case State.Move:
                    move();
                    break;
                case State.Land:
                    land();
                    break;
                case State.Charge:
                    charge();
                    break;
                case State.Attack:
                    attack();
                    break;
                case State.Scratch:
                    scratch();
                    break;
                case State.Roar:
                    roar();
                    break;
                case State.Hurt:
                    hurt();
                    break;
            }
            //UpdatePath(player.position);
            Move();

            if(!canDamage)
            {
                if(recoveryTimer < recoveryCD)
                {
                    recoveryTimer += Time.deltaTime;
                }
                else
                {
                    recoveryTimer = 0;
                    canDamage = true;
                }
            }
        }
    }

    public void idle()
    {
        ChangeAnimationState(IDLE);
        col2d.enabled = true;
        if(idleTimer < idleCD)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0;
            state = State.Jump;
        }
    }
  
    public void jump()
    {
        ChangeAnimationState(JUMP);
        col2d.enabled = false;
        if(jumpTimer < jumpCD)
        {
            jumpTimer += Time.deltaTime;
        }
        else
        {
            jumpTimer = 0;
            state = State.Move;
        }
    }

    public void move()
    {
        ChangeAnimationState(MOVE);
        canMove = true;
        if(moveTimer < moveCD)
        {
            moveTimer += Time.deltaTime;
            UpdatePath(player.position);
        }
        else if(moveTimer > moveCD)
        {
            moveTimer = 0;
            state = State.Land;
        }
        else if(Vector2.Distance(this.transform.position, player.position) < 1.5f)
        {
            moveTimer = 0;
            state = State.Land;
        }
    }

    public void land()
    {
        ChangeAnimationState(LAND);
        canMove = false;
        if(landTimer < landCD)
        {
            landTimer += Time.deltaTime;
        }
        else
        {
            landTimer = 0;
            state = State.Attack;
        }

    }

    public void charge()
    {
        ChangeAnimationState(IDLE);
        if (chargeTimer < chargeCD)
        {
            chargeTimer += Time.deltaTime;
        }
        else
        {
            chargeTimer = 0;
            state = State.Attack;
        }
    }


    public void attack()
    {
        int x = Random.Range(0, 2);
        isAttacking = true;
        col2d.enabled = true;
        switch (x)
        {
            case 0:
                state = State.Scratch;
                break;
            case 1:
                state = State.Roar;
                break;
        }
    }

    public void scratch()
    {
        ChangeAnimationState(SCARTCH);
        attackCol.enabled = true;
        if (scratchTimer < scratchCD)
        {
            scratchTimer += Time.deltaTime;
        }
        else
        {
            scratchTimer = 0;
            isAttacking = false;
            attackCol.enabled = false;
            state = State.Idle;
        }
    }

    public void roar()
    {
        ChangeAnimationState(ROAR);
        effectCol.enabled = true;   
        effect.Play("roar");
        if (roarTimer < roarCD)
        {
            roarTimer += Time.deltaTime;
        }
        else
        {
            roarTimer = 0;
            effectCol.enabled = false;
            isAttacking = false;
            state = State.Idle;
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

        state = State.Idle;
    }
    // Astar Movement
    #region AstartPathMovement
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

        if (canMove)
        {
            if (Vector2.Distance(transform.position, player.position) < attackRange)
            {
                updateSpeed = 0.1f;
                state = State.Attack;
            }
            else if (Vector2.Distance(transform.position, player.position) < detectDistance)
            {
                updateSpeed = 0.5f;
                state = State.Move;
            }
        }

    }

    // AI move
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

    // AI attack
    #region Attack
    // function that doing attack
    /*
    void attack()
    {
        if (canAttack)
        {
            canAttack = false;
            isAttacking = true;
            speed = 7f;
            ChangeAnimationState(ATTACKING);
            attackDelay = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke("attackComplete", attackDelay);
        }
        ChasePlayer();
    }
    */

    // check attack is totally complete
    void attackComplete()
    {
        speed = 3f;
        ChangeAnimationState(HOPPING);
        isAttacking = false;
        Invoke("waitForCD", attackCD);
    }
    // attack CD
    void waitForCD()
    {
        canAttack = true;
    }
    #endregion

    // AI hurt
    #region take damage
    // function that take the damage
    void takeDamage(int damage)
    {
        if (canDamage)
        {
            health -= damage;
            canDamage = false;
            flash();
            isDeath();
        }
    }

    // function that set hurt animaion
    void hurt()
    {
        if (canHurt)
        {
            canHurt = false;
            flash();
            isDeath();
        }
    }

    // hurt animation CD
    void recovery()
    {
        CancelInvoke();
        isDeath();
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

    // function detect enemy death
    void isDeath()
    {
        // if enemy health drop to 0 destory the enemy
        if (health < 1)
        {
            death = true;
            _player.levelSystem.AddXP(XP);
            _player.soulSystem.AddSoul(soul);
            ChangeAnimationState("POOF");
            col2d.enabled = false;
            effectCol.enabled = false;
            attackCol.enabled = false;
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
        if (c.gameObject.tag == "Player" && c.gameObject.layer != 13 && isAttacking)
        {
            c.gameObject.SendMessage("takeDamage", damage);
        }
    }


}

