using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    //inital component
    private Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    public Transform attackPoint;
    private Player player;
    public Collider2D col2d;

    // Health System
    #region HealthSystem
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite breakedHeart;
    #endregion

    // LevelSystem
    #region Level System
    public LevelSystem levelSystem;
    private LevelSystemAnimator levelSystemAnimator;
    private LevelSystemUI levelSystemUI;
    #endregion

    // SkillSystem
    #region SkillSystem
    // SkillSystem
    private PlayerSkills playerSkills;
    private SkillTree_UI skillTree_UI;
    [SerializeField] private GameObject SkillTree;
    #endregion

    //movement Direction State
    #region MovementDirState

    string dir = "";
    const string D_ = "D_";
    const string L_ = "L_";
    const string R_ = "R_";
    const string U_ = "U_";

    #endregion
    // Animation state
    #region Animation State
    string currentState = "R_Idle";

    // Level Up effect
    const string LEVEL_UP_EFFECT = "LevelUp";

    // Death Animation
    const string DEATH_ANIMATOIN = "Death anim";
    const string DEATH_LOOP = "Death Loop";
/*
    // Down animation
    const string D_ATTACK = "D_Attack";
    const string D_DASH = "D_Dash";
    const string D_HURT = "D_Hurt";
    const string D_IDLE = "D_Idle";
    const string D_RUN = "D_Run";
    // Left aniamtion
    const string L_ATTACK = "L_Attack";
    const string L_DASH = "L_Dash";
    const string L_HURT = "L_Hurt";
    const string L_IDLE = "L_Idle";
    const string L_RUN = "L_Run";
    // Right Animation
    const string R_ATTACK = "R_Attack";
    const string R_DASH = "R_Dash";
    const string R_HURT = "R_Hurt";
    const string R_IDLE = "R_Idle";
    const string R_RUN = "R_Run";
    // Up Animation
    const string U_ATTACK = "U_Attack";
    const string U_DASH = "U_Dash";
    const string U_HURT = "U_Hurt";
    const string U_IDLE = "U_Idle";
    const string U_RUN = "U_Run";
    */
    #endregion


    // Player movement data
    #region MovementDate

    // movement bool
    bool isMoving = false;
    bool isDashing = false;
    bool isAttacking = false;
    bool isTakeingDamage = false;
    bool isDead = false;
    public bool isInvincible = false;

    // movement
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashTimer = 2f;
    private float dashCD = 0f;
    Vector2 movement;

    // attack
    public int damage = 1;
    public float attackSpeed = 2f;
    private float attackCD = 0f;
    public float attackRange = 0.5f;
    public LayerMask[] attackableLayers;

    public Animator effects;


    #endregion

    private void Awake()
    {

    }

    void Start()
    {
        // Get Component
        player = this.GetComponent<Player>();
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        attackPoint = transform.Find("AttackPoint");

        dir = D_;

        LevelSystemStartSetting();
        SkillSystemStartSetting();

    }

    void Update()    //Every frame...
    {
        if (!isDead) {
            OpenCloseSkillMenu();
            UpdateHealth();
            CheckInput();
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            dashCD -= Time.deltaTime;
            attackCD -= Time.deltaTime;

            Move();
            Dash();
        }
    }

    // health System
    #region HealthSystem
    public void UpdateHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (health > numOfHearts)
            {
                health = numOfHearts;
            }

            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = breakedHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    #endregion

    // level System
    #region LevelSystem

    public void LevelSystemStartSetting()
    {
        levelSystemUI = GameObject.Find("LevelSystem_UI").GetComponent<LevelSystemUI>();
        levelSystem = new LevelSystem();
        levelSystemUI.SetLevelSystem(levelSystem);
        levelSystemAnimator = new LevelSystemAnimator(levelSystem);
        levelSystemUI.SetLevelSystemAnimator(levelSystemAnimator);
        player.SetLevelSystem(levelSystem);
        player.SetLevelSystemAnimator(levelSystemAnimator);
    }
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
    }
    public void SetLevelSystemAnimator(LevelSystemAnimator levelSystemAnimator)
    {
        this.levelSystemAnimator = levelSystemAnimator;
        levelSystemAnimator.OnLevelChanged += LevelSystemAnimator_OnLevelChanged;
    }

    private void LevelSystemAnimator_OnLevelChanged(object sender, EventArgs e)
    {
        // set full health
        health = numOfHearts;
        playerSkills.addSkillPoint();

        effects.Play(LEVEL_UP_EFFECT);

    }
    #endregion

    // skill System
    #region SkillSystem
    public void SkillSystemStartSetting()
    {

        playerSkills = new PlayerSkills();
        skillTree_UI = GameObject.Find("SkillTree_UI").GetComponent<SkillTree_UI>();
        skillTree_UI.setPlayerSkills(playerSkills);

        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;


        SkillTree.SetActive(false);
    }


    private void OpenCloseSkillMenu()
    {
        if (Input.GetButtonDown("SkillsTree"))
        {
            SkillTree.SetActive(!SkillTree.activeSelf);
        }
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnskillUnlockedEventArgs e)
    {
        switch (e.skillType)
        {
            // Health
            case PlayerSkills.SkillType.HealthUp_1:
                SetHealthAmount(5);
                break;
            case PlayerSkills.SkillType.HealthUp_2:
                SetHealthAmount(7);
                break;
            case PlayerSkills.SkillType.HealthUp_3:
                SetHealthAmount(10);
                break;
            // DashSpeed
            case PlayerSkills.SkillType.DashFaster_1:
                SetDashCD();
                break;
            case PlayerSkills.SkillType.DashFaster_2:
                SetDashCD();
                break;
            case PlayerSkills.SkillType.DashFaster_3:
                SetDashCD();
                break;
            // Attack Speed
            case PlayerSkills.SkillType.AttackSpeed_1:
                SetAttackSpeed();
                break;
            case PlayerSkills.SkillType.AttackSpeed_2:
                SetAttackSpeed();
                break;
            case PlayerSkills.SkillType.AttackSpeed_3:
                SetAttackSpeed();
                break;
        }
    }

    private void SetHealthAmount(int HealthAmount)
    {
        numOfHearts = HealthAmount;
        health = numOfHearts;
    }

    private void SetDashCD()
    {
        dashTimer *= 1.5f;
    }

    private void SetAttackSpeed()
    {
        attackSpeed *= 1.5f;
    }

    #endregion

    // player movement
    #region Movement
    public void CheckInput()
    {
        if (!isDashing && !isTakeingDamage && !isDead)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");


            if (Input.GetKey(KeyCode.W))
            {
                dir = U_;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir = D_;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                dir = L_;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir = R_;
                isMoving = true;
            }

        }

        if (!isDashing && !isTakeingDamage && !isDead && isMoving && dashCD <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.RightControl))
            {
                isDashing = true;
            }
        }

        if (!isAttacking && !isDashing && !isTakeingDamage && !isDead && attackCD <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isAttacking = true;
            }
        }
    }
    public void Move()
    {
        // basic movement and animation
        // Idle when no movement 
        if (movement.x == 0 && movement.y == 0 && !isDashing && !isAttacking && !isTakeingDamage && !isDead)
        {
            isMoving = false;
            string idleAnimation = dir + "Idle";
            //Debug.Log(idleAnimation);
            ChangeAnimationState(idleAnimation);
        }
        else if (isMoving && !isDashing && !isAttacking && !isTakeingDamage && !isDead)
        {
            string runAnimation = dir + "Run";
            ChangeAnimationState(runAnimation);
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
    public void Dash()
    {
        if (isDashing && !isTakeingDamage && !isDead && isMoving)
        {
            
            string dashAnimation = dir + "Dash";
            ChangeAnimationState(dashAnimation);
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
            Invoke("OnDashComplete", animator.GetCurrentAnimatorStateInfo(0).length * 1.5f);
        }
    }
    public void OnDashComplete()
    {
        CancelInvoke();
        col2d.enabled = true;
        isDashing = false;
        dashCD = 1 / dashTimer;
    }

    #endregion

    // player attack
    #region Attack

    public void Attack()
    {
        if (isAttacking)
        {
            string attackAnimation = dir + "Attack";
            ChangeAnimationState(attackAnimation);
            // detect object in range of attack

            Collider2D[] hitObjects;

            for (int i = 0; i < attackableLayers.Length; i++)
            {
                hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackableLayers[i]);
                foreach (Collider2D attackableObject in hitObjects)
                {
                    attackableObject.SendMessage("takeDamage", damage);
                }
            }
            if (isDashing)
            {
                Invoke("OnAttackComplete", 0f);
            }
            else
            { 
                Invoke("OnAttackComplete", animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
    }

    public void OnAttackComplete()
    {
        if(!isTakeingDamage)
            CancelInvoke();
        isAttacking = false;
        attackCD = 1 / attackSpeed;
    }

    #endregion

    // player taking damge
    #region TakingDamge

    public void takeDamage(int damage)
    {


        if (!isDashing && !isTakeingDamage && !isInvincible)
        {
            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                ChangeAnimationState(DEATH_ANIMATOIN);
                SceneManager.LoadScene("DeadScreen");
            }
            else
            {
                isTakeingDamage = true;
                string hurtAnimation = dir + "Hurt";
                ChangeAnimationState(hurtAnimation);
                sr.color = new Color(255, 0, 0);
                Invoke("OnTakeDamgeComplete", animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
    }

    public void OnTakeDamgeComplete()
    {
        string idleAnimation = dir + "Idle";
        ChangeAnimationState(idleAnimation);
        sr.color = new Color(255, 255, 255);
        isTakeingDamage = false;
    }

    #endregion


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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Portal")
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            GameObject.FindWithTag("Editor").SendMessage("nextStage");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && isDashing)
        {
            col2d.enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}