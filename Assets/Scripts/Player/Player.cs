using System.Collections;
using System.Collections.Generic;
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

    //Timer counters                  How counters work: set them to 0 and count up to the limit
    private int dashTime;   
    private int immuneTime;
    private int attackTime;

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
      if(Time.timeScale != 0)   //not paused
      {
        manageTimers();     //Put all timer loop stuff in here
        moveAndAttack();    //Groups together move and attack into one function
      }
    }

    void moveAndAttack()
    {
  		float x = Input.GetAxisRaw ("Horizontal");  //Horizontal input 		
	  	float y = Input.GetAxisRaw ("Vertical");    //Vertical input		
      attacking = false;

      if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)) && !dashing)     //Set up dashing
      {
        dashing = true;
        gameObject.layer = immortalLayer;
        dashTime = 0;
        speed *= dashMulti;       //dash speed = 3 x normal speed
      }

		  Vector2 direction = new Vector2 (x, y).normalized;		
		  GetComponent<Rigidbody2D>().velocity = direction * speed;

      animator.ResetTrigger("Attacking");
      animator.ResetTrigger("Dashing");
      animator.ResetTrigger("Damaged");
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
        //  Attacking
        if(dashing && !attacking)
        {
          animator.SetTrigger("Dashing");
        }
      }
    
  public void manageTimers()
  {
    if(immune)
    {
      gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
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
  }

    public void takeDamage(int damage)
    {
      if(!immune && !immortal)
      {
        animator.SetTrigger("Damaged");
        health -= damage;
      
        if(health == 0)  
        {
          Time.timeScale = 0;
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

