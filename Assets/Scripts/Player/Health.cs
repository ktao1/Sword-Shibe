using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{ 
    
    GameObject player;
    Player thePlayer;
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite breakedHeart;


    private void Start()
    {
        player = GameObject.Find("Player");
        thePlayer = player.GetComponent<Player>();
    }

    private void Update()
    {
        health = thePlayer.health;
        for (int i = 0; i < hearts.Length; i++)
        {
            if(health > numOfHearts)
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
            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
