using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;  
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

    void Start()
    {
      source = GetComponent<AudioSource>();

      if(immortal)
      {
        gameObject.layer = immortalLayer;
      }
    }

    void Update ()    //Every frame...
    {
      if(!dead)   //not paused
      {
        manageTimers();     //Put all timer loop stuff in here
        moveAndAttack();    //Groups together move and attack into one function
      }
      else
      {
          animator.SetTrigger("Dead");
      }
    }

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


      animator.SetFloat("Horizontal", x);
      animator.SetFloat("Vertical", y);

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

            if(x != 0)
            {
            attackOffset[0] += x;
            }
            if(y != 0)
            {
              attackOffset[1] += y;
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
    GameObject.FindWithTag("Editor").SendMessage("nextStage");
  }
}

}

