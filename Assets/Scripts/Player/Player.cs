using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    private Player player;
    // Health System
    #region HealthSystem
    public int health = 3;
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

    #region
    // SkillSystem
    private PlayerSkills playerSkills;
    private SkillTree_UI skillTree_UI;
    [SerializeField] private GameObject SkillTree;
    #endregion



    public float speed = 5; 
    public int immortalLayer = 13;  //The layer where the player can't take damage
    public int playerLayer = 12;    //The layer where the player can collide with things the player should collide with
    public AudioClip swordSound;
    private AudioSource source;

    public int attackLimit = 50;
    public int dashLimit = 50;      //How long the player can dash for, timer counter limit
    private int dashMulti = 3;      //When dashing, speed *= dashMulti
    public bool immortal = false;   //Is the game in betatesting mode (can't take damage)
    public int immuneLimit = 100;   //How long the invincibility frames are, timer counter limit
    public GameObject attack;       //Type of attack
    public Animator animator;       //Use when animating, set triggers, bools, ints, and floats for the animations

    //Status bools
    private bool immune = false;
    bool dashing = false;    
    bool attacking = false;
    bool canAttack = true;
    bool canDash = true;
    bool hurt = false;
    bool dead = false;

    //Timer counters                  How counters work: set them to 0 and count up to the limit
    private int dashTime = 0;   
    private int immuneTime = 0;
    private int attackTime = 0;
    private int damageTime = 0;

enum dir
{
  up,
  down,
  left,
  right
}

float curDir = 1;


    private void Awake()
    {
        
    }

    void Start()
    {
        player = this.GetComponent<Player>();

        LevelSystemStartSetting();
        SkillSystemStartSetting();


        source = GetComponent<AudioSource>();

      if(immortal)
      {
        gameObject.layer = immortalLayer;
      }
    }

    void Update ()    //Every frame...
    {

        OpenCloseSkillMenu();

      if (!dead)   //not paused
      {
        UpdateHealth();     //Update player's health
        manageTimers();     //Put all timer loop stuff in here
        moveAndAttack();    //Groups together move and attack into one function
      }
      else
      {
          animator.SetTrigger("Dead");
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
        Debug.Log("Lvel Up!");
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
            case PlayerSkills.SkillType.HealthUp_1:
                SetHealthAmount(5);
                break;
            case PlayerSkills.SkillType.HealthUp_2:
                SetHealthAmount(7);
                break;
            case PlayerSkills.SkillType.HealthUp_3:
                SetHealthAmount(10);
                break;
        }
    }

    private void SetHealthAmount(int HealthAmount)
    {
        numOfHearts = HealthAmount;
        health = numOfHearts;
    }



    #endregion


    void moveAndAttack()
    {
  		float x = Input.GetAxisRaw ("Horizontal");  //Horizontal input 		
	  	float y = Input.GetAxisRaw ("Vertical");    //Vertical input		
      attacking = false;

      if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)) && !dashing && canDash && !attacking)     //Set up dashing
      {
        hurt = false;
        dashing = true;
        gameObject.layer = immortalLayer;
        dashTime = 0;
        speed *= dashMulti;       //dash speed = 3 x normal speed
      }

		  Vector2 direction = new Vector2 (x, y).normalized;		
		  GetComponent<Rigidbody2D>().velocity = direction * speed;


if(x == 0 && y == 0)
{
  animator.SetFloat("curDir", curDir);
}
else
{
      animator.SetFloat("Horizontal", x);
      animator.SetFloat("Vertical", y);
      if(x == 0 && y > 0) curDir = (float)dir.up;
      if(x == 0 && y < 0) curDir = (float)dir.down;
      if(x < 0 && y == 0) curDir = (float)dir.left;
      if(x > 0 && y == 0) curDir = (float)dir.right;

}
        animator.SetInteger("speed", (int)(Mathf.Abs(direction[0])+Mathf.Abs(direction[1])));

        //Animation Setup 
        //      Motion: Prioritize up/down over left/right

        //      Dashing
        if(Input.GetMouseButtonDown(0) && !dashing && canAttack)
        {
            attacking = true;
            
            canAttack = false;
            attackTime = 0;

            hurt = false;
            animator.SetTrigger("Attacking");

            Vector3 attackOffset = transform.position;

            if(x < 0)
            {
            attackOffset[0] -= 1;
            }
            if(x > 0)
            {
            attackOffset[0] += 1;
            }

            if(y < 0)
            {
              attackOffset[1] -= 1;
            }
            if(y > 0)
            {
              attackOffset[1] += 1;
            }

            if(x == 0 && y == 0)
            {
              if(curDir == (float)dir.up)
              {
                attackOffset[1] +=1;
              }
              if(curDir == (float)dir.down)
              {
                attackOffset[1] -=1;
              }
              if(curDir == (float)dir.left)
              {
                attackOffset[0] -= 1;
              }
              if(curDir == (float)dir.right)
              {
                attackOffset[0] += 1;
              }
            }

            Instantiate (attack, attackOffset, transform.rotation);
            source.PlayOneShot(swordSound, 1F);
        }
        else if(dashing && !attacking)
        {
          hurt = false;
          animator.SetTrigger("Dashing");
        }
        else if(hurt)
        {
          animator.SetTrigger("Damaged");
        }
      }

    

    public void manageTimers()
  {
    if(immune)
    {
      gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
      if(immuneTime < immuneLimit)  immuneTime++;
      else
      {
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        
        immune = false;
        if(!dashing && !immune)
        {
        gameObject.layer = playerLayer;
        }
      } 
    }
    if(hurt)
    {
        if(damageTime < immuneLimit)        damageTime++;
        else{
          hurt = false;
        }
    }
    if(dashing)
    {
      if(dashTime < dashLimit) dashTime ++;
      else
      {
        speed /= 3;
        dashing = false;
        if(!immortal && !immune)
        {
        gameObject.layer = playerLayer;
        }
      }
    }
    if(!canAttack)
    {
      if(attackTime < attackLimit) attackTime ++;
      else
      {
        canAttack = true;
      }
    }
    if(!canDash)
    {
      if(dashTime < dashLimit) dashTime ++;
      else
      {
        canDash = true;
      }

    }
  }

    public void takeDamage(int damage)
    {
      if(!immune && !immortal)
      {
        damageTime = 0;
        hurt = true;
        health -= damage;
      
        if(health == 0)  
        {
      animator.ResetTrigger("Attacking");
      animator.ResetTrigger("Dashing");
      animator.ResetTrigger("Damaged");
      animator.SetInteger("speed", 0);
      GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
          dead = true;
          animator.SetTrigger("Dead");
          SceneManager.LoadScene("DeadScreen");
        }
      
        immune = true;
        immuneTime = 0;

        gameObject.layer = immortalLayer;
      }
    }    
void OnTriggerEnter2D(Collider2D col)
{
  if(col.gameObject.tag == "Portal")
  {    
    Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    GameObject.FindWithTag("Editor").SendMessage("nextStage");
  }
}

}

   

